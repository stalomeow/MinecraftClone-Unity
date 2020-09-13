using Minecraft.BlocksData;
using Minecraft.Buffers;
using Minecraft.Collections;
using Minecraft.DebugUtils;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static Minecraft.WorldConsts;
using Random = System.Random;

namespace Minecraft
{
    public sealed partial class Chunk : IReusableObject, IDebugMessageSender
    {
        [Flags]
        private enum MeshDirtyFlags : byte
        {
            Neither = 0,

            SolidMesh = 1 << 0,
            LiquidMesh = 1 << 1,

            Both = SolidMesh | LiquidMesh
        }


        string IDebugMessageSender.DisplayName => $"Chunk({PositionX.ToString()},{PositionZ.ToString()})";

        public int PositionX { get; private set; }

        public int PositionZ { get; private set; }

        public bool DisableLog { get; set; }

        public bool IsModified => m_IsModified;

        public bool IsBuildingMesh => m_IsBuildingMesh;

        public bool ShouldUpdateMesh => ((m_DirtyData >> 8) & 0xFFFF) > ushort.MinValue;


        private readonly ChunkData m_Data;
        private readonly Mesh[] m_SolidMeshes;
        private readonly Mesh[] m_LiquidMeshes;
        private readonly ChunkMeshDataBuffer m_MeshDataBuffer;

        private volatile int m_DirtyData;
        private volatile bool m_IsModified;
        private volatile bool m_HasBuildedMesh;
        private volatile bool m_ShouldWaitForNeighborChunksLoaded; // 有chunk被卸载后会重新赋值
        private volatile bool m_IsBuildingMesh;


        public Chunk()
        {
            m_Data = new ChunkData();
            m_SolidMeshes = new Mesh[SectionCountInChunk];
            m_LiquidMeshes = new Mesh[SectionCountInChunk];
            m_MeshDataBuffer = new ChunkMeshDataBuffer();
        }

        void IReusableObject.OnAllocated()
        {
            PositionX = 0;
            PositionZ = 0;
            DisableLog = false;

            SetDirtyData(ushort.MaxValue, MeshDirtyFlags.Both);

            m_IsModified = false;
            m_HasBuildedMesh = false;
            m_ShouldWaitForNeighborChunksLoaded = true;
            m_IsBuildingMesh = false;
        }

        void IReusableObject.OnFree(bool destroy)
        {
            if (destroy)
            {
                m_Data.Dispose();
                m_MeshDataBuffer.Dispose();            
            }
            else
            {
                m_Data.Clear();
            }
        }


        public void MarkAsStartUp()
        {
            m_ShouldWaitForNeighborChunksLoaded = true;
        }


        public void Init(int posX, int posZ)
        {
            PositionX = posX;
            PositionZ = posZ;

            GenerateOtherInitialDataAndLightBlocks();
            GenerateSkyLightData();
        }

        public void Init(int posX, int posZ, int seed, WorldType type)
        {
            PositionX = posX;
            PositionZ = posZ;

            Random random = new Random(seed);
            FastNoise noise = new FastNoise(seed);
            int waterLevel = 66;

            // 地形生成代码借用 https://github.com/bodhid/MineClone-Unity

            for (int dx = 0; dx < ChunkWidth; dx++)
            {
                for (int dz = 0; dz < ChunkWidth; dz++)
                {
                    switch (type)
                    {
                        case WorldType.Normal:
                        case WorldType.Fixed:
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
                                            m_Data.SetBlockType(dx, y, dz, BlockType.Water);
                                        }
                                        else
                                        {
                                            m_Data.SetBlockType(dx, y, dz, BlockType.Air);

                                            if (y == hillHeight + 1 && m_Data.GetBlockType(dx, y - 1, dz) == BlockType.Grass)
                                            {
                                                int r = random.Next(0, 50);

                                                if (r == 1)
                                                {
                                                    m_Data.SetBlockType(dx, y, dz, BlockType.Plant_Grass);
                                                }
                                                else
                                                {
                                                    r = random.Next(0, 150);

                                                    if (r == 1)
                                                    {
                                                        m_Data.SetBlockType(dx, y, dz, BlockType.Flower_Rose_Blue);
                                                    }
                                                }
                                            }
                                        }
                                        continue;
                                    }

                                    if (y < bedrockHeight)
                                    {
                                        m_Data.SetBlockType(dx, y, dz, BlockType.Bedrock);
                                        continue;
                                    }

                                    if (y > hillHeight - 4)
                                    {
                                        if (GenerateCaves(dx, y, dz, x, z, 0.2f, noise))
                                            continue;

                                        if (y == hillHeight)
                                        {
                                            if (y < waterLevel - 1)
                                            {
                                                int r = random.Next(0, 100);

                                                if (r == 1)
                                                {
                                                    m_Data.SetBlockType(dx, y, dz, BlockType.Dirt);
                                                }
                                                else
                                                {
                                                    m_Data.SetBlockType(dx, y, dz, BlockType.Sand);
                                                }
                                            }
                                            else
                                            {
                                                m_Data.SetBlockType(dx, y, dz, BlockType.Grass);
                                            }
                                        }
                                        else
                                        {
                                            m_Data.SetBlockType(dx, y, dz, BlockType.Dirt);
                                        }

                                        continue;
                                    }
                                    else
                                    {
                                        if (GenerateCaves(dx, y, dz, x, z, 0f, noise))
                                            continue;

                                        if (GenerateOres(dx, y, dz, noise)) continue;

                                        m_Data.SetBlockType(dx, y, dz, BlockType.Stone);
                                        continue;
                                    }
                                }
                            }
                            break;
                        case WorldType.Plain:
                            {
                                for (int y = 0; y < WorldHeight; y++)
                                {
                                    switch (y)
                                    {
                                        case 0:
                                            m_Data.SetBlockType(dx, y, dz, BlockType.Bedrock);
                                            break;
                                        case 1:
                                        case 2:
                                            m_Data.SetBlockType(dx, y, dz, BlockType.Dirt);
                                            break;
                                        case 3:
                                            m_Data.SetBlockType(dx, y, dz, BlockType.Grass);
                                            break;
                                        default:
                                            m_Data.SetBlockType(dx, y, dz, BlockType.Air);
                                            break;
                                    }
                                }
                            }
                            break;
                    }
                }
            }

            if (type != WorldType.Plain)
            {
                bool[,] spotsTaken = new bool[16, 16];

                //cave entrances
                if (random.Next() < (int.MaxValue / 20))
                {
                    int h = WorldHeight - 1;

                    while (h-- > 0)
                    {
                        if (m_Data.GetBlockType(8, h, 8) != BlockType.Air)
                        {
                            Queue<Vector3Int> path = new Queue<Vector3Int>();
                            int depth = random.Next(5, 11);

                            for (int i = 0; i < depth; i++)
                            {
                                path.Enqueue(new Vector3Int(random.Next(2, 13), 44 - (i * 4), random.Next(2, 13)));
                            }

                            float d = 0;
                            Vector3Int nextPos = path.Dequeue();

                            while (path.Count > 0)
                            {
                                Vector3Int currentPos = nextPos;
                                nextPos = path.Dequeue();
                                float size = Mathf.Lerp(2, 0.75f, d / depth);

                                for (int i = 0; i < 16; ++i)
                                {
                                    float lerpPos = i / 15f;
                                    Vector3 lerped = Vector3.Lerp(currentPos, nextPos, lerpPos);
                                    Vector3Int p = new Vector3Int((int)lerped.x, (int)lerped.y, (int)lerped.z);

                                    for (int z = -2; z < 3; ++z)
                                    {
                                        for (int y = -2; y < 3; ++y)
                                        {
                                            for (int x = -2; x < 3; ++x)
                                            {
                                                Vector3Int b = new Vector3Int(p.x + x, p.y + y, p.z + z);

                                                if (Vector3Int.Distance(p, b) > size)
                                                    continue;

                                                if (b.x < 0 || b.x > ChunkWidth - 1)
                                                    continue;

                                                if (b.y < 0 || b.y > 47)
                                                    continue;

                                                if (b.z < 0 || b.z > ChunkWidth - 1)
                                                    continue;

                                                int ry = b.y + h + 6 - 48;

                                                if (ry < 0 || ry > ChunkWidth - 1)
                                                    continue;

                                                m_Data.SetBlockType(b.x, ry, b.z, BlockType.Air);
                                            }
                                        }
                                    }
                                }
                                d++;
                            }
                            break;
                        }
                    }
                }

                //trees
                for (int y = 2; y < 14; ++y)
                {
                    for (int x = 2; x < 14; ++x)
                    {
                        if (random.Next() < (int.MaxValue / 100))
                        {
                            if (IsSpotFree(spotsTaken, new Vector2Int(x, y), 2))
                            {
                                spotsTaken[x, y] = true;
                                int h = WorldHeight - 1;

                                while (h-- > 0)
                                {
                                    if (m_Data.GetBlockType(x, h, y) == BlockType.Grass)
                                    {
                                        Vector3Int p = new Vector3Int(x, h + 1, y);

                                        m_Data.SetBlockType(p.x, p.y - 1, p.z, BlockType.Dirt);
                                        bool cutOff = random.Next(100) == 0;

                                        if (cutOff)
                                        {
                                            m_Data.SetBlockType(p.x, p.y, p.z, BlockType.Log_Oak);
                                            goto End;
                                        }

                                        int height = (byte)random.Next(4, 7);
                                        bool superHigh = random.Next(100) == 0;
                                        if (superHigh) height = 10;

                                        for (int i = 0; i < height; ++i)
                                        {
                                            m_Data.SetBlockType(p.x, p.y + i, p.z, BlockType.Log_Oak);
                                        }
                                        m_Data.SetBlockType(p.x, p.y + height, p.z, BlockType.Leaves_Oak);

                                        for (int i = 0; i < 4; ++i)
                                        {
                                            m_Data.SetBlockType(p.x + 1, p.y + height - i, p.z, BlockType.Leaves_Oak);
                                            m_Data.SetBlockType(p.x, p.y + height - i, p.z + 1, BlockType.Leaves_Oak);
                                            m_Data.SetBlockType(p.x - 1, p.y + height - i, p.z, BlockType.Leaves_Oak);
                                            m_Data.SetBlockType(p.x, p.y + height - i, p.z - 1, BlockType.Leaves_Oak);
                                        }

                                        if (random.Next(0, 2) == 0) m_Data.SetBlockType(p.x + 1, p.y + height - 1, p.z + 1, BlockType.Leaves_Oak);
                                        if (random.Next(0, 2) == 0) m_Data.SetBlockType(p.x - 1, p.y + height - 1, p.z + 1, BlockType.Leaves_Oak);
                                        if (random.Next(0, 2) == 0) m_Data.SetBlockType(p.x + 1, p.y + height - 1, p.z - 1, BlockType.Leaves_Oak);
                                        if (random.Next(0, 2) == 0) m_Data.SetBlockType(p.x - 1, p.y + height - 1, p.z - 1, BlockType.Leaves_Oak);

                                        for (int i = 2; i < 4; ++i)
                                        {
                                            m_Data.SetBlockType(p.x + 2, p.y + height - i, p.z - 1, BlockType.Leaves_Oak);
                                            m_Data.SetBlockType(p.x + 2, p.y + height - i, p.z + 0, BlockType.Leaves_Oak);
                                            m_Data.SetBlockType(p.x + 2, p.y + height - i, p.z + 1, BlockType.Leaves_Oak);

                                            m_Data.SetBlockType(p.x - 2, p.y + height - i, p.z - 1, BlockType.Leaves_Oak);
                                            m_Data.SetBlockType(p.x - 2, p.y + height - i, p.z + 0, BlockType.Leaves_Oak);
                                            m_Data.SetBlockType(p.x - 2, p.y + height - i, p.z + 1, BlockType.Leaves_Oak);

                                            m_Data.SetBlockType(p.x - 1, p.y + height - i, p.z + 2, BlockType.Leaves_Oak);
                                            m_Data.SetBlockType(p.x + 0, p.y + height - i, p.z + 2, BlockType.Leaves_Oak);
                                            m_Data.SetBlockType(p.x + 1, p.y + height - i, p.z + 2, BlockType.Leaves_Oak);

                                            m_Data.SetBlockType(p.x - 1, p.y + height - i, p.z - 2, BlockType.Leaves_Oak);
                                            m_Data.SetBlockType(p.x + 0, p.y + height - i, p.z - 2, BlockType.Leaves_Oak);
                                            m_Data.SetBlockType(p.x + 1, p.y + height - i, p.z - 2, BlockType.Leaves_Oak);

                                            m_Data.SetBlockType(p.x + 1, p.y + height - i, p.z + 1, BlockType.Leaves_Oak);
                                            m_Data.SetBlockType(p.x - 1, p.y + height - i, p.z + 1, BlockType.Leaves_Oak);
                                            m_Data.SetBlockType(p.x + 1, p.y + height - i, p.z - 1, BlockType.Leaves_Oak);
                                            m_Data.SetBlockType(p.x - 1, p.y + height - i, p.z - 1, BlockType.Leaves_Oak);

                                            if (random.Next(0, 2) == 0) m_Data.SetBlockType(p.x + 2, p.y + height - i, p.z + 2, BlockType.Leaves_Oak);
                                            if (random.Next(0, 2) == 0) m_Data.SetBlockType(p.x - 2, p.y + height - i, p.z + 2, BlockType.Leaves_Oak);
                                            if (random.Next(0, 2) == 0) m_Data.SetBlockType(p.x + 2, p.y + height - i, p.z - 2, BlockType.Leaves_Oak);
                                            if (random.Next(0, 2) == 0) m_Data.SetBlockType(p.x - 2, p.y + height - i, p.z - 2, BlockType.Leaves_Oak);

                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        End:
            GenerateOtherInitialDataAndLightBlocks();
            GenerateSkyLightData();
        }

        private bool GenerateCaves(int x, int y, int z, int rx, int rz, float threshold, FastNoise noise)
        {
            float cave1 = noise.GetPerlin(rx * 10f - 400, y * 10f, rz * 10f);
            float cave2 = noise.GetPerlin(rx * 20f - 600, y * 20f, rz * 20f);
            float cave3 = noise.GetPerlin(rx * 5f - 200, y * 5f, rz * 5f);
            float cave4 = noise.GetPerlin(rx * 2f - 300, y * 2f, rz * 2f);
            float cave = Mathf.Min(Mathf.Min(cave1, cave4), Mathf.Min(cave2, cave3));

            if (cave > threshold)
            {
                m_Data.SetBlockType(x, y, z, BlockType.Air);
                return true;
            }
            return false;
        }

        private bool GenerateOres(int x, int y, int z, FastNoise noise)
        {
            int worldX = PositionX + x;
            int worldZ = PositionZ + z;

            float ore1 = noise.GetPerlin(worldX * 15f, y * 15f, worldZ * 15f + 300);
            float ore2 = noise.GetPerlin(worldX * 15f, y * 15f, worldZ * 15f + 400);


            if (ore1 > 0.3 && ore2 > 0.4)
            {
                m_Data.SetBlockType(x, y, z, BlockType.Diorite);
                return true;
            }
            if (ore1 < -0.3 && ore2 < -0.4)
            {
                m_Data.SetBlockType(x, y, z, BlockType.Granite);
                return true;
            }

            if (ore1 > 0.3 && ore2 < -0.4)
            {
                m_Data.SetBlockType(x, y, z, BlockType.Dirt);
                return true;
            }


            float ore3 = noise.GetPerlin(worldX * 20f, y * 20f, worldZ * 20f + 500);

            if (ore1 < -0.3 && ore3 > 0.4)
            {
                m_Data.SetBlockType(x, y, z, BlockType.Coal);
                return true;
            }

            float ore4 = noise.GetPerlin(worldX * 21f, y * 21f, worldZ * 21f - 300);

            if (ore4 > 0.6)
            {
                m_Data.SetBlockType(x, y, z, BlockType.Iron);
                return true;
            }

            if (y < 32)
            {
                float ore5 = noise.GetPerlin(worldX * 22f, y * 22f, worldZ * 22f - 400);

                if (ore5 > 0.7)
                {
                    m_Data.SetBlockType(x, y, z, BlockType.Gold);
                    return true;
                }
                if (y < 16)
                {
                    if (ore5 < -0.7)
                    {
                        m_Data.SetBlockType(x, y, z, BlockType.Diamond);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsSpotFree(bool[,] spotsTaken, Vector2Int position, int size) //x area is for example size + 1 + size
        {
            bool spotTaken = false;

            for (int y = Mathf.Max(0, position.y - size); y < Mathf.Min(15, position.y + size + 1); ++y)
            {
                for (int x = Mathf.Max(0, position.x - size); x < Mathf.Min(15, position.x + size + 1); ++x)
                {
                    spotTaken |= spotsTaken[x, y];
                }
            }
            return !spotTaken;
        }

        private void GenerateOtherInitialDataAndLightBlocks()
        {
            WorldManager world = WorldManager.Active;

            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int z = 0; z < ChunkWidth; z++)
                {
                    int height = -1;

                    for (int y = WorldHeight - 1; y >= 0; y--)
                    {
                        BlockType type = m_Data.GetBlockType(x, y, z);
                        Block block = world.DataManager.GetBlockByType(type);
                        int sectionIndex = Mathf.FloorToInt(y * OverSectionHeight);

                        if (type != BlockType.Air && height == -1)
                        {
                            height = y;
                        }

                        if (block.HasAnyFlag(BlockFlags.NeedsRandomTick))
                        {
                            m_Data.IncreaseTickRefCount(sectionIndex);
                        }

                        if (block.VertexType != BlockVertexType.None)
                        {
                            if (block.HasAnyFlag(BlockFlags.Liquid))
                            {
                                m_Data.IncreaseRenderableLiquidCount(sectionIndex);
                            }
                            else
                            {
                                m_Data.IncreaseRenderableSolidCount(sectionIndex);
                            }
                        }

                        if (block.LightValue > 0)
                        {
                            world.ChunkManager.LightBlock(x + PositionX, y, z + PositionZ);
                        }
                    }

                    m_Data.SetTopNonAirIndex(x, z, (byte)height);
                }
            }
        }


        public void RandomTick(Random random, float playerY)
        {
            DataManager mgr = WorldManager.Active.DataManager;
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
                        int y = k * SectionHeight + j;

                        BlockType type = m_Data.GetBlockType(x, y, z);
                        mgr.GetBlockByType(type).OnRandomTick(x + PositionX, y, z + PositionZ);
                    }
                }
            }
        }
        

        private void GenerateSkyLightData()
        {
            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int z = 0; z < ChunkWidth; z++)
                {
                    UpdateSkyLightData(x, z);
                }
            }
        }

        private void UpdateSkyLightData(int localX, int localZ)
        {
            UpdateSkyLightData(localX, localZ, m_Data.GetTopNonAirIndex(localX, localZ));
        }

        private void UpdateSkyLightData(int localX, int localZ, int topNonAirBlockY)
        {
            for (int y = topNonAirBlockY + 1; y < WorldHeight; y++)
            {
                m_Data.SetSkyLight(localX, y, localZ, SkyLight);
            }

            int light = SkyLight;
            int opacity = 0;

            do
            {
                BlockType type = m_Data.GetBlockType(localX, topNonAirBlockY, localZ);
                Block block = WorldManager.Active.DataManager.GetBlockByType(type);

                light = Mathf.Clamp(light - opacity, 0, MaxNonAirBlockSkyLightValue);
                m_Data.SetSkyLight(localX, topNonAirBlockY, localZ, (byte)light);

                opacity = block.LightOpacity;

            } while (--topNonAirBlockY > -1);
        }


        private void SetDirtyData(ushort dirtySectionIndices, MeshDirtyFlags flags)
        {
            Interlocked.Exchange(ref m_DirtyData, (dirtySectionIndices << 8) | (int)flags);
        }

        private void SetMeshDirty(int dirtySectionIndex, MeshDirtyFlags flags)
        {
            int data = m_DirtyData;

            ushort indices = (ushort)((data >> 8) & 0xFFFF);
            indices |= (ushort)(1 << dirtySectionIndex);

            MeshDirtyFlags oldFlags = (MeshDirtyFlags)(data & 0xFF);
            flags |= oldFlags;

            Interlocked.Exchange(ref m_DirtyData, (indices << 8) | (int)flags);
        }

        public void GetRawData(out byte[] blocks, out byte[] states)
        {
            m_Data.GetRawBlockData(out blocks, out states);
        }

        public void OnSaved()
        {
            this.Log("Saved!");

            m_IsModified = false;
        }


        public static ulong GetUniqueIdByPosition(int x, int z)
        {
            return ((ulong)x << 32) | (uint)z;
        }

        public static Vector2Int NormalizeToChunkPosition(float x, float z)
        {
            int chunkX = Mathf.FloorToInt(x * OverChunkWidth) * ChunkWidth;
            int chunkZ = Mathf.FloorToInt(z * OverChunkWidth) * ChunkWidth;
            return new Vector2Int(chunkX, chunkZ);
        }
    }
}