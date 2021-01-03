using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Profiling;

namespace Minecraft.Rendering
{
    [CreateAssetMenu(menuName = "Minecraft/Managers/Rendering Manager", fileName = "Rendering Manager")]
    public sealed class RenderingManager : ScriptableObject
    {
        [SerializeField] [Range(10, 100)] private int m_GeometryBufferSize = 32;
        [SerializeField] [Range(0, 1)] private float m_GeometryUpdatingInterval = 0.25f;
        [SerializeField] private WorldRenderer[] m_Renderers;

        //main thread
        [NonSerialized] private Camera m_MainCamera;
        [NonSerialized] private BlockTextureTable m_TextureTable;
        [NonSerialized] private WaitCallback m_UpdateGeometryMethod;
        [NonSerialized] private HashSet<ChunkMeshSlice> m_VisibleChunkMeshes;
        [NonSerialized] private HashSet<AbstractMesh> m_EntityMeshes;

        // ...
        [NonSerialized] private volatile float m_UnityTime;
        [NonSerialized] private volatile float m_LastTimeUpdatingGeometry;
        [NonSerialized] private volatile bool m_IsUpdatingGeometry;
        [NonSerialized] private ConcurrentStack<ChunkMeshSlice> m_GeometryToUpdate;
        [NonSerialized] private ConcurrentStack<AbstractMesh> m_MeshToApply;

        // worker thread
        [NonSerialized] private ChunkMeshSlice[] m_GeometryBuffer;
        [NonSerialized] private HashSet<ChunkMeshSlice> m_GeometryUpdatingQueue;


        public void Initialize(IEnumerable<Block> blocks)
        {
            m_MainCamera = Camera.main;
            m_TextureTable = new BlockTextureTable(blocks);
            m_UpdateGeometryMethod = UpdateGeometry;
            m_VisibleChunkMeshes = new HashSet<ChunkMeshSlice>(EqualityComparer<ChunkMeshSlice>.Default);
            m_EntityMeshes = new HashSet<AbstractMesh>(EqualityComparer<AbstractMesh>.Default);

            m_UnityTime = 0f;
            m_LastTimeUpdatingGeometry = 0f;
            m_IsUpdatingGeometry = false;
            m_GeometryToUpdate = new ConcurrentStack<ChunkMeshSlice>();
            m_MeshToApply = new ConcurrentStack<AbstractMesh>();

            m_GeometryBuffer = new ChunkMeshSlice[m_GeometryBufferSize];
            m_GeometryUpdatingQueue = new HashSet<ChunkMeshSlice>();

            for (int i = 0; i < m_Renderers.Length; i++)
            {
                m_Renderers[i].Initialize(m_TextureTable);
            }
        }

        public void UpdateChunkGeometry(ChunkMeshSlice mesh)
        {
            m_GeometryToUpdate.Push(mesh);
        }

        public void SetVisibleChunks(IEnumerator<Chunk> iterator)
        {
            m_VisibleChunkMeshes.Clear();

            while (iterator.MoveNext())
            {
                Chunk chunk = iterator.Current;
                IEnumerator<ChunkMeshSlice> it = chunk.GetMeshEnumerator();

                while (it.MoveNext())
                {
                    m_VisibleChunkMeshes.Add(it.Current);
                }

                it.Dispose();
            }

            iterator.Dispose();
        }

        public void Update()
        {
            float time = Time.time;
            Interlocked.Exchange(ref m_UnityTime, time);

            if (!m_IsUpdatingGeometry && ((time - m_LastTimeUpdatingGeometry) >= m_GeometryUpdatingInterval) && !m_GeometryToUpdate.IsEmpty)
            {
                ThreadPool.QueueUserWorkItem(m_UpdateGeometryMethod);
            }

            RenderMeshGroup(m_VisibleChunkMeshes);
            RenderMeshGroup(m_EntityMeshes);

            while (m_MeshToApply.TryPop(out AbstractMesh mesh))
            {
                mesh.Apply();
            }
        }

        private void RenderMeshGroup(IEnumerable<AbstractMesh> meshes)
        {
            foreach (AbstractMesh mesh in meshes)
            {
                for (int i = 0; i < m_Renderers.Length; i++)
                {
                    if (m_Renderers[i].Render(mesh, m_MainCamera))
                    {
                        break;
                    }
                }
            }
        }

        private void UpdateGeometry(object state)
        {
            m_IsUpdatingGeometry = true;
            Profiler.BeginSample("RenderingManager.UpdateGeometry");
            m_GeometryUpdatingQueue.Clear();

            try
            {
                int count = m_GeometryToUpdate.TryPopRange(m_GeometryBuffer);

                while (count-- > 0)
                {
                    m_GeometryUpdatingQueue.Add(m_GeometryBuffer[count]);
                }

                foreach (ChunkMeshSlice mesh in m_GeometryUpdatingQueue)
                {
                    if (mesh.UpdateGeometry(m_TextureTable))
                    {
                        m_MeshToApply.Push(mesh);
                    }

                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                Interlocked.Exchange(ref m_LastTimeUpdatingGeometry, m_UnityTime);
                Profiler.EndSample();
                m_IsUpdatingGeometry = false;
            }
        }
    }
}