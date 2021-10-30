using System;
using System.Collections.Generic;
using Minecraft.Configurations;
using Minecraft.Lua;
using Minecraft.Rendering.Jobs;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using static Minecraft.Rendering.RenderingUtility;
using static Minecraft.WorldConsts;
using static Unity.Mathematics.math;

namespace Minecraft.Rendering
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SectionMeshManager))]
    public class SectionRenderingManager : MonoBehaviour, ILuaCallCSharp
    {
        [Serializable]
        public class RenderingSetting
        {
            [Range(4, 16)] public int RenderingRadius = 9;
            [Range(0, 0.5f)] public float MinLightLimit = 0.1f;
            [Range(0.5f, 1)] public float MaxLightLimit = 1;
            public ShadowCastingMode CastShadows = ShadowCastingMode.Off;
            public bool ReceiveShadows = false;
            public bool EnableDestroyEffect = true;
            public Texture2DArray DigProgressTexture;
            public int DigProgressTextureCount = 10;
        }

        [SerializeField] private RenderingSetting m_Setting;
        [SerializeField] private int m_TargetLayer;

        private Transform m_MainCameraTransform;
        private BlockTable m_BlockTable;
        private NativeArray<float4> m_FrustumPlanes;
        private NativeList<int> m_VisibleSectionIndices;
        private List<(Mesh mesh, Vector3Int pos)> m_RenderMeshBuffer;
        private int3 m_SectionSize;
        private int3 m_SectionOffset;
        private NativeArray<int3> m_Sections;
        private SectionMeshManager m_MeshManager;
        private IWorld m_World;
        private bool m_Initialized;

        public int DigProgressTextureCount => m_Setting.DigProgressTextureCount;

        public void Initialize(IWorld world)
        {
            m_MainCameraTransform = world.MainCamera.transform;
            m_BlockTable = world.BlockDataTable;
            m_FrustumPlanes = new NativeArray<float4>(FrustumPlaneCount, Allocator.Persistent);
            m_VisibleSectionIndices = new NativeList<int>(Allocator.Persistent);
            m_RenderMeshBuffer = new List<(Mesh mesh, Vector3Int pos)>();
            m_SectionSize = int3(ChunkWidth, SectionHeight, ChunkWidth);
            m_SectionOffset = default;
            InitSections();
            m_MeshManager = GetComponent<SectionMeshManager>();
            m_World = world;
            m_Initialized = true;

            m_MeshManager.Initialize(world);

            ShaderUtility.BlockTextures = m_BlockTable.GetTextureArray();
            ShaderUtility.DigProgressTextures = m_Setting.DigProgressTexture;
            ShaderUtility.RenderDistance = m_Setting.RenderingRadius * ChunkWidth;
            ShaderUtility.LightLimits = new Vector2(m_Setting.MinLightLimit, m_Setting.MaxLightLimit);
        }

        private void InitSections()
        {
            int radius = m_Setting.RenderingRadius;
            int width = radius * 2 + 1;
            int length = width * width * SectionCountInChunk;
            m_Sections = new NativeArray<int3>(length, Allocator.Persistent);

            for (int x = -radius; x <= radius; x++)
            {
                for (int z = -radius; z <= radius; z++)
                {
                    for (int i = 0; i < SectionCountInChunk; i++)
                    {
                        m_Sections[--length] = int3(x * ChunkWidth, GetSectionY(i), z * ChunkWidth);
                    }
                }
            }

            Debug.Assert(length == 0);
        }

        private void Update()
        {
            if (!m_Initialized)
            {
                return;
            }

            CalculateSectionOffset();
            FrustumCulling();
            RenderMeshes();
        }

        private void OnDestroy()
        {
            m_FrustumPlanes.Dispose();
            m_VisibleSectionIndices.Dispose();
            m_Sections.Dispose();
            m_Initialized = false;
        }

        public void MarkBlockMeshDirty(int x, int y, int z, ModificationSource source)
        {
            Vector3Int section = GetSection(x, y, z);

            // 如果是内部系统进行的更新，那么没有必要，等待玩家走近后再一起生成
            bool forceLoadMesh = source == ModificationSource.PlayerAction;

            // 被标记为 important 后，会在主线程进行更新，量大的话容易卡
            bool important = source == ModificationSource.PlayerAction;

            m_MeshManager.MarkSectionMeshDirty(section, forceLoadMesh, important);

            if (x == section.x)
            {
                m_MeshManager.MarkSectionMeshDirty(section - new Vector3Int(ChunkWidth, 0, 0), forceLoadMesh, important);
            }
            else if (x == section.x + ChunkWidth - 1)
            {
                m_MeshManager.MarkSectionMeshDirty(section + new Vector3Int(ChunkWidth, 0, 0), forceLoadMesh, important);
            }

            if (y == section.y && section.y > 0)
            {
                m_MeshManager.MarkSectionMeshDirty(section - new Vector3Int(0, SectionHeight, 0), forceLoadMesh, important);
            }
            else if (y == section.y + SectionHeight - 1 && section.y < ChunkHeight - SectionHeight)
            {
                m_MeshManager.MarkSectionMeshDirty(section + new Vector3Int(0, SectionHeight, 0), forceLoadMesh, important);
            }

            if (z == section.z)
            {
                m_MeshManager.MarkSectionMeshDirty(section - new Vector3Int(0, 0, ChunkWidth), forceLoadMesh, important);
            }
            else if (z == section.z + ChunkWidth - 1)
            {
                m_MeshManager.MarkSectionMeshDirty(section + new Vector3Int(0, 0, ChunkWidth), forceLoadMesh, important);
            }
        }

        private void CalculateSectionOffset()
        {
            Profiler.BeginSample("SectionRenderingManager.CalculateSectionOffset");
            Vector3 player = m_World.PlayerTransform.position;
            ChunkPos playerChunk = ChunkPos.GetFromAny(player.x, player.z);
            m_SectionOffset = int3(playerChunk.X, 0, playerChunk.Z);
            Profiler.EndSample();
        }

        private void FrustumCulling()
        {
            Profiler.BeginSample("SectionRenderingManager.FrustumCulling");
            m_VisibleSectionIndices.Clear();
            CalculateFrustumPlanes(m_World.MainCamera, m_MainCameraTransform, m_FrustumPlanes);

            FrustumCullingJob job = new FrustumCullingJob
            {
                Planes = m_FrustumPlanes,
                Sections = m_Sections,
                SectionOffset = m_SectionOffset,
                SectionSize = m_SectionSize
            };

            job.ScheduleAppend(m_VisibleSectionIndices, m_Sections.Length, SectionCountInChunk).Complete();
            Profiler.EndSample();
        }

        private Mesh FindSectionMesh(int sectionIndex, out Vector3Int position)
        {
            Profiler.BeginSample("SectionRenderingManager.FindSectionMesh");
            int3 section = m_Sections[sectionIndex] + m_SectionOffset;
            position = new Vector3Int(section.x, section.y, section.z);
            Mesh result = m_MeshManager.GetSectionMesh(position);
            Profiler.EndSample();
            return result;
        }

        private void RenderMeshes()
        {
            Profiler.BeginSample("SectionRenderingManager.RenderMeshes");

            int subMeshCount = m_BlockTable.MaterialCount;

            if (subMeshCount <= 0)
            {
                return;
            }

            m_RenderMeshBuffer.Clear();
            Material material = m_BlockTable.GetMaterial(0);

            for (int i = 0; i < m_VisibleSectionIndices.Length; i++)
            {
                Mesh mesh = FindSectionMesh(m_VisibleSectionIndices[i], out Vector3Int pos);

                if (mesh.subMeshCount != subMeshCount)
                {
                    continue;
                }

                m_RenderMeshBuffer.Add((mesh, pos));
                RenderSectionMesh(mesh, 0, pos, material, m_TargetLayer, m_World.MainCamera, m_Setting.CastShadows, m_Setting.ReceiveShadows);
            }

            for (int i = 1; i < subMeshCount; i++)
            {
                material = m_BlockTable.GetMaterial(i);

                for (int j = 0; j < m_RenderMeshBuffer.Count; j++)
                {
                    (Mesh mesh, Vector3Int pos) = m_RenderMeshBuffer[j];
                    RenderSectionMesh(mesh, i, pos, material, m_TargetLayer, m_World.MainCamera, m_Setting.CastShadows, m_Setting.ReceiveShadows);
                }
            }

            Profiler.EndSample();
        }

        private void RenderSectionMesh(Mesh mesh, int subMeshIndex, Vector3 position, Material material, int layer, Camera camera, ShadowCastingMode castShadows, bool receiveShadows)
        {
            Profiler.BeginSample("SectionRenderingManager.RenderSectionMesh");

            Matrix4x4 matrix = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);
            SubMeshDescriptor subMesh = mesh.GetSubMesh(subMeshIndex);

            if (subMesh.indexCount > 0)
            {
                Graphics.DrawMesh(mesh, matrix, material, layer, camera, subMeshIndex, null, castShadows, receiveShadows, null, false);
            }

            Profiler.EndSample();
        }
    }
}