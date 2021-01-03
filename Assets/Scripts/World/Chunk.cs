using Minecraft.Collections;
using Minecraft.Rendering;
using Minecraft.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Minecraft.WorldConsts;
using Random = System.Random;

namespace Minecraft
{
    public sealed partial class Chunk : IReusableObject, IBinarySerializable, IEquatable<Chunk>
    {
        public delegate void GeometryChangedEventHandler(ChunkMeshSlice mesh);


        public int PositionX { get; private set; }

        public int PositionZ { get; private set; }

        public World World { get; private set; }

        public event GeometryChangedEventHandler GeometryChanged;

        private readonly ChunkData m_Data;
        private readonly ChunkMeshSlice[] m_SolidMeshes;
        private readonly ChunkMeshSlice[] m_FluidMeshes;


        public Chunk()
        {
            m_Data = new ChunkData();
            m_SolidMeshes = new ChunkMeshSlice[SectionCountInChunk];
            m_FluidMeshes = new ChunkMeshSlice[SectionCountInChunk];
        }

        void IReusableObject.OnAllocated() { }

        void IReusableObject.OnFree(bool destroy)
        {
            World = null;
            GeometryChanged = null;
            m_Data.Free(destroy);
            FreeMeshes(m_SolidMeshes, destroy);
            FreeMeshes(m_FluidMeshes, destroy);
        }

        void IBinarySerializable.Serialize(Stream stream)
        {
            int posX = PositionX;
            int posZ = PositionZ;

            stream.WriteByte((byte)posX);
            stream.WriteByte((byte)(posX >> 8));
            stream.WriteByte((byte)(posX >> 16));
            stream.WriteByte((byte)(posX >> 24));

            stream.WriteByte((byte)posZ);
            stream.WriteByte((byte)(posZ >> 8));
            stream.WriteByte((byte)(posZ >> 16));
            stream.WriteByte((byte)(posZ >> 24));

            stream.WriteObject(m_Data);
        }

        void IBinarySerializable.Deserialize(World world, Stream stream)
        {
            int x_b0 = stream.ReadByte();
            int x_b1 = stream.ReadByte();
            int x_b2 = stream.ReadByte();
            int x_b3 = stream.ReadByte();

            int z_b0 = stream.ReadByte();
            int z_b1 = stream.ReadByte();
            int z_b2 = stream.ReadByte();
            int z_b3 = stream.ReadByte();

            PositionX = x_b0 | (x_b1 << 8) | (x_b2 << 16) | (x_b3 << 24);
            PositionZ = z_b0 | (z_b1 << 8) | (z_b2 << 16) | (z_b3 << 24);
            World = world;

            stream.ReadObject(world, m_Data);

            CoreInitialization();
        }

        public void Deconstruct(out int posX, out int posZ)
        {
            posX = PositionX;
            posZ = PositionZ;
        }

        public IEnumerator<ChunkMeshSlice> GetMeshEnumerator()
        {
            for (int i = 0; i < m_SolidMeshes.Length; i++)
            {
                yield return m_SolidMeshes[i];
            }

            for (int i = 0; i < m_FluidMeshes.Length; i++)
            {
                yield return m_FluidMeshes[i];
            }
        }

        public bool Equals(Chunk other)
        {
            return !(other is null) && ReferenceEquals(m_Data, other.m_Data);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Chunk);
        }

        public override int GetHashCode()
        {
            return m_Data.GetHashCode();
        }

        public override string ToString()
        {
            return $"Chunk({PositionX}, {PositionZ})";
        }


        public void Initialize(int posX, int posZ, World world)
        {
            PositionX = posX;
            PositionZ = posZ;
            World = world;

            Random random = new Random(world.Seed);
            FastNoise noise = new FastNoise(world.Seed);
            int waterLevel = 66;

            for (int dx = 0; dx < ChunkWidth; dx++)
            {
                for (int dz = 0; dz < ChunkWidth; dz++)
                {
                    int x = posX + dx;
                    int z = posZ + dz;

                    int bottomHeight = 0;
                    float hills = noise.GetPerlin(x * 4f + 500, z * 4f) * 0.5f + 0.5f;

                    int hillHeight = (int)(BaseHeight + (hills * 16));
                    float bedrock = noise.GetPerlin(x * 64f, z * 64f) * 0.5f + 0.5f;
                    int bedrockHeight = (int)(1 + bedrock * 4);

                    for (int y = 0; y < WorldHeight; y++)
                    {
                        if (y > hillHeight || y < bottomHeight)
                        {
                            if (y < waterLevel)
                            {
                                m_Data.SetBlockId(dx, y, dz, 6);
                            }
                            else
                            {
                                m_Data.SetBlockId(dx, y, dz, Block.AirId);

                                if (y == hillHeight + 1 && m_Data.GetBlockId(dx, y - 1, dz) == 3)
                                {
                                    int r = random.Next(0, 150);

                                    if (r == 1)
                                    {
                                        m_Data.SetBlockId(dx, y, dz, 5);
                                    }
                                }
                            }
                            continue;
                        }

                        if (y < bedrockHeight)
                        {
                            m_Data.SetBlockId(dx, y, dz, 1);
                            continue;
                        }

                        if (y > hillHeight - 4)
                        {
                            if (y == hillHeight)
                            {
                                if (y < waterLevel - 1)
                                {
                                    m_Data.SetBlockId(dx, y, dz, 2);
                                }
                                else
                                {
                                    m_Data.SetBlockId(dx, y, dz, 3);
                                }
                            }
                            else
                            {
                                m_Data.SetBlockId(dx, y, dz, 2);
                            }

                            continue;
                        }

                        m_Data.SetBlockId(dx, y, dz, 2);
                    }
                }
            }

            CoreInitialization();
        }

        private void InitializeDataAndLightBlocks()
        {
            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int z = 0; z < ChunkWidth; z++)
                {
                    int height = -1;

                    for (int y = WorldHeight - 1; y >= 0; y--)
                    {
                        byte id = m_Data.GetBlockId(x, y, z);
                        Block block = World.GetBlock(id);
                        int sectionIndex = Mathf.FloorToInt(y * OverSectionHeight);

                        if (height == -1 && (!block.IsAir))
                        {
                            height = y;
                        }

                        if (block.HasAnyFlag(BlockFlags.NeedsRandomTick))
                        {
                            m_Data.IncreaseTickRefCount(sectionIndex);
                        }

                        if (!block.MeshWriter.IsEmpty)
                        {
                            m_Data.IncreaseVisibleBlockCount(sectionIndex, block);
                        }

                        if (block.LightValue > 0)
                        {
                            World.LightBlock(x + PositionX, y, z + PositionZ);
                        }
                    }

                    m_Data.SetTopNonAirIndex(x, z, (byte)height);
                }
            }
        }

        private void InitializeSkyLight()
        {
            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int z = 0; z < ChunkWidth; z++)
                {
                    UpdateSkyLightData(x, z, m_Data.GetTopNonAirBlockIndex(x, z));
                }
            }
        }

        private void UpdateSkyLightData(int localX, int localZ, int topNonAirBlockY)
        {
            for (int y = topNonAirBlockY + 1; y < WorldHeight; y++)
            {
                m_Data.SetSkyLight(localX, y, localZ, LightingUtility.SkyLight);
            }

            int light = LightingUtility.SkyLight;
            int opacity = 0;

            do
            {
                byte id = m_Data.GetBlockId(localX, topNonAirBlockY, localZ);
                Block block = World.GetBlock(id);

                light = LightingUtility.ClampBlockLight(light - opacity);
                m_Data.SetSkyLight(localX, topNonAirBlockY, localZ, (byte)light);

                opacity = block.LightOpacity;

            } while (--topNonAirBlockY > -1);
        }

        private void CoreInitialization()
        {
            InitializeDataAndLightBlocks();
            InitializeSkyLight();

            InitializeMeshes(m_SolidMeshes, this, MeshSliceType.Solid);
            InitializeMeshes(m_FluidMeshes, this, MeshSliceType.Fluid);

            for (int i = 0; i < SectionCountInChunk; i++)
            {
                InvokeGeometryChangedEvent(i);

                // 因为当前区块没有加载成功时，会默认其所有方块全部不透明，所以通知临近的区块更新
                World.GetChunk(PositionX - ChunkWidth, PositionZ)?.InvokeGeometryChangedEvent(i);
                World.GetChunk(PositionX + ChunkWidth, PositionZ)?.InvokeGeometryChangedEvent(i);
                World.GetChunk(PositionX, PositionZ - ChunkWidth)?.InvokeGeometryChangedEvent(i);
                World.GetChunk(PositionX, PositionZ + ChunkWidth)?.InvokeGeometryChangedEvent(i);
            }
        }


        private void InvokeGeometryChangedEvent(int sectionIndex)
        {
            GeometryChanged?.Invoke(m_SolidMeshes[sectionIndex]);
            GeometryChanged?.Invoke(m_FluidMeshes[sectionIndex]);
        }


        public void RandomTick(Random random, float playerY)
        {
            int sectionIndex = Mathf.FloorToInt(playerY * OverSectionHeight);

            for (int i = -1; i < 2; i++)
            {
                int k = sectionIndex + i;

                if (k < 0 || k >= SectionCountInChunk)
                    continue;

                if (m_Data.GetTickRefCount(k) > 0)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        int x = random.Next(0, ChunkWidth);
                        int z = random.Next(0, ChunkWidth);
                        int j = random.Next(0, SectionHeight);
                        int y = (k * SectionHeight) + j;

                        byte id = m_Data.GetBlockId(x, y, z);
                        Block block = World.GetBlock(id);

                        // 佛系更新
                        if (block.HasAnyFlag(BlockFlags.NeedsRandomTick))
                        {
                            block.Logics.RandomTick(x + PositionX, y, z + PositionZ, block);
                        }
                    }
                }
            }
        }


        private static void InitializeMeshes(ChunkMeshSlice[] meshes, Chunk chunk, MeshSliceType sliceType)
        {
            for (int i = 0; i < meshes.Length; i++)
            {
                if (meshes[i] == null)
                {
                    meshes[i] = new ChunkMeshSlice();
                }

                meshes[i].Initialize(chunk, i, sliceType);
            }
        }

        private static void FreeMeshes(ChunkMeshSlice[] meshes, bool destroy)
        {
            for (int i = 0; i < meshes.Length; i++)
            {
                if (destroy)
                {
                    meshes[i].Destroy();
                }
                else
                {
                    meshes[i].Clear(true);
                }
            }
        }


        public static ulong GetUniqueIdByPosition(int x, int z)
        {
            return ((ulong)x << 32) | (uint)z;
        }

        public static Vector2Int GetChunkPosition(float x, float z)
        {
            int chunkX = Mathf.FloorToInt(x * OverChunkWidth) * ChunkWidth;
            int chunkZ = Mathf.FloorToInt(z * OverChunkWidth) * ChunkWidth;
            return new Vector2Int(chunkX, chunkZ);
        }


        public static implicit operator Vector2Int(Chunk chunk)
        {
            return new Vector2Int(chunk.PositionX, chunk.PositionZ);
        }

        public static bool operator ==(Chunk left, Chunk right)
        {
            bool flag1 = left is null;
            bool flag2 = right is null;

            if (flag1 && flag2)
            {
                return true;
            }

            if (!flag1 && !flag2)
            {
                return left.Equals(right);
            }

            return false;
        }

        public static bool operator !=(Chunk left, Chunk right)
        {
            return !(left == right);
        }
    }
}