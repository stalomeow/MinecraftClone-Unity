using Minecraft.BlocksData;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using static Minecraft.BlocksData.BlockVertexHelper;
using static Minecraft.WorldConsts;

namespace Minecraft
{
    public sealed partial class Chunk
    {
        [Flags]
        private enum MeshDirtyFlags : byte
        {
            Neither = 0,

            SolidMesh = 1 << 0,
            LiquidMesh = 1 << 1,

            Both = SolidMesh | LiquidMesh
        }

        private sealed class MeshDataBuffer
        {
            public int SectionIndex { get; private set; }

            public List<VertexData> VertexBuffer { get; }

            public List<int> TriangleBuffer { get; }

            public MeshDataBuffer()
            {
                SectionIndex = 0;
                VertexBuffer = new List<VertexData>();
                TriangleBuffer = new List<int>();
            }

            public void Reset(int sectionIndex)
            {
                SectionIndex = sectionIndex;

                VertexBuffer.Clear();
                TriangleBuffer.Clear();
            }
        }


        private static readonly Bounds s_SectionBounds = new Bounds(
            new Vector3(ChunkWidth >> 1, SectionHeight >> 1, ChunkWidth >> 1),
            new Vector3(ChunkWidth, SectionHeight, ChunkWidth)
        );


        public void RenderChunk(Material solidMaterial, Material liquidMaterial, Camera camera, MaterialPropertyBlock solidProp, MaterialPropertyBlock liquidProp)
        {
            for (int i = 0; i < SectionCountInChunk; i++)
            {
                Vector3 pos = new Vector3(PositionX, i * SectionHeight, PositionZ);

                Mesh mesh = m_SolidMeshes[i];

                if (mesh)
                {
                    Graphics.DrawMesh(mesh, pos, Quaternion.identity, solidMaterial, BlockLayer, camera, 0, solidProp, false, false, false);
                }

                mesh = m_LiquidMeshes[i];

                if (mesh)
                {
                    Graphics.DrawMesh(mesh, pos, Quaternion.identity, liquidMaterial, BlockLayer, camera, 0, liquidProp, false, false, false);
                }
            }
        }

        public async void StartBuildMesh()
        {
            if (m_IsBuildingMesh || !ShouldUpdateMesh)
                return;

            m_IsBuildingMesh = true;

            MeshDirtyFlags updateFlags = m_MeshDirtyFlags;
            m_MeshDirtyFlags = MeshDirtyFlags.Neither;

            ushort dirtyIndexes = m_DirtyMeshIndexes;
            m_DirtyMeshIndexes = ushort.MinValue;

            try
            {
                Monitor.Enter(m_SyncLock);

                if (dirtyIndexes > ushort.MinValue)
                {
                    bool updateSolid = (updateFlags & MeshDirtyFlags.SolidMesh) == MeshDirtyFlags.SolidMesh;
                    bool updateLiquid = (updateFlags & MeshDirtyFlags.LiquidMesh) == MeshDirtyFlags.LiquidMesh;

                    if (m_IsStartUp)
                    {
                        m_IsStartUp = false;

                        await WorldManager.Active.ChunkManager.WaitForAllNeighborChunksLoaded(this);
                    }

                    await Task.Factory.StartNew(GenerateSkyLightData);

                    for (int i = 0; i < SectionCountInChunk; i++)
                    {
                        if ((dirtyIndexes & (1 << i)) == 0)
                            continue;

                        if (updateSolid)
                        {
                            m_MeshDataBuffer.Reset(i);

                            await Task.Factory.StartNew(BuildSolidMeshData, m_MeshDataBuffer);

                            SetMeshData(m_MeshDataBuffer, ref m_SolidMeshes[i]);
                        }

                        if (updateLiquid)
                        {
                            m_MeshDataBuffer.Reset(i);

                            await Task.Factory.StartNew(BuildLiquidMeshData, m_MeshDataBuffer);

                            SetMeshData(m_MeshDataBuffer, ref m_LiquidMeshes[i]);
                        }
                    }
                }
            }
#if !UNITY_EDITOR
            catch
            {
            }
#endif
            finally
            {
                Monitor.Exit(m_SyncLock);
                m_IsBuildingMesh = false;
            }
        }

        private void BuildSolidMeshData(object state)
        {
            MeshDataBuffer buffer = state as MeshDataBuffer;
            WorldManager world = WorldManager.Active;

            List<VertexData> vertexBuffer = buffer.VertexBuffer;
            List<int> triangleBuffer = buffer.TriangleBuffer;

            Profiler.BeginSample("Build Solid Mesh");

            try
            {
                if (m_SolidCounts[buffer.SectionIndex] <= 0)
                    return;

                for (int dx = 0; dx < ChunkWidth; dx++)
                {
                    for (int dz = 0; dz < ChunkWidth; dz++)
                    {
                        int rx = PositionX + dx;
                        int rz = PositionZ + dz;

                        int maxY = GetHighestNonAirYPrivate(dx, dz);
                        int baseY = buffer.SectionIndex * SectionHeight;

                        for (int dy = 0; dy < SectionHeight; dy++)
                        {
                            int ry = baseY + dy;

                            if (ry > maxY)
                                break;

                            BlockType type = GetBlockTypePrivateUnchecked(dx, ry, dz);
                            Block block = world.DataManager.GetBlockByType(type);

                            if (block.VertexType == BlockVertexType.None || block.HasAnyFlag(BlockFlags.Liquid))
                                continue;

                            switch (block.VertexType)
                            {
                                case BlockVertexType.Cube:
                                    {
                                        float light = block.LightValue * OverMaxLight;

                                        if (world.IsBlockTransparent(rx + 1, ry, rz))
                                        {
                                            float lb = (world.GetFinalLightLevel(rx + 1, ry, rz) + world.GetFinalLightLevel(rx + 1, ry - 1, rz) + world.GetFinalLightLevel(rx + 1, ry, rz - 1) + world.GetFinalLightLevel(rx + 1, ry - 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rb = (world.GetFinalLightLevel(rx + 1, ry, rz) + world.GetFinalLightLevel(rx + 1, ry - 1, rz) + world.GetFinalLightLevel(rx + 1, ry, rz + 1) + world.GetFinalLightLevel(rx + 1, ry - 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float rt = (world.GetFinalLightLevel(rx + 1, ry, rz) + world.GetFinalLightLevel(rx + 1, ry + 1, rz) + world.GetFinalLightLevel(rx + 1, ry, rz + 1) + world.GetFinalLightLevel(rx + 1, ry + 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float lt = (world.GetFinalLightLevel(rx + 1, ry, rz) + world.GetFinalLightLevel(rx + 1, ry + 1, rz) + world.GetFinalLightLevel(rx + 1, ry, rz - 1) + world.GetFinalLightLevel(rx + 1, ry + 1, rz - 1)) * 0.25f * OverMaxLight;

                                            AddCubeVertexTriangles(triangleBuffer, vertexBuffer.Count);
                                            AddCubeVertexDataPX(dx, dy, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, vertexBuffer);
                                        }

                                        if (world.IsBlockTransparent(rx - 1, ry, rz))
                                        {
                                            float lb = (world.GetFinalLightLevel(rx - 1, ry, rz) + world.GetFinalLightLevel(rx - 1, ry - 1, rz) + world.GetFinalLightLevel(rx - 1, ry, rz + 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float rb = (world.GetFinalLightLevel(rx - 1, ry, rz) + world.GetFinalLightLevel(rx - 1, ry - 1, rz) + world.GetFinalLightLevel(rx - 1, ry, rz - 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rt = (world.GetFinalLightLevel(rx - 1, ry, rz) + world.GetFinalLightLevel(rx - 1, ry + 1, rz) + world.GetFinalLightLevel(rx - 1, ry, rz - 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float lt = (world.GetFinalLightLevel(rx - 1, ry, rz) + world.GetFinalLightLevel(rx - 1, ry + 1, rz) + world.GetFinalLightLevel(rx - 1, ry, rz + 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz + 1)) * 0.25f * OverMaxLight;

                                            AddCubeVertexTriangles(triangleBuffer, vertexBuffer.Count);
                                            AddCubeVertexDataNX(dx, dy, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, vertexBuffer);
                                        }

                                        if (world.IsBlockTransparent(rx, ry + 1, rz))
                                        {
                                            float lb = (world.GetFinalLightLevel(rx, ry + 1, rz) + world.GetFinalLightLevel(rx, ry + 1, rz - 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz) + world.GetFinalLightLevel(rx - 1, ry + 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rb = (world.GetFinalLightLevel(rx, ry + 1, rz) + world.GetFinalLightLevel(rx, ry + 1, rz - 1) + world.GetFinalLightLevel(rx + 1, ry + 1, rz) + world.GetFinalLightLevel(rx + 1, ry + 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rt = (world.GetFinalLightLevel(rx, ry + 1, rz) + world.GetFinalLightLevel(rx, ry + 1, rz + 1) + world.GetFinalLightLevel(rx + 1, ry + 1, rz) + world.GetFinalLightLevel(rx + 1, ry + 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float lt = (world.GetFinalLightLevel(rx, ry + 1, rz) + world.GetFinalLightLevel(rx, ry + 1, rz + 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz) + world.GetFinalLightLevel(rx - 1, ry + 1, rz + 1)) * 0.25f * OverMaxLight;

                                            AddCubeVertexTriangles(triangleBuffer, vertexBuffer.Count);
                                            AddCubeVertexDataPY(dx, dy, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, vertexBuffer);
                                        }

                                        if (ry > 0 && world.IsBlockTransparent(rx, ry - 1, rz))
                                        {
                                            float lb = (world.GetFinalLightLevel(rx, ry - 1, rz) + world.GetFinalLightLevel(rx, ry - 1, rz - 1) + world.GetFinalLightLevel(rx + 1, ry - 1, rz) + world.GetFinalLightLevel(rx + 1, ry - 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rb = (world.GetFinalLightLevel(rx, ry - 1, rz) + world.GetFinalLightLevel(rx, ry - 1, rz - 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz) + world.GetFinalLightLevel(rx - 1, ry - 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rt = (world.GetFinalLightLevel(rx, ry - 1, rz) + world.GetFinalLightLevel(rx, ry - 1, rz + 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz) + world.GetFinalLightLevel(rx - 1, ry - 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float lt = (world.GetFinalLightLevel(rx, ry - 1, rz) + world.GetFinalLightLevel(rx, ry - 1, rz + 1) + world.GetFinalLightLevel(rx + 1, ry - 1, rz) + world.GetFinalLightLevel(rx + 1, ry - 1, rz + 1)) * 0.25f * OverMaxLight;

                                            AddCubeVertexTriangles(triangleBuffer, vertexBuffer.Count);
                                            AddCubeVertexDataNY(dx, dy, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, vertexBuffer);
                                        }

                                        if (world.IsBlockTransparent(rx, ry, rz + 1))
                                        {
                                            float lb = (world.GetFinalLightLevel(rx, ry, rz + 1) + world.GetFinalLightLevel(rx, ry - 1, rz + 1) + world.GetFinalLightLevel(rx + 1, ry, rz + 1) + world.GetFinalLightLevel(rx + 1, ry - 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float rb = (world.GetFinalLightLevel(rx, ry, rz + 1) + world.GetFinalLightLevel(rx, ry - 1, rz + 1) + world.GetFinalLightLevel(rx - 1, ry, rz + 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float rt = (world.GetFinalLightLevel(rx, ry, rz + 1) + world.GetFinalLightLevel(rx, ry + 1, rz + 1) + world.GetFinalLightLevel(rx - 1, ry, rz + 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float lt = (world.GetFinalLightLevel(rx, ry, rz + 1) + world.GetFinalLightLevel(rx, ry + 1, rz + 1) + world.GetFinalLightLevel(rx + 1, ry, rz + 1) + world.GetFinalLightLevel(rx + 1, ry + 1, rz + 1)) * 0.25f * OverMaxLight;

                                            AddCubeVertexTriangles(triangleBuffer, vertexBuffer.Count);
                                            AddCubeVertexDataPZ(dx, dy, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, vertexBuffer);
                                        }

                                        if (world.IsBlockTransparent(rx, ry, rz - 1))
                                        {
                                            float lb = (world.GetFinalLightLevel(rx, ry, rz - 1) + world.GetFinalLightLevel(rx, ry - 1, rz - 1) + world.GetFinalLightLevel(rx - 1, ry, rz - 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rb = (world.GetFinalLightLevel(rx, ry, rz - 1) + world.GetFinalLightLevel(rx, ry - 1, rz - 1) + world.GetFinalLightLevel(rx + 1, ry, rz - 1) + world.GetFinalLightLevel(rx + 1, ry - 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rt = (world.GetFinalLightLevel(rx, ry, rz - 1) + world.GetFinalLightLevel(rx, ry + 1, rz - 1) + world.GetFinalLightLevel(rx + 1, ry, rz - 1) + world.GetFinalLightLevel(rx + 1, ry + 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float lt = (world.GetFinalLightLevel(rx, ry, rz - 1) + world.GetFinalLightLevel(rx, ry + 1, rz - 1) + world.GetFinalLightLevel(rx - 1, ry, rz - 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz - 1)) * 0.25f * OverMaxLight;

                                            AddCubeVertexTriangles(triangleBuffer, vertexBuffer.Count);
                                            AddCubeVertexDataNZ(dx, dy, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, vertexBuffer);
                                        }
                                    }
                                    break;
                                case BlockVertexType.PerpendicularQuads:
                                    {
                                        float light = GetFinalLightLevelPrivate(dx, ry, dz) * OverMaxLight;

                                        AddPerpendicularQuadsTriangles(triangleBuffer, vertexBuffer.Count);
                                        AddPerpendicularQuadsVertexDataFirstQuad(dx, dy, dz, light, block, vertexBuffer);

                                        AddPerpendicularQuadsTriangles(triangleBuffer, vertexBuffer.Count);
                                        AddPerpendicularQuadsVertexDataSecondQuad(dx, dy, dz, light, block, vertexBuffer);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
#if !UNITY_EDITOR
            catch
            {
                
            }
#endif
            finally
            {
                Profiler.EndSample();
            }
        }

        private void BuildLiquidMeshData(object state)
        {
            MeshDataBuffer buffer = state as MeshDataBuffer;
            WorldManager world = WorldManager.Active;

            List<VertexData> vertexBuffer = buffer.VertexBuffer;
            List<int> triangleBuffer = buffer.TriangleBuffer;

            Profiler.BeginSample("Build Liquid Mesh");

            try
            {
                if (m_LiquidCounts[buffer.SectionIndex] <= 0)
                    return;

                for (int dx = 0; dx < ChunkWidth; dx++)
                {
                    for (int dz = 0; dz < ChunkWidth; dz++)
                    {
                        int rx = PositionX + dx;
                        int rz = PositionZ + dz;

                        int maxY = GetHighestNonAirYPrivate(dx, dz);
                        int baseY = buffer.SectionIndex * SectionHeight;

                        for (int dy = 0; dy < SectionHeight; dy++)
                        {
                            int ry = baseY + dy;

                            if (ry > maxY)
                                break;

                            //if (ry == 0)
                            //    continue;

                            BlockType type = GetBlockTypePrivateUnchecked(dx, ry, dz);
                            Block block = world.DataManager.GetBlockByType(type);

                            if (block.VertexType != BlockVertexType.Cube || !block.HasAllFlags(BlockFlags.Liquid))
                                continue;

                            float light = GetFinalLightLevelPrivate(dx, ry, dz) * OverMaxLight;

                            if (world.IsBlockTransparentAndNotWater(rx + 1, ry, rz))
                            {
                                AddCubeVertexTriangles(triangleBuffer, vertexBuffer.Count);
                                AddCubeVertexDataPX(dx, dy, dz, light, light, light, light, block, vertexBuffer);
                            }

                            if (world.IsBlockTransparentAndNotWater(rx - 1, ry, rz))
                            {
                                AddCubeVertexTriangles(triangleBuffer, vertexBuffer.Count);
                                AddCubeVertexDataNX(dx, dy, dz, light, light, light, light, block, vertexBuffer);
                            }

                            //if (world.IsBlockTransparentAndNotWater(rx, ry + 1, rz))
                            if (world.GetBlockType(rx, ry + 1, rz) != BlockType.Water)
                            {
                                AddCubeVertexTriangles(triangleBuffer, vertexBuffer.Count);
                                AddCubeVertexDataPY(dx, dy, dz, light, light, light, light, block, vertexBuffer);
                            }

                            if (world.IsBlockTransparentAndNotWater(rx, ry - 1, rz))
                            {
                                AddCubeVertexTriangles(triangleBuffer, vertexBuffer.Count);
                                AddCubeVertexDataNY(dx, dy, dz, light, light, light, light, block, vertexBuffer);
                            }

                            if (world.IsBlockTransparentAndNotWater(rx, ry, rz + 1))
                            {
                                AddCubeVertexTriangles(triangleBuffer, vertexBuffer.Count);
                                AddCubeVertexDataPZ(dx, dy, dz, light, light, light, light, block, vertexBuffer);
                            }

                            if (world.IsBlockTransparentAndNotWater(rx, ry, rz - 1))
                            {
                                AddCubeVertexTriangles(triangleBuffer, vertexBuffer.Count);
                                AddCubeVertexDataNZ(dx, dy, dz, light, light, light, light, block, vertexBuffer);
                            }
                        }
                    }
                }
            }
#if !UNITY_EDITOR
            catch
            {

            }
#endif
            finally
            {
                Profiler.EndSample();
            }
        }

        private static void SetMeshData(MeshDataBuffer buffer, ref Mesh mesh)
        {
            List<VertexData> vertexBuffer = buffer.VertexBuffer;
            List<int> triangleBuffer = buffer.TriangleBuffer;

            if (vertexBuffer.Count > 0 && triangleBuffer.Count > 0)
            {
                if (mesh)
                {
                    mesh.Clear();
                }
                else
                {
                    Interlocked.Exchange(ref mesh, new Mesh
                    {
                        indexFormat = SystemInfo.supports32bitsIndexBuffer ? IndexFormat.UInt32 : IndexFormat.UInt16,
                        bounds = s_SectionBounds
                    });
                    mesh.MarkDynamic();
                }

                mesh.SetVertexBufferParams(vertexBuffer.Count, VertexLayout);
                mesh.SetVertexBufferData(vertexBuffer, 0, 0, vertexBuffer.Count);
                mesh.SetTriangles(triangleBuffer, 0);
                mesh.UploadMeshData(false);
            }
        }
    }
}