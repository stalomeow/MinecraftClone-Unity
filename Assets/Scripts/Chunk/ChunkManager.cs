using Minecraft.BlocksData;
using Minecraft.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;
using static Minecraft.WorldConsts;
using Random = System.Random;

namespace Minecraft
{
    public sealed class ChunkManager : IDisposable
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


        private readonly ChunkPositionComparer m_ChunkPositionComparer;

        private readonly ConcurrentDictionary<Vector2Int, Chunk> m_Chunks; // 所有的chunk

        private readonly ConcurrentQueue<Vector2Int> m_ChunksToLoad; // 加入到该队列后，chunks会在BuildChunks线程中加载，加载后放入到chunks字典中
        private readonly ConcurrentQueue<Vector2Int> m_ChunksToUnload; // 销毁chunks的队列
        private readonly ChunkLoader m_ChunkLoader;
        private readonly Thread m_BuildAndUnloadChunksThread; // 构建/销毁 chunks

        private readonly HashSet<Vector2Int> m_ChunksToRender; // 主线程遍历，渲染，update线程添加
        private readonly object m_ChunksToRenderHashSetLock;

        private readonly Thread m_UpdateChunksThread; // 计算需要加载/更新/渲染的chunks队列

        private readonly Queue<Vector3Int> m_BlocksToTickQueue; // 主线程
        private readonly float m_TickBlockInterval;
        private float m_TickBlockDeltaTime;

        private readonly Stack<Vector3Int> m_BlocksToLightQueue; // 主线程

        private readonly Random m_TickChunkRandom;
        private readonly float m_TickChunkInterval;
        private float m_TickChunkDeltaTime;

        private volatile float m_PlayerPositionX;
        private volatile float m_PlayerPositionZ;
        private volatile float m_PlayerForwardX;
        private volatile float m_PlayerForwardZ;
        private volatile bool m_ChunksUpdatingRequired; // 是否需要更新玩家周围的chunk

        private readonly int m_RenderChunkRadius; // 渲染距离（chunk为单位）
        private readonly int m_SqrRenderRadius; // 渲染距离的平方
        private readonly float m_HorizontalFOVInDEG; // 水平视角大小（角度制）
        private readonly int m_MaxChunkCountInMemory;
        private readonly Material m_SharedSolidMaterial;
        private readonly Material m_SharedLiquidMaterial;
        private readonly Camera m_MainCamera;
        private readonly Transform m_PlayerTransform;

        private bool m_IsStartUp;
        private bool m_Disposed;

        public event Action OnChunksReadyWhenStartingUp;

        public ExposedMaterialProperties MaterialProperties { get; }

        public int LoadedChunkCount => m_Chunks.Count;

        public int RenderChunkCount
        {
            get
            {
                lock (m_ChunksToRenderHashSetLock)
                {
                    return m_ChunksToRender.Count;
                }
            }
        }


        public ChunkManager(WorldSettings settings, Camera mainCamera, Transform player, string chunkSavingDirectory, Material solidMaterial, Material liquidMaterial)
        {
            m_ChunkPositionComparer = new ChunkPositionComparer();

            m_Chunks = new ConcurrentDictionary<Vector2Int, Chunk>(m_ChunkPositionComparer);

            m_ChunksToLoad = new ConcurrentQueue<Vector2Int>();
            m_ChunksToUnload = new ConcurrentQueue<Vector2Int>();
            m_ChunkLoader = new ChunkLoader(settings.Seed, chunkSavingDirectory, settings.Type);
            m_BuildAndUnloadChunksThread = new Thread(BuildAndUnloadChunksThreadMethod) { IsBackground = true };

            m_ChunksToRender = new HashSet<Vector2Int>(m_ChunkPositionComparer);
            m_ChunksToRenderHashSetLock = new object();

            m_UpdateChunksThread = new Thread(UpdateChunksThreadMethod) { IsBackground = true };

            m_BlocksToTickQueue = new Queue<Vector3Int>();
            m_TickBlockInterval = 0.5f;
            m_TickBlockDeltaTime = 0;

            m_BlocksToLightQueue = new Stack<Vector3Int>();

            m_TickChunkRandom = new Random(settings.Seed);
            m_TickChunkInterval = 2;
            m_TickChunkDeltaTime = 0;

            m_PlayerPositionX = 0;
            m_PlayerPositionZ = 0;
            m_PlayerForwardX = 0;
            m_PlayerForwardZ = 0;
            m_ChunksUpdatingRequired = false;

            m_RenderChunkRadius = GlobalSettings.Instance.RenderChunkRadius;
            m_SqrRenderRadius = m_RenderChunkRadius * m_RenderChunkRadius * ChunkWidth * ChunkWidth;
            m_HorizontalFOVInDEG = GlobalSettings.Instance.HorizontalFOVInDEG;
            m_MaxChunkCountInMemory = GlobalSettings.Instance.MaxChunkCountInMemory;
            m_SharedSolidMaterial = solidMaterial;
            m_SharedLiquidMaterial = liquidMaterial;
            m_MainCamera = mainCamera;
            m_MainCamera.farClipPlane = GlobalSettings.Instance.RenderRadius;
            m_PlayerTransform = player;

            MaterialProperties = new ExposedMaterialProperties(new MaterialPropertyBlock(), new MaterialPropertyBlock());

            InitMaterialProperties();

            m_IsStartUp = true;
            m_Disposed = false;

#if UNITY_EDITOR
            m_BuildAndUnloadChunksThread.Name = "Build And Unload Chunks Thread";
            m_UpdateChunksThread.Name = "Update Chunks Thread";
#endif
        }

        public void SyncFixedUpdateOnMainThread()
        {
            if (m_IsStartUp)
                return;

            LightBlocksOnMainThread();
        }

        public void SyncUpdateOnMainThread()
        {
            TickBlocksOnMainThread();
            RandomTickChunkOnMainThread();
        }

        public void SyncLateUpdateOnMainThread()
        {
            RenderAndUpdateChunksOnMainThread();
            UpdatePlayerPositionOnMainThread();
        }

        public void StartChunksUpdatingThread()
        {
            m_UpdateChunksThread.Start();
        }

        public void StartChunksBuildingThread()
        {
            m_BuildAndUnloadChunksThread.Start();
        }

        public void Dispose()
        {
            if (!m_Disposed)
            {
                m_Disposed = true;
                //m_UpdateChunksThread.Abort();
                //m_BuildAndUnloadChunksThread.Abort();

                // 保存chunks
                Parallel.ForEach(m_Chunks.Values, chunk => m_ChunkLoader.SaveChunk(chunk));
            }
        }


        private void InitMaterialProperties()
        {
            ExposedMaterialProperties properties = MaterialProperties;

            properties.SetRenderRadius(GlobalSettings.Instance.RenderRadius);
            properties.SetAmbientColor(GlobalSettings.Instance.DefaultAmbientColor);
        }

        private void TickBlocksOnMainThread()
        {
            m_TickBlockDeltaTime += Time.fixedDeltaTime;

            if (m_TickBlockDeltaTime < m_TickBlockInterval)
                return;

            m_TickBlockDeltaTime = 0;

            int count = Mathf.Clamp(m_BlocksToTickQueue.Count, 0, MaxTickBlockCountPerFrame); // 防止卡死
            WorldManager world = WorldManager.Active;

            while (count-- > 0)
            {
                Vector3Int blockCenterPos = m_BlocksToTickQueue.Dequeue();

                int x = blockCenterPos.x;
                int y = blockCenterPos.y;
                int z = blockCenterPos.z;

                world.GetBlock(x, y, z).OnTick(x, y, z);
                world.GetBlock(x, y + 1, z).OnTick(x, y + 1, z);
                world.GetBlock(x, y - 1, z).OnTick(x, y - 1, z);
                world.GetBlock(x + 1, y, z).OnTick(x + 1, y, z);
                world.GetBlock(x - 1, y, z).OnTick(x - 1, y, z);
                world.GetBlock(x, y, z + 1).OnTick(x, y, z + 1);
                world.GetBlock(x, y, z - 1).OnTick(x, y, z - 1);
            }
        }

        private void LightBlocksOnMainThread()
        {
            // https://github.com/ddevault/TrueCraft/wiki/Lighting
            // https://github.com/ddevault/TrueCraft/blob/master/TrueCraft.Core/Lighting/WorldLighting.cs

            int count = m_BlocksToLightQueue.Count;

            if (count > MaxLightBlockCountPerFrame)
            {
                count = MaxLightBlockCountPerFrame; // 防止卡死
            }

            WorldManager world = WorldManager.Active;
            
            while (count-- > 0)
            {
                Vector3Int blockPos = m_BlocksToLightQueue.Pop();

                Vector2Int chunkPos = Chunk.NormalizeToChunkPosition(blockPos.x, blockPos.z);

                if (!m_Chunks.ContainsKey(chunkPos))
                    continue;

                int x = blockPos.x;
                int y = blockPos.y;
                int z = blockPos.z;

                Block block = world.GetBlock(x, y, z);
                int opacity = block.LightOpacity < MinBlockLightOpacity ? MinBlockLightOpacity : block.LightOpacity;

                int current = world.GetBlockLight(x, y, z);
                int finalLight = 0;

                if (opacity < MaxLight || block.LightValue > 0) // 不然就是0
                {
                    int max = world.GetBlockLight(x + 1, y, z);
                    int temp;

                    if ((temp = world.GetBlockLight(x - 1, y, z)) > max)
                        max = temp;

                    if ((temp = world.GetBlockLight(x, y + 1, z)) > max)
                        max = temp;

                    if ((temp = world.GetBlockLight(x, y - 1, z)) > max)
                        max = temp;

                    if ((temp = world.GetBlockLight(x, y, z + 1)) > max)
                        max = temp;

                    if ((temp = world.GetBlockLight(x, y, z - 1)) > max)
                        max = temp;

                    finalLight = max - opacity;

                    if (block.LightValue > finalLight)
                    {
                        finalLight = block.LightValue; // 假设这个值一定是合法的（不过确实应该是合法的）
                    }
                    else if (finalLight < 0)
                    {
                        finalLight = 0;
                    }
                    //else if (finalLight > MaxLight)
                    //{
                    //    finalLight = MaxLight;
                    //}
                }

                if (current != finalLight)
                {
                    world.SetBlockLight(x, y, z, (byte)finalLight);

                    m_BlocksToLightQueue.Push(new Vector3Int(x - 1, y, z));
                    m_BlocksToLightQueue.Push(new Vector3Int(x, y - 1, z));
                    m_BlocksToLightQueue.Push(new Vector3Int(x, y, z - 1));
                    m_BlocksToLightQueue.Push(new Vector3Int(x + 1, y, z));
                    m_BlocksToLightQueue.Push(new Vector3Int(x, y + 1, z));
                    m_BlocksToLightQueue.Push(new Vector3Int(x, y, z + 1));
                }
            }
        }

        //render mesh, update mesh
        private void RenderAndUpdateChunksOnMainThread()
        {
            MaterialPropertyBlock solidProperty = MaterialProperties.GetSolidPropertyBlock();
            MaterialPropertyBlock liquidProperty = MaterialProperties.GetLiquidPropertyBlock();

            bool flag = true;

            lock (m_ChunksToRenderHashSetLock)
            {
                HashSet<Vector2Int>.Enumerator iterator = m_ChunksToRender.GetEnumerator();

                while (iterator.MoveNext())
                {
                    Vector2Int chunkPos = iterator.Current;

                    if (!m_Chunks.TryGetValue(chunkPos, out Chunk chunk)) // 未加载完成
                        continue;

                    if (chunk.ShouldUpdateMesh)
                    {
                        flag = false;
                        chunk.StartBuildMesh(); // async
                    }

                    chunk.RenderChunk(m_SharedSolidMaterial, m_SharedLiquidMaterial, m_MainCamera, solidProperty, liquidProperty);
                }

                iterator.Dispose();
            }

            if (m_IsStartUp && flag && m_Chunks.Count > 0 && m_ChunksToLoad.Count == 0)
            {
                m_IsStartUp = false;
                OnChunksReadyWhenStartingUp?.Invoke();
            }
        }

        private void UpdatePlayerPositionOnMainThread()
        {
            if (m_ChunksUpdatingRequired) // 上一次update未完成
                return;

            Vector3 pos = m_PlayerTransform.localPosition;
            Vector3 forward = m_PlayerTransform.forward;

            Vector2Int lastChunkPos = Chunk.NormalizeToChunkPosition(m_PlayerPositionX, m_PlayerPositionZ);
            Vector2Int chunkPos = Chunk.NormalizeToChunkPosition(pos.x, pos.z);

            if (lastChunkPos != chunkPos || forward.x != m_PlayerForwardX || forward.z != m_PlayerForwardZ)
            {
                m_PlayerPositionX = pos.x;
                m_PlayerPositionZ = pos.z;

                m_PlayerForwardX = forward.x;
                m_PlayerForwardZ = forward.z;

                m_ChunksUpdatingRequired = true; // 最后赋值
            }
        }

        private void RandomTickChunkOnMainThread()
        {
            m_TickChunkDeltaTime += Time.deltaTime;

            if (m_TickChunkDeltaTime < m_TickChunkInterval)
                return;

            m_TickChunkDeltaTime = 0;

            Vector3 pos = m_PlayerTransform.localPosition;
            Vector2Int chunkPos = Chunk.NormalizeToChunkPosition(pos.x, pos.z);

            // 只tick玩家周围的9个chunk中的一个

            int i = m_TickChunkRandom.Next(-1, 2);
            int j = m_TickChunkRandom.Next(-1, 2);

            chunkPos.x += i * ChunkWidth;
            chunkPos.y += j * ChunkWidth;

            if (m_Chunks.TryGetValue(chunkPos, out Chunk chunk))
            {
                chunk.RandomTick(m_TickChunkRandom);
            }
        }

        private void UpdateChunksThreadMethod()
        {
            ChunkPriorityComparer priorityComparer = new ChunkPriorityComparer();

            PriorityQueue<ChunkNeedsLoading> updateChunkQueue = new PriorityQueue<ChunkNeedsLoading>(priorityComparer);
            HashSet<Vector2Int> inRangeChunks = new HashSet<Vector2Int>(m_ChunkPositionComparer); // 求交集优化

            while (!m_Disposed)
            {
                if (!m_ChunksUpdatingRequired) // 不需要更新
                    continue;

                Profiler.BeginSample("Update Chunks");

                updateChunkQueue.Clear();
                inRangeChunks.Clear();

                float playerX = m_PlayerPositionX;
                float playerZ = m_PlayerPositionZ;
                float forwardX = m_PlayerForwardX;
                float forwardZ = m_PlayerForwardZ;

                Vector2Int playerChunkPos = Chunk.NormalizeToChunkPosition(playerX, playerZ);
                Vector2 player = new Vector2(playerX, playerZ);
                Vector2 forward = new Vector2(forwardX, forwardZ).normalized;

                for (int x = -m_RenderChunkRadius; x <= m_RenderChunkRadius; x++)
                {
                    for (int z = -m_RenderChunkRadius; z <= m_RenderChunkRadius; z++)
                    {
                        Vector2Int chunkPos = new Vector2Int(playerChunkPos.x + x * ChunkWidth, playerChunkPos.y + z * ChunkWidth);

                        inRangeChunks.Add(chunkPos);

                        lock (m_ChunksToRenderHashSetLock)
                        {
                            if (m_ChunksToRender.Contains(chunkPos)) // 已经在渲染队列
                                continue;
                        }

                        Vector2 chunkCenterPos = chunkPos + new Vector2(ChunkWidth >> 1, ChunkWidth  >> 1);
                        Vector2 toChunkCenter = chunkCenterPos - player;

                        float sqrDis = toChunkCenter.sqrMagnitude;
                        float angle = Vector2.Angle(toChunkCenter, forward);

                        bool close = sqrDis < SqrMinChunkDistance;
                        bool inAngle = angle < m_HorizontalFOVInDEG;

                        if (m_IsStartUp || inAngle || close)
                        {
                            updateChunkQueue.Enqueue(new ChunkNeedsLoading
                            {
                                ChunkPosition = chunkPos,
                                PriorityFactor = (sqrDis / m_SqrRenderRadius) * 4.25f + (angle / m_HorizontalFOVInDEG) * 5.75f
                            });
                        }
                    }
                }
                
                lock (m_ChunksToRenderHashSetLock)
                {
                    m_ChunksToRender.IntersectWith(inRangeChunks);
                }

                while (updateChunkQueue.Count > 0)
                {
                    Vector2Int chunk = updateChunkQueue.Dequeue().ChunkPosition;

                    lock (m_ChunksToRenderHashSetLock)
                    {
                        m_ChunksToRender.Add(chunk);
                    }

                    if (!m_Chunks.ContainsKey(chunk))
                    {
                        m_ChunksToLoad.Enqueue(chunk);
                    }
                }

                Profiler.EndSample();

                if (m_Chunks.Count > m_MaxChunkCountInMemory)
                {
                    Parallel.ForEach(m_Chunks, pair =>
                    {
                        Vector2 playerPos = new Vector2(m_PlayerPositionX, m_PlayerPositionZ);
                        pair.Value.MarkAsStartUp();

                        if ((pair.Key - playerPos).sqrMagnitude > 4 * m_SqrRenderRadius)
                        {
                            m_ChunksToUnload.Enqueue(pair.Key);
                        }
                    });
                }

                //END
                m_ChunksUpdatingRequired = false;
            }
        }

        private void BuildAndUnloadChunksThreadMethod()
        {
            ObjectPool<Chunk> chunkPool = new ObjectPool<Chunk>(m_RenderChunkRadius * 2, m_RenderChunkRadius * m_RenderChunkRadius * 4);

            while (!m_Disposed)
            {
                // unload chunks

                if (m_ChunksToUnload.TryDequeue(out Vector2Int chunkPos) && m_Chunks.TryRemove(chunkPos, out Chunk chunk))
                {
                    if (chunk.IsModified)
                    {
                        m_ChunkLoader.SaveChunk(chunk, false);
                    }

                    chunkPool.Free(chunk);
                }

                //build chunks

                if (!m_ChunksToLoad.TryDequeue(out chunkPos))
                {
                    continue;
                }

                if (m_Chunks.ContainsKey(chunkPos))
                {
                    continue;
                }

                chunk = chunkPool.Allocate();
                m_ChunkLoader.LoadChunk(chunk, chunkPos.x, chunkPos.y);
                while (!m_Chunks.TryAdd(chunkPos, chunk)) ;
            }
        }

        public Task WaitForAllNeighborChunksLoaded(Chunk chunk)
        {
            return Task.Factory.StartNew(c =>
            {
                Chunk ch = c as Chunk;

                int x = ch.PositionX;
                int z = ch.PositionZ;

                WaitForChunkLoaded(new Vector2Int(x + ChunkWidth, z));
                WaitForChunkLoaded(new Vector2Int(x - ChunkWidth, z));
                WaitForChunkLoaded(new Vector2Int(x, z + ChunkWidth));
                WaitForChunkLoaded(new Vector2Int(x, z - ChunkWidth));
            }, chunk);
        }

        private void WaitForChunkLoaded(Vector2Int pos)
        {
            if (!m_Chunks.ContainsKey(pos))
            {
                m_ChunksToLoad.Enqueue(pos);

                while (!m_Chunks.ContainsKey(pos))
                    Thread.Sleep(10);
            }
        }


        public void TickBlock(int x, int y, int z)
        {
            m_BlocksToTickQueue.Enqueue(new Vector3Int(x, y, z));
        }

        public void LightBlock(int x, int y, int z)
        {
            m_BlocksToLightQueue.Push(new Vector3Int(x, y, z));
        }

        public Chunk GetChunkByNormalizedPosition(int x, int z)
        {
            Vector2Int key = new Vector2Int(x, z);
            m_Chunks.TryGetValue(key, out Chunk chunk);
            return chunk;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Chunk GetChunk(float x, float z)
        {
            Vector2Int key = Chunk.NormalizeToChunkPosition(x, z);
            m_Chunks.TryGetValue(key, out Chunk chunk);
            return chunk;
        }
    }
}
