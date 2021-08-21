using System;
using System.Collections.Generic;
using Minecraft.Lua;
using UnityEngine;
using UnityEngine.Profiling;
using static Minecraft.Rendering.RenderingUtility;

namespace Minecraft.Rendering
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SectionMeshWorkScheduler))]
    public class SectionMeshManager : MonoBehaviour, ILuaCallCSharp
    {
        [SerializeField] [Range(10, 10000)] private int m_BuildNotImportantMeshPreFrame = 10;

        private Stack<Mesh> m_SectionMeshPool;
        private Dictionary<Vector3Int, Mesh> m_SectionMeshMap;
        private HashSet<Vector3Int> m_DirtySections;
        private Queue<Vector3Int> m_NotImportantDirtySectionQueue;
        private Queue<Vector3Int> m_ImportantDirtySectionQueue;
        private List<Vector3Int> m_DirtySectionFallbacks;
        private SectionMeshWorkScheduler m_WorkScheduler;
        private Action<SectionMeshWorkScheduler.AsyncWork> m_SchedulerClearWorkCallback;
        private IWorld m_World;
        private bool m_Initialized;

        public void Initialize(IWorld world)
        {
            m_SectionMeshPool = new Stack<Mesh>();
            m_SectionMeshMap = new Dictionary<Vector3Int, Mesh>();
            m_DirtySections = new HashSet<Vector3Int>();
            m_NotImportantDirtySectionQueue = new Queue<Vector3Int>();
            m_ImportantDirtySectionQueue = new Queue<Vector3Int>();
            m_DirtySectionFallbacks = new List<Vector3Int>();
            m_WorkScheduler = GetComponent<SectionMeshWorkScheduler>();
            m_World = world;
            m_Initialized = true;

            m_WorkScheduler.Initialize(world);
            m_World.ChunkManager.OnChunkUnloaded += FreeChunkMeshes;
        }

        private void OnDestroy()
        {
            m_Initialized = false;
        }

        private void Update()
        {
            if (!m_Initialized || (m_NotImportantDirtySectionQueue.Count == 0 && m_ImportantDirtySectionQueue.Count == 0))
            {
                return;
            }

            Profiler.BeginSample("SectionMeshManager.CollectMesh");

            m_WorkScheduler.ClearWorks(m_SchedulerClearWorkCallback ??= work =>
            {
                if (m_DirtySections.Add(work.Section))
                {
                    m_NotImportantDirtySectionQueue.Enqueue(work.Section);
                }
            });

            Vector3 position = m_World.PlayerTransform.position;
            int limit = m_BuildNotImportantMeshPreFrame;

            while (m_NotImportantDirtySectionQueue.Count > 0 && limit-- > 0)
            {
                Vector3Int section = m_NotImportantDirtySectionQueue.Dequeue();
                TryEnqueueMeshWork(section, position, false);
            }
            UpdateFallbacks(m_NotImportantDirtySectionQueue);

            while (m_ImportantDirtySectionQueue.Count > 0)
            {
                Vector3Int section = m_ImportantDirtySectionQueue.Dequeue();
                TryEnqueueMeshWork(section, position, true);
            }
            UpdateFallbacks(m_ImportantDirtySectionQueue);

            Profiler.EndSample();
        }

        private void TryEnqueueMeshWork(Vector3Int section, Vector3 playerPos, bool important)
        {
            Profiler.BeginSample("SectionMeshManager.TryEnqueueMeshWork");

            ChunkPos chunk = ChunkPos.Get(section.x, section.z);

            if (m_World.ChunkManager.GetChunk3x3Accessor(chunk, true, out Chunk3x3Accessor accessor))
            {
                m_DirtySections.Remove(section);
                Mesh mesh = GetSectionMesh(section);

                if (important)
                {
                    m_WorkScheduler.ScheduleWork(mesh, section, accessor);
                }
                else
                {
                    Vector2 playerXZ = new Vector2(playerPos.x, playerPos.z);
                    Vector2 sectionXZ = new Vector2(section.x, section.z);
                    float sqrDistance = (playerXZ - sectionXZ).sqrMagnitude;
                    float deltaY = Mathf.Abs(playerPos.y - section.y);
                    m_WorkScheduler.ScheduleAsyncWork(mesh, section, sqrDistance * deltaY, accessor);
                }
            }
            else
            {
                m_DirtySectionFallbacks.Add(section);
            }

            Profiler.EndSample();
        }

        private void UpdateFallbacks(Queue<Vector3Int> queue)
        {
            Profiler.BeginSample("SectionMeshManager.UpdateFallbacks");

            for (int i = 0; i < m_DirtySectionFallbacks.Count; i++)
            {
                queue.Enqueue(m_DirtySectionFallbacks[i]);
            }
            m_DirtySectionFallbacks.Clear();

            Profiler.EndSample();
        }

        private void FreeChunkMeshes(ChunkPos pos)
        {
            for (int i = 0; i < SectionCountInChunk; i++)
            {
                FreeSectionMesh(pos.XYZ(GetSectionY(i)));
            }
        }

        protected Mesh AllocateSectionMesh()
        {
            if (m_SectionMeshPool.Count > 0)
            {
                return m_SectionMeshPool.Pop();
            }

            Mesh mesh = new Mesh();
            mesh.MarkDynamic();
            return mesh;
        }

        public void FreeSectionMesh(Vector3Int section)
        {
            if (m_SectionMeshMap.TryGetValue(section, out Mesh mesh))
            {
                m_SectionMeshMap.Remove(section);
                mesh.Clear(false);
                m_SectionMeshPool.Push(mesh);
            }
        }

        public void MarkSectionMeshDirty(Vector3Int section, bool forceLoadMesh, bool important)
        {
            if (!forceLoadMesh && !m_SectionMeshMap.ContainsKey(section))
            {
                return;
            }

            if (m_DirtySections.Add(section))
            {
                if (important)
                {
                    m_ImportantDirtySectionQueue.Enqueue(section);
                }
                else
                {
                    m_NotImportantDirtySectionQueue.Enqueue(section);
                }
            }
        }

        public Mesh GetSectionMesh(Vector3Int section)
        {
            if (!m_SectionMeshMap.TryGetValue(section, out Mesh mesh))
            {
                mesh = AllocateSectionMesh();
                m_SectionMeshMap.Add(section, mesh);
                MarkSectionMeshDirty(section, true, false);
            }
            return mesh;
        }
    }
}
