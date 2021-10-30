using System;
using Minecraft.Collections;
using Minecraft.Configurations;
using UnityEngine;
using static Minecraft.Rendering.LightingUtility;
using static Minecraft.WorldConsts;

namespace Minecraft
{
    // 这里全部不加锁，因为一般只有距离玩家很远的区块才会在其他线程加载。
    public class Chunk : IWorldRWAccessor, IDisposable
    {
        public bool m_Accessible;
        private ChunkPos m_Position;
        private IWorld m_World;
        private BlockData[,,] m_Blocks;
        private Quaternion[,,] m_Rotations;
        private NibbleArray m_SkyLights;
        private NibbleArray m_AmbientLights;
        private byte[,] m_HeightMap;


        public Chunk()
        {
            m_Accessible = false;
            m_Position = default;
            m_World = default;
            m_Blocks = new BlockData[ChunkWidth, ChunkHeight, ChunkWidth];
            m_Rotations = new Quaternion[ChunkWidth, ChunkHeight, ChunkWidth];
            m_SkyLights = new NibbleArray(ChunkWidth * ChunkHeight * ChunkWidth);
            m_AmbientLights = new NibbleArray(ChunkWidth * ChunkHeight * ChunkWidth);
            m_HeightMap = new byte[ChunkWidth, ChunkWidth];
        }

        public override string ToString()
        {
            return m_Position.ToString();
        }


        public bool Accessible => m_Accessible;

        public Vector3Int WorldSpaceOrigin => Accessible ? Position.XOZ : throw new InvalidOperationException("Chunk is not accessible.");

        public IWorld World => Accessible ? m_World : throw new InvalidOperationException("Chunk is not accessible.");

        public ChunkPos Position => Accessible ? m_Position : throw new InvalidOperationException("Chunk is not accessible.");


        public void Initialize(int x, int z, IWorld world)
        {
            m_Position = ChunkPos.Get(x, z);
            m_World = world;
        }

        public void AllowAccessing()
        {
            m_Accessible = true;
        }

        public void Dispose()
        {
            m_Accessible = false;
            m_Position = default;
            m_World = default;
            m_SkyLights.Clear();
            m_AmbientLights.Clear();
            Array.Clear(m_Blocks, 0, m_Blocks.Length);
            Array.Clear(m_Rotations, 0, m_Rotations.Length);
            Array.Clear(m_HeightMap, 0, m_HeightMap.Length);
        }

        public void GetRawDataNoCheck(out ChunkPos pos, out IWorld world, out BlockData[,,] blocks, out Quaternion[,,] rotations, out NibbleArray skyLights, out byte[,] heightMap)
        {
            pos = m_Position;
            world = m_World;
            blocks = m_Blocks;
            rotations = m_Rotations;
            skyLights = m_SkyLights;
            heightMap = m_HeightMap;
        }


        private void SetTopVisibleBlockY(int x, int z, int value)
        {
            m_HeightMap[x, z] = (byte)value;
        }

        private void UpdateSkyLightData(int x, int z, ModificationSource source)
        {
            int skyLight = SkyLight;

            for (int y = ChunkHeight - 1; y >= 0; y--)
            {
                int index = GetNibbleArrayIndex(x, y, z);

                if (m_SkyLights[index] != skyLight)
                {
                    m_SkyLights[index] = (byte)skyLight;
                    World.MarkBlockMeshDirty(x, y, z, source);
                }

                skyLight = GetBlockedLight(skyLight, m_Blocks[x, y, z]);
            }
        }


        public bool SetBlock(int x, int y, int z, BlockData value, Quaternion rotation, ModificationSource source)
        {
            if (!Accessible)
            {
                throw new InvalidOperationException("Chunk is not accessible.");
            }

            if (y < 0 || y >= ChunkHeight)
            {
                return false;
            }

            ref BlockData blockData = ref m_Blocks[x, y, z];

            if (blockData != value)
            {
                BlockData previousBlock = blockData;
                blockData = value;
                m_Rotations[x, y, z] = rotation;

                int topVisibleBlockY = GetTopVisibleBlockY(x, z);
                bool visible = !value.HasFlag(BlockFlags.AlwaysInvisible);

                if (y > topVisibleBlockY && visible)
                {
                    SetTopVisibleBlockY(x, z, y);
                }
                else if (y == topVisibleBlockY && !visible)
                {
                    for (int i = y - 1; i >= 0; i--)
                    {
                        if (!GetBlock(x, i, z).HasFlag(BlockFlags.AlwaysInvisible) || i == 0)
                        {
                            SetTopVisibleBlockY(x, z, i);
                            break;
                        }
                    }
                }

                UpdateSkyLightData(x, z, source);

                this.AccessorSpaceToWorldSpacePosition(ref x, ref y, ref z);

                previousBlock.Destroy(World, x, y, z);
                value.Place(World, x, y, z);
                World.TickBlock(x, y, z);
                World.LightBlock(x, y, z, source);
                World.MarkBlockMeshDirty(x, y, z, source);
                return true;
            }

            return false;
        }

        public bool SetAmbientLightLevel(int x, int y, int z, int value, ModificationSource source)
        {
            if (!Accessible)
            {
                throw new InvalidOperationException("Chunk is not accessible.");
            }

            if (y < 0 || y >= ChunkHeight)
            {
                return false;
            }

            int index = GetNibbleArrayIndex(x, y, z);

            if (m_AmbientLights[index] != value)
            {
                m_AmbientLights[index] = (byte)value;

                this.AccessorSpaceToWorldSpacePosition(ref x, ref y, ref z);
                World.MarkBlockMeshDirty(x, y, z, source);
                return true;
            }

            return false;
        }

        public BlockData GetBlock(int x, int y, int z, BlockData defaultValue = null)
        {
            if (!Accessible)
            {
                throw new InvalidOperationException("Chunk is not accessible.");
            }

            if (y >= ChunkHeight || y < 0)
            {
                return defaultValue;
            }

            return m_Blocks[x, y, z];
        }

        public Quaternion GetBlockRotation(int x, int y, int z, Quaternion defaultValue = default)
        {
            if (!Accessible)
            {
                throw new InvalidOperationException("Chunk is not accessible.");
            }

            if (y >= ChunkHeight || y < 0)
            {
                return defaultValue;
            }

            return m_Rotations[x, y, z];
        }

        public int GetAmbientLight(int x, int y, int z, int defaultValue = 0)
        {
            if (!Accessible)
            {
                throw new InvalidOperationException("Chunk is not accessible.");
            }

            if (y >= ChunkHeight || y < 0)
            {
                return defaultValue;
            }

            return m_AmbientLights[GetNibbleArrayIndex(x, y, z)];
        }

        public int GetSkyLight(int x, int y, int z, int defaultValue = 0)
        {
            if (!Accessible)
            {
                throw new InvalidOperationException("Chunk is not accessible.");
            }

            if (y >= ChunkHeight || y < 0)
            {
                return defaultValue;
            }

            return m_SkyLights[GetNibbleArrayIndex(x, y, z)];
        }

        public int GetMixedLightLevel(int x, int y, int z, int defaultValue = 0)
        {
            if (!Accessible)
            {
                throw new InvalidOperationException("Chunk is not accessible.");
            }

            if (y >= ChunkHeight || y < 0)
            {
                return defaultValue;
            }

            ref BlockData blockData = ref m_Blocks[x, y, z];
            int result = blockData.LightValue;
            int nIndex = GetNibbleArrayIndex(x, y, z);

            if (m_AmbientLights[nIndex] > result)
            {
                result = m_AmbientLights[nIndex];
            }

            if (m_SkyLights[nIndex] > result)
            {
                result = m_SkyLights[nIndex];
            }
            return result;
        }

        public int GetTopVisibleBlockY(int x, int z, int defaultValue = 0)
        {
            if (!Accessible)
            {
                throw new InvalidOperationException("Chunk is not accessible.");
            }

            return m_HeightMap[x, z];
        }

        // 一般在 Chunk 被 Build 以后调用一次
        public void PostLightAllBlocks()
        {
            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int z = 0; z < ChunkWidth; z++)
                {
                    for (int y = 0; y < ChunkHeight; y++)
                    {
                        BlockData block = m_Blocks[x, y, z];

                        if (block.LightValue > 0)
                        {
                            int worldX = x;
                            int worldY = y;
                            int worldZ = z;
                            this.AccessorSpaceToWorldSpacePosition(ref worldX, ref worldY, ref worldZ);
                            World.LightBlock(worldX, worldY, worldZ, ModificationSource.InternalOrSystem);
                        }
                    }
                }
            }
        }

        public static int GetNibbleArrayIndex(int x, int y, int z)
        {
            return (x << 12) | (y << 4) | z;
        }
    }
}