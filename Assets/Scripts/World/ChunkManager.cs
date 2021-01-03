using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System;
using System.Collections.Concurrent;
using System.Threading;
using static Minecraft.WorldConsts;
using Minecraft.Collections;

namespace Minecraft
{
    [CreateAssetMenu(menuName = "Minecraft/Managers/Chunk Manager", fileName = "Chunk Manager")]
    public class ChunkManager : ScriptableObject
    {
        private struct ChunkNeedsLoading
        {
            public Vector2Int ChunkPosition;
            public float PriorityFactor;
        }

        private sealed class ChunkPriorityComparer : IComparer<ChunkNeedsLoading>
        {
            public int Compare(ChunkNeedsLoading x, ChunkNeedsLoading y)
            {
                return (int)(x.PriorityFactor - y.PriorityFactor);
            }
        }

        private sealed class ChunkPositionComparer : IEqualityComparer<Vector2Int>
        {
            public bool Equals(Vector2Int x, Vector2Int y)
            {
                return x.x == y.x && x.y == y.y;
            }

            public int GetHashCode(Vector2Int obj)
            {
                return obj.GetHashCode();
            }
        }


        private ConcurrentDictionary<Vector2Int, Chunk> m_Chunks;
        private World m_World;


        private readonly ChunkPositionComparer m_ChunkPositionComparer;
        private readonly HashSet<Vector2Int> m_ChunksToRender; // 主线程遍历，渲染，update线程添加

        private readonly Thread m_UpdateChunksThread; // 计算需要加载/更新/渲染的chunks队列

        private volatile float m_PlayerPositionX;
        private volatile float m_PlayerPositionY;
        private volatile float m_PlayerPositionZ;
        private volatile float m_PlayerForwardX;
        private volatile float m_PlayerForwardZ;
        private readonly ManualResetEventSlim m_UpdateChunksEvent; // 是否需要更新玩家周围的chunk

        private readonly Camera m_MainCamera;
        private readonly Transform m_PlayerTransform;

        public IEnumerable<Chunk> Chunks => m_Chunks.Values;


        public void Initialize(World world)
        {
            m_Chunks = new ConcurrentDictionary<Vector2Int, Chunk>(m_ChunkPositionComparer);
            m_World = world;


            m_PlayerPositionX = 0;
            m_PlayerPositionY = 0;
            m_PlayerPositionZ = 0;
            m_PlayerForwardX = 0;
            m_PlayerForwardZ = 0;
        }

        public Chunk GetChunk(int x, int z)
        {
            if (m_Chunks.TryGetValue(new Vector2Int(x, z), out Chunk chunk))
            {
                return chunk;
            }

            return null;
        }

        public Chunk LoadChunk(int x, int z)
        {
            Chunk chunk = new Chunk();
            m_Chunks.TryAdd(new Vector2Int(x, z), chunk);
            return chunk;
        }

        public void Update()
        {
            //if (m_UpdateChunksEvent.IsSet)
            //    return;

            //Vector3 pos = m_PlayerTransform.localPosition;
            //Vector3 forward = m_PlayerTransform.forward;

            //Vector2Int lastChunkPos = Chunk.GetChunkPosition(m_PlayerPositionX, m_PlayerPositionZ);
            //Vector2Int chunkPos = Chunk.GetChunkPosition(pos.x, pos.z);

            //Interlocked.Exchange(ref m_PlayerPositionY, pos.y);

            //if (lastChunkPos != chunkPos || forward.x != m_PlayerForwardX || forward.z != m_PlayerForwardZ)
            //{
            //    Interlocked.Exchange(ref m_PlayerPositionX, pos.x);
            //    Interlocked.Exchange(ref m_PlayerPositionZ, pos.z);

            //    Interlocked.Exchange(ref m_PlayerForwardX, forward.x);
            //    Interlocked.Exchange(ref m_PlayerForwardZ, forward.z);

            //    m_UpdateChunksEvent.Set();
            //}
        }

        public void UpdateChunksThreadMethod()
        {
            ChunkPriorityComparer priorityComparer = new ChunkPriorityComparer();

            PriorityQueue<ChunkNeedsLoading> updateChunkQueue = new PriorityQueue<ChunkNeedsLoading>(priorityComparer);
            HashSet<Vector2Int> inRangeChunks = new HashSet<Vector2Int>(m_ChunkPositionComparer); // 求交集优化

            while (true)
            {
                m_UpdateChunksEvent.Wait();

                Profiler.BeginSample("Update Chunks");

                updateChunkQueue.Clear();
                inRangeChunks.Clear();

                float playerX = m_PlayerPositionX;
                float playerZ = m_PlayerPositionZ;
                float forwardX = m_PlayerForwardX;
                float forwardZ = m_PlayerForwardZ;

                Vector2Int playerChunkPos = Chunk.GetChunkPosition(playerX, playerZ);
                Vector2 player = new Vector2(playerX, playerZ);
                Vector2 forward = new Vector2(forwardX, forwardZ).normalized;

                for (int x = -16 * 2; x <= 16 * 2; x++)
                {
                    for (int z = -16 * 2; z <= 16 * 2; z++)
                    {
                        Vector2Int chunkPos = new Vector2Int(playerChunkPos.x + x * ChunkWidth, playerChunkPos.y + z * ChunkWidth);

                        inRangeChunks.Add(chunkPos);

                        lock (m_ChunksToRender)
                        {
                            if (m_ChunksToRender.Contains(chunkPos)) // 已经在渲染队列
                                continue;
                        }

                        Vector2 chunkCenterPos = chunkPos + new Vector2(ChunkWidth >> 1, ChunkWidth >> 1);
                        Vector2 toChunkCenter = chunkCenterPos - player;

                        float sqrDis = toChunkCenter.sqrMagnitude;
                        float angle = Vector2.Angle(toChunkCenter, forward);

                        bool close = sqrDis < SqrMinChunkDistance;
                        bool inAngle = angle < 90;

                        if (inAngle || close)
                        {
                            updateChunkQueue.Enqueue(new ChunkNeedsLoading
                            {
                                ChunkPosition = chunkPos,
                                PriorityFactor = sqrDis / (16 * 16 * 2 * 2) + angle / 90
                            });
                        }
                    }
                }

                lock (m_ChunksToRender)
                {
                    m_ChunksToRender.IntersectWith(inRangeChunks);
                }

                while (updateChunkQueue.Count > 0)
                {
                    Vector2Int chunk = updateChunkQueue.Dequeue().ChunkPosition;

                    lock (m_ChunksToRender)
                    {
                        m_ChunksToRender.Add(chunk);
                    }

                    if (!m_Chunks.ContainsKey(chunk))
                    {
                        LoadChunk(chunk.x, chunk.y);
                    }
                }

                Profiler.EndSample();

                //END
                m_UpdateChunksEvent.Reset();
            }
        }
    }
}
