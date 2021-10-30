using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Minecraft.Collections;
using Minecraft.Configurations;
using Minecraft.Lua;
using Minecraft.Utils;
using UnityEngine;
using static Minecraft.Rendering.LightingUtility;
using static Minecraft.WorldConsts;

namespace Minecraft
{
    [DisallowMultipleComponent]
    public class ChunkBuilder : WorkScheduler<ChunkBuilder.Empty, ChunkPos>, ILuaCallCSharp
    {
        public struct Empty { }


        private ConcurrentBag<Chunk> m_ChunkPool;
        private ConcurrentQueue<Chunk> m_BuildedChunks;
        private IWorld m_World;
        private volatile float m_PlayerX;
        private volatile float m_PlayerZ;

        protected override void OnInitialize()
        {
            m_ChunkPool = new ConcurrentBag<Chunk>();
            m_BuildedChunks = new ConcurrentQueue<Chunk>();
            m_World = null;
            m_PlayerX = 0;
            m_PlayerZ = 0;
        }

        public void Initialize(IWorld world)
        {
            m_World = world;
            StartWorkerThread();
        }

        public void RecycleChunk(Chunk chunk)
        {
            chunk.Dispose();
            m_ChunkPool.Add(chunk);
        }

        public void BuildChunk(ChunkPos transform)
        {
            AddWork(in transform);
        }

        public void GetBuildedChunks(Action<Chunk> callback)
        {
            if (callback == null)
            {
                return;
            }

            while (m_BuildedChunks.TryDequeue(out Chunk chunk))
            {
                chunk.AllowAccessing();
                callback(chunk);
            }
        }

        private void Update()
        {
            if (m_World != null)
            {
                Vector3 playerPos = m_World.PlayerTransform.position;
                Interlocked.Exchange(ref m_PlayerX, playerPos.x);
                Interlocked.Exchange(ref m_PlayerZ, playerPos.z);
            }
        }


        protected override void DoMainThreadWorks(List<Empty> works) { }

        protected override void DoAsyncWork(in ChunkPos pos)
        {
            if (!m_ChunkPool.TryTake(out Chunk chunk))
            {
                chunk = new Chunk();
            }

            chunk.Initialize(pos.X, pos.Z, m_World);
            chunk.GetRawDataNoCheck(out _, out IWorld world, out BlockData[,,] blocks, out _, out NibbleArray skyLights, out byte[,] heightMap);

            world.WorldGenPipeline.GenerateChunk(chunk);
            InitLightsAndColumns(pos, world, blocks, skyLights, heightMap);
            m_BuildedChunks.Enqueue(chunk);
        }

        protected override int CompareAsyncWork(ChunkPos x, ChunkPos y)
        {
            Vector2 playerXZ = new Vector2(m_PlayerX, m_PlayerZ);
            float sqrDistX = (playerXZ - x.XZ).sqrMagnitude;
            float sqrDistY = (playerXZ - y.XZ).sqrMagnitude;
            return (int)(sqrDistX - sqrDistY);
        }


        protected static void InitLightsAndColumns(ChunkPos pos, IWorld world, BlockData[,,] blocks, NibbleArray skyLights, byte[,] heightMap)
        {
            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int z = 0; z < ChunkWidth; z++)
                {
                    int topVisibleBlockY = -1;
                    int skyLight = SkyLight;

                    for (int y = ChunkHeight - 1; y >= 0; y--)
                    {
                        BlockData block = blocks[x, y, z];

                        if (topVisibleBlockY == -1 && (!block.HasFlag(BlockFlags.AlwaysInvisible) || y == 0))
                        {
                            topVisibleBlockY = y;
                        }

                        skyLights[Chunk.GetNibbleArrayIndex(x, y, z)] = (byte)skyLight;
                        skyLight = GetBlockedLight(skyLight, block);

                        // Chunk 加载完后，在主线程调用 Chunk.PostLightAllBlocks
                        // if (block.LightValue > 0)
                        // {
                        //     int worldX = x;
                        //     int worldY = y;
                        //     int worldZ = z;

                        //     WorldUtility.AccessorSpaceToWorldSpacePosition(pos.XOZ, ref worldX, ref worldY, ref worldZ);
                        //     world.LightBlock(worldX, worldY, worldZ);
                        // }
                    }

                    heightMap[x, z] = (byte)topVisibleBlockY;
                }
            }
        }
    }
}
