using Minecraft.BlocksData;
using Minecraft.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using static Minecraft.BlocksData.BlockVertexHelper;
using static Minecraft.WorldConsts;
using Random = System.Random;

namespace Minecraft
{
    public sealed class Chunk : IReusableObject
    {
        [Flags]
        private enum MeshUpdateFlags : byte
        {
            Neither = 0,

            SolidMesh = 1 << 0,
            LiquidMesh = 1 << 1,

            LightingUpdate = 1 << 2,

            LightingSolidMesh = LightingUpdate | SolidMesh,
            LightingLiquidMesh = LightingUpdate | LiquidMesh,

            Both = SolidMesh | LiquidMesh,
            LightingBoth = LightingUpdate | Both
        }


        private static readonly Bounds s_ChunkBounds = new Bounds(
            new Vector3(ChunkWidth * 0.5f, WorldHeight * 0.5f, ChunkWidth * 0.5f),
            new Vector3(ChunkWidth, WorldHeight, ChunkWidth)
        );


        public int PositionX { get; private set; }

        public int PositionZ { get; private set; }

        public bool IsModified { get; private set; }

        public Mesh SolidMesh { get; private set; }

        public Mesh LiquidMesh { get; private set; }

        public bool ShouldUpdateMesh => m_MeshUpdateFlags != MeshUpdateFlags.Neither;

        private byte[] m_Blocks; // 所有方块信息
        private byte[] m_BlockStates; // 所有方块的状态
        private byte[] m_HeightMap; // chunk 的高度图, 第一个非空方块的y
        private int[] m_TickRefCounts; // 每一个section（高度16）需要tick的数量
        private NibbleArray m_SkyLights; // 每一个方块受到的天空光照值，不公开
        private NibbleArray m_BlockLights; // 每一个方块受到的由其他方块引起的光照值

        private MeshUpdateFlags m_MeshUpdateFlags;
        private bool m_IsStartUp; // 有chunk被卸载后会重新赋值
        private bool m_IsBuildingMesh;
        private List<VertexData> m_VertexBuffer;
        private List<int> m_TrianglesBuffer;

        private object m_SyncLock;


        public Chunk()
        {
            m_Blocks = new byte[BlockCountInChunk];
            m_BlockStates = new byte[BlockCountInChunk];
            m_HeightMap = new byte[ChunkWidth * ChunkWidth];
            m_TickRefCounts = new int[SectionCountInChunk];
            m_SkyLights = new NibbleArray(BlockCountInChunk);
            m_BlockLights = new NibbleArray(BlockCountInChunk);

            m_VertexBuffer = new List<VertexData>();
            m_TrianglesBuffer = new List<int>();

            m_SyncLock = new object();
        }

        void IReusableObject.OnAllocated()
        {
            PositionX = 0;
            PositionZ = 0;
            IsModified = false;

            m_MeshUpdateFlags = MeshUpdateFlags.Both;
            m_IsStartUp = true;
            m_IsBuildingMesh = false;
        }

        void IReusableObject.OnFree(bool destroy)
        {
            if (destroy)
            {
                SolidMesh = null;
                LiquidMesh = null;

                m_Blocks = null;
                m_BlockStates = null;
                m_HeightMap = null;
                m_TickRefCounts = null;
                m_SkyLights = null;
                m_BlockLights = null;

                m_VertexBuffer = null;
                m_TrianglesBuffer = null;

                m_SyncLock = null;
            }
            else
            {
                // mesh 会在build前clear

                Array.Clear(m_Blocks, 0, m_Blocks.Length);
                Array.Clear(m_BlockStates, 0, m_BlockStates.Length);
                Array.Clear(m_HeightMap, 0, m_HeightMap.Length);
                Array.Clear(m_TickRefCounts, 0, m_TickRefCounts.Length);

                m_SkyLights.Clear();
                m_BlockLights.Clear();
            }
        }


        public void MarkAsStartUp()
        {
            m_IsStartUp = true;
        }


        public int GetHighestNonAirY(int worldX, int worldZ) => GetHighestNonAirYPrivate(worldX - PositionX, worldZ - PositionZ);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetHighestNonAirYPrivate(int localX, int localZ) => m_HeightMap[(localX << 4) | localZ];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetHighestNonAirY(int localX, int localZ, byte value) => m_HeightMap[(localX << 4) | localZ] = value;

        public byte GetFinalLightLevel(int worldX, int y, int worldZ) => GetFinalLightLevelPrivate(worldX - PositionX, y, worldZ - PositionZ);

        private byte GetFinalLightLevelPrivate(int localX, int y, int localZ)
        {
            if (y >= WorldHeight || y < 0)
            {
                return MaxLight; // default
            }

            BlockType type = (BlockType)m_Blocks[(localX << 12) | (y << 4) | localZ];
            Block block = WorldManager.Active.DataManager.GetBlockByType(type);

            byte skyLight = (byte)Mathf.Clamp(m_SkyLights[(localX << 12) | (y << 4) | localZ] - SkyLightSubtracted, 0, MaxLight); // temp
            byte blockLight = m_BlockLights[(localX << 12) | (y << 4) | localZ];
            byte light = block.LightValue;

            // MAX(skyLight, blockLight, emission)

            if (skyLight > light)
                light = skyLight;

            if (blockLight > light)
                light = blockLight;

            return light;
        }

        public byte GetSkyLight(int worldX, int y, int worldZ) => GetSkyLightPrivate(worldX - PositionX, y, worldZ - PositionZ);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte GetSkyLightPrivate(int localX, int y, int localZ) => y >= WorldHeight || y < 0 ? MaxLight : m_SkyLights[(localX << 12) | (y << 4) | localZ];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetSkyLight(int localX, int y, int localZ, byte value) => m_SkyLights[(localX << 12) | (y << 4) | localZ] = value;

        public byte GetBlockLight(int worldX, int y, int worldZ) => GetBlockLightPrivate(worldX - PositionX, y, worldZ - PositionZ);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte GetBlockLightPrivate(int localX, int y, int localZ) => y >= WorldHeight || y < 0 ? (byte)0 : m_BlockLights[(localX << 12) | (y << 4) | localZ];

        public void SetBlockLight(int worldX, int y, int worldZ, byte value) => SetBlockLightPrivate(worldX - PositionX, y, worldZ - PositionZ, value);

        private void SetBlockLightPrivate(int localX, int y, int localZ, byte value)
        {
            if (y >= WorldHeight || y < 0)
                return;

            lock (m_SyncLock)
            {
                m_BlockLights[(localX << 12) | (y << 4) | localZ] = value;
            }

            BlockType type = GetBlockTypePrivateUnchecked(localX, y, localZ);
            Block block = WorldManager.Active.DataManager.GetBlockByType(type);
            m_MeshUpdateFlags |= block.HasAnyFlag(BlockFlags.Liquid) ? MeshUpdateFlags.LightingBoth : MeshUpdateFlags.LightingSolidMesh; // 液体的mesh不计算环境光照
        }

        public BlockType GetBlockType(int worldX, int y, int worldZ)
        {
            int index = ((worldX - PositionX) << 12) | (y << 4) | (worldZ - PositionZ);
            return index < 0 || index >= m_Blocks.Length ? BlockType.Air : (BlockType)m_Blocks[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private BlockType GetBlockTypePrivateUnchecked(int localX, int y, int localZ) => (BlockType)m_Blocks[(localX << 12) | (y << 4) | localZ];

        // return: 是否设置成功
        public bool SetBlockType(int worldX, int y, int worldZ, BlockType value, byte state = 0, bool lightBlocks = true, bool tickBlocks = true, bool updateNeighborChunks = true)
        {
            int localX = worldX - PositionX;
            int localZ = worldZ - PositionZ;

            int index = (localX << 12) | (y << 4) | localZ;

            if (index < 0 || index >= m_Blocks.Length)
                return false;

            BlockType previousBlockType = (BlockType)m_Blocks[index];

            if (previousBlockType == value)
                return false;

            lock (m_SyncLock)
            {
                m_Blocks[index] = (byte)value;
                SetBlockStatePrivateUnchecked(localX, y, localZ, state);

                int height = GetHighestNonAirYPrivate(localX, localZ);

                if (y >= height)
                {
                    for (int i = y; i > -1; i--)
                    {
                        if (GetBlockTypePrivateUnchecked(localX, i, localZ) != BlockType.Air)
                        {
                            SetHighestNonAirY(localX, localZ, (byte)i); // 至少会有一个非空方块，比如基岩
                            break;
                        }
                    }
                }
            }

            WorldManager world = WorldManager.Active;
            ChunkManager manager = world.ChunkManager;
            DataManager dataManager = world.DataManager;

            Block previousBlock = dataManager.GetBlockByType(previousBlockType);
            Block block = dataManager.GetBlockByType(value);
            int sectionIndex = Mathf.FloorToInt(y * OverSectionHeight);

            if (previousBlock.HasAnyFlag(BlockFlags.NeedsRandomTick))
            {
                m_TickRefCounts[sectionIndex]--;
            }

            if (block.HasAnyFlag(BlockFlags.NeedsRandomTick))
            {
                m_TickRefCounts[sectionIndex]++;
            }

            if (tickBlocks)
            {
                manager.TickBlock(worldX, y, worldZ);
            }

            if (lightBlocks)
            {
                manager.LightBlock(worldX, y, worldZ);
            }

            if (updateNeighborChunks)
            {
                if (localX == 0)
                {
                    Chunk chunk = manager.GetChunk(PositionX - ChunkWidth, PositionZ);

                    if (chunk != null)
                    {
                        chunk.m_MeshUpdateFlags = MeshUpdateFlags.Both;
                    }
                }
                else if (localX == ChunkWidth - 1)
                {
                    Chunk chunk = manager.GetChunk(PositionX + ChunkWidth, PositionZ);

                    if (chunk != null)
                    {
                        chunk.m_MeshUpdateFlags = MeshUpdateFlags.Both;
                    }
                }

                if (localZ == 0)
                {
                    Chunk chunk = manager.GetChunk(PositionX, PositionZ - ChunkWidth);

                    if (chunk != null)
                    {
                        chunk.m_MeshUpdateFlags = MeshUpdateFlags.Both;
                    }
                }
                else if (localZ == ChunkWidth - 1)
                {
                    Chunk chunk = manager.GetChunk(PositionX, PositionZ + ChunkWidth);

                    if (chunk != null)
                    {
                        chunk.m_MeshUpdateFlags = MeshUpdateFlags.Both;
                    }
                }
            }

            bool flag1 = previousBlock.HasAnyFlag(BlockFlags.Liquid);
            bool flag2 = block.HasAnyFlag(BlockFlags.Liquid);

            if (flag1 != flag2)
            {
                m_MeshUpdateFlags |= block.VertexType == BlockVertexType.None || previousBlock.VertexType == BlockVertexType.None
                    ? MeshUpdateFlags.LiquidMesh
                    : MeshUpdateFlags.Both;
            }
            else
            {
                m_MeshUpdateFlags |= flag2 ? MeshUpdateFlags.LiquidMesh : MeshUpdateFlags.SolidMesh;
            }

            return IsModified = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetBlockTypePrivate(int localX, int y, int localZ, BlockType value) => m_Blocks[(localX << 12) | (y << 4) | localZ] = (byte)value;

        private void SetBlockStatePrivateUnchecked(int localX, int y, int localZ, byte value) => m_BlockStates[(localX << 12) | (y << 4) | localZ] = value;

        public void SetBlockState(int worldX, int y, int worldZ, byte value)
        {
            int localX = worldX - PositionX;
            int localZ = worldZ - PositionZ;

            int index = (localX << 12) | (y << 4) | localZ;

            if (index < 0 || index >= m_Blocks.Length)
                return;

            if (m_BlockStates[index] != value)
            {
                m_BlockStates[index] = value;
                IsModified = true;
            }
        }

        public byte GetBlockState(int worldX, int y, int worldZ)
        {
            int localX = worldX - PositionX;
            int localZ = worldZ - PositionZ;

            int index = (localX << 12) | (y << 4) | localZ;

            if (index < 0 || index >= m_Blocks.Length)
                return default;

            return m_BlockStates[index];
        }

        public void Init(int posX, int posZ)
        {
            PositionX = posX;
            PositionZ = posZ;

            GenerateHeightMapAndTickCountMapAndLightBlocks();
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
                                            SetBlockTypePrivate(dx, y, dz, BlockType.Water);
                                        }
                                        else
                                        {
                                            SetBlockTypePrivate(dx, y, dz, BlockType.Air);

                                            if (y == hillHeight + 1 && GetBlockTypePrivateUnchecked(dx, y - 1, dz) == BlockType.Grass)
                                            {
                                                int r = random.Next(0, 50);

                                                if (r == 1)
                                                {
                                                    SetBlockTypePrivate(dx, y, dz, BlockType.Plant_Grass);
                                                }
                                                else
                                                {
                                                    r = random.Next(0, 150);

                                                    if (r == 1)
                                                    {
                                                        SetBlockTypePrivate(dx, y, dz, BlockType.Flower_Rose_Blue);
                                                    }
                                                }
                                            }
                                        }
                                        continue;
                                    }

                                    if (y < bedrockHeight)
                                    {
                                        SetBlockTypePrivate(dx, y, dz, BlockType.Bedrock);
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
                                                    SetBlockTypePrivate(dx, y, dz, BlockType.Dirt);
                                                }
                                                else
                                                {
                                                    SetBlockTypePrivate(dx, y, dz, BlockType.Sand);
                                                }
                                            }
                                            else
                                            {
                                                SetBlockTypePrivate(dx, y, dz, BlockType.Grass);
                                            }
                                        }
                                        else
                                        {
                                            SetBlockTypePrivate(dx, y, dz, BlockType.Dirt);
                                        }

                                        continue;
                                    }
                                    else
                                    {
                                        if (GenerateCaves(dx, y, dz, x, z, 0f, noise))
                                            continue;

                                        if (GenerateOres(dx, y, dz, noise)) continue;

                                        SetBlockTypePrivate(dx, y, dz, BlockType.Stone);
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
                                            SetBlockTypePrivate(dx, y, dz, BlockType.Bedrock);
                                            break;
                                        case 1:
                                        case 2:
                                            SetBlockTypePrivate(dx, y, dz, BlockType.Dirt);
                                            break;
                                        case 3:
                                            SetBlockTypePrivate(dx, y, dz, BlockType.Grass);
                                            break;
                                        default:
                                            SetBlockTypePrivate(dx, y, dz, BlockType.Air);
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
                        if (GetBlockTypePrivateUnchecked(8, h, 8) != BlockType.Air)
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

                                                SetBlockTypePrivate(b.x, ry, b.z, BlockType.Air);
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
                                    if (GetBlockTypePrivateUnchecked(x, h, y) == BlockType.Grass)
                                    {
                                        Vector3Int p = new Vector3Int(x, h + 1, y);

                                        SetBlockTypePrivate(p.x, p.y - 1, p.z, BlockType.Dirt);
                                        bool cutOff = random.Next(100) == 0;

                                        if (cutOff)
                                        {
                                            SetBlockTypePrivate(p.x, p.y, p.z, BlockType.Log_Oak);
                                            goto End;
                                        }

                                        int height = (byte)random.Next(4, 7);
                                        bool superHigh = random.Next(100) == 0;
                                        if (superHigh) height = 10;

                                        for (int i = 0; i < height; ++i)
                                        {
                                            SetBlockTypePrivate(p.x, p.y + i, p.z, BlockType.Log_Oak);
                                        }
                                        SetBlockTypePrivate(p.x, p.y + height, p.z, BlockType.Leaves_Oak);

                                        for (int i = 0; i < 4; ++i)
                                        {
                                            SetBlockTypePrivate(p.x + 1, p.y + height - i, p.z, BlockType.Leaves_Oak);
                                            SetBlockTypePrivate(p.x, p.y + height - i, p.z + 1, BlockType.Leaves_Oak);
                                            SetBlockTypePrivate(p.x - 1, p.y + height - i, p.z, BlockType.Leaves_Oak);
                                            SetBlockTypePrivate(p.x, p.y + height - i, p.z - 1, BlockType.Leaves_Oak);
                                        }

                                        if (random.Next(0, 2) == 0) SetBlockTypePrivate(p.x + 1, p.y + height - 1, p.z + 1, BlockType.Leaves_Oak);
                                        if (random.Next(0, 2) == 0) SetBlockTypePrivate(p.x - 1, p.y + height - 1, p.z + 1, BlockType.Leaves_Oak);
                                        if (random.Next(0, 2) == 0) SetBlockTypePrivate(p.x + 1, p.y + height - 1, p.z - 1, BlockType.Leaves_Oak);
                                        if (random.Next(0, 2) == 0) SetBlockTypePrivate(p.x - 1, p.y + height - 1, p.z - 1, BlockType.Leaves_Oak);

                                        for (int i = 2; i < 4; ++i)
                                        {
                                            SetBlockTypePrivate(p.x + 2, p.y + height - i, p.z - 1, BlockType.Leaves_Oak);
                                            SetBlockTypePrivate(p.x + 2, p.y + height - i, p.z + 0, BlockType.Leaves_Oak);
                                            SetBlockTypePrivate(p.x + 2, p.y + height - i, p.z + 1, BlockType.Leaves_Oak);

                                            SetBlockTypePrivate(p.x - 2, p.y + height - i, p.z - 1, BlockType.Leaves_Oak);
                                            SetBlockTypePrivate(p.x - 2, p.y + height - i, p.z + 0, BlockType.Leaves_Oak);
                                            SetBlockTypePrivate(p.x - 2, p.y + height - i, p.z + 1, BlockType.Leaves_Oak);

                                            SetBlockTypePrivate(p.x - 1, p.y + height - i, p.z + 2, BlockType.Leaves_Oak);
                                            SetBlockTypePrivate(p.x + 0, p.y + height - i, p.z + 2, BlockType.Leaves_Oak);
                                            SetBlockTypePrivate(p.x + 1, p.y + height - i, p.z + 2, BlockType.Leaves_Oak);

                                            SetBlockTypePrivate(p.x - 1, p.y + height - i, p.z - 2, BlockType.Leaves_Oak);
                                            SetBlockTypePrivate(p.x + 0, p.y + height - i, p.z - 2, BlockType.Leaves_Oak);
                                            SetBlockTypePrivate(p.x + 1, p.y + height - i, p.z - 2, BlockType.Leaves_Oak);

                                            SetBlockTypePrivate(p.x + 1, p.y + height - i, p.z + 1, BlockType.Leaves_Oak);
                                            SetBlockTypePrivate(p.x - 1, p.y + height - i, p.z + 1, BlockType.Leaves_Oak);
                                            SetBlockTypePrivate(p.x + 1, p.y + height - i, p.z - 1, BlockType.Leaves_Oak);
                                            SetBlockTypePrivate(p.x - 1, p.y + height - i, p.z - 1, BlockType.Leaves_Oak);

                                            if (random.Next(0, 2) == 0) SetBlockTypePrivate(p.x + 2, p.y + height - i, p.z + 2, BlockType.Leaves_Oak);
                                            if (random.Next(0, 2) == 0) SetBlockTypePrivate(p.x - 2, p.y + height - i, p.z + 2, BlockType.Leaves_Oak);
                                            if (random.Next(0, 2) == 0) SetBlockTypePrivate(p.x + 2, p.y + height - i, p.z - 2, BlockType.Leaves_Oak);
                                            if (random.Next(0, 2) == 0) SetBlockTypePrivate(p.x - 2, p.y + height - i, p.z - 2, BlockType.Leaves_Oak);

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
            GenerateHeightMapAndTickCountMapAndLightBlocks();
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
                m_Blocks[(x << 12) | (y << 4) | z] = (byte)BlockType.Air;
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
                SetBlockTypePrivate(x, y, z, BlockType.Diorite);
                return true;
            }
            if (ore1 < -0.3 && ore2 < -0.4)
            {
                SetBlockTypePrivate(x, y, z, BlockType.Granite);
                return true;
            }

            if (ore1 > 0.3 && ore2 < -0.4)
            {
                SetBlockTypePrivate(x, y, z, BlockType.Dirt);
                return true;
            }


            float ore3 = noise.GetPerlin(worldX * 20f, y * 20f, worldZ * 20f + 500);

            if (ore1 < -0.3 && ore3 > 0.4)
            {
                SetBlockTypePrivate(x, y, z, BlockType.Coal);
                return true;
            }

            float ore4 = noise.GetPerlin(worldX * 21f, y * 21f, worldZ * 21f - 300);

            if (ore4 > 0.6)
            {
                SetBlockTypePrivate(x, y, z, BlockType.Iron);
                return true;
            }

            if (y < 32)
            {
                float ore5 = noise.GetPerlin(worldX * 22f, y * 22f, worldZ * 22f - 400);

                if (ore5 > 0.7)
                {
                    SetBlockTypePrivate(x, y, z, BlockType.Gold);
                    return true;
                }
                if (y < 16)
                {
                    if (ore5 < -0.7)
                    {
                        SetBlockTypePrivate(x, y, z, BlockType.Diamond);
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

        private void GenerateHeightMapAndTickCountMapAndLightBlocks()
        {
            WorldManager world = WorldManager.Active;

            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int z = 0; z < ChunkWidth; z++)
                {
                    int height = -1;

                    for (int y = WorldHeight - 1; y >= 0; y--)
                    {
                        BlockType type = GetBlockTypePrivateUnchecked(x, y, z);
                        Block block = world.DataManager.GetBlockByType(type);

                        if (type != BlockType.Air && height == -1)
                        {
                            height = y;
                        }

                        if (block.HasAnyFlag(BlockFlags.NeedsRandomTick))
                        {
                            m_TickRefCounts[Mathf.FloorToInt(y * OverSectionHeight)]++;
                        }

                        if (block.LightValue > 0)
                        {
                            world.ChunkManager.LightBlock(x + PositionX, y, z + PositionZ);
                        }
                    }

                    SetHighestNonAirY(x, z, (byte)height);
                }
            }
        }


        public void RandomTick(Random random)
        {
            WorldManager world = WorldManager.Active;

            for (int i = 0; i < SectionCountInChunk; i++)
            {
                if (m_TickRefCounts[i] > 0)
                {
                    int x = random.Next(0, ChunkWidth);
                    int z = random.Next(0, ChunkWidth);
                    int j = random.Next(0, SectionHeight);
                    int y = i * SectionHeight + j;
                    
                    // 其实基本就tick不到重要的方块...

                    BlockType type = GetBlockTypePrivateUnchecked(x, y, z);
                    world.DataManager.GetBlockByType(type).OnRandomTick(x + PositionX, y, z + PositionZ);
                }
            }
        }
        

        public async void StartBuildMesh()
        {
            if (m_IsBuildingMesh || !ShouldUpdateMesh)
                return;
            
            m_IsBuildingMesh = true;

            MeshUpdateFlags updateFlags = m_MeshUpdateFlags;
            m_MeshUpdateFlags = MeshUpdateFlags.Neither;

            bool updateSolid = (updateFlags & MeshUpdateFlags.SolidMesh) == MeshUpdateFlags.SolidMesh;
            bool updateLiquid = (updateFlags & MeshUpdateFlags.LiquidMesh) == MeshUpdateFlags.LiquidMesh;
            bool lightingUpdate = (updateFlags & MeshUpdateFlags.LightingUpdate) == MeshUpdateFlags.LightingUpdate;

            if (lightingUpdate && (SolidMesh == null || SolidMesh.vertexCount == 0))
            {
                updateSolid = false;
            }

            if (lightingUpdate && (LiquidMesh == null || LiquidMesh.vertexCount == 0))
            {
                updateLiquid = false;
            }

            try
            {
                Monitor.Enter(m_SyncLock);

                if (m_IsStartUp)
                {
                    m_IsStartUp = false;

                    await WorldManager.Active.ChunkManager.WaitForAllNeighborChunksLoaded(this);
                }
                
                await Task.Factory.StartNew(GenerateSkyLightData);

                if (updateSolid)
                {
                    m_VertexBuffer.Clear();
                    m_TrianglesBuffer.Clear();

                    await Task.Factory.StartNew(BuildSolidMesh);

                    if (m_VertexBuffer.Count > 0 && m_TrianglesBuffer.Count > 0)
                    {
                        if (SolidMesh == null)
                        {
                            SolidMesh = new Mesh
                            {
                                indexFormat = SystemInfo.supports32bitsIndexBuffer ? IndexFormat.UInt32 : IndexFormat.UInt16,
                                bounds = s_ChunkBounds
                            };
                            SolidMesh.MarkDynamic();
                        }
                        else
                        {
                            SolidMesh.Clear();
                        }

                        SolidMesh.SetVertexBufferParams(m_VertexBuffer.Count, VertexLayout);
                        SolidMesh.SetVertexBufferData(m_VertexBuffer, 0, 0, m_VertexBuffer.Count);
                        SolidMesh.SetTriangles(m_TrianglesBuffer, 0);
                        SolidMesh.UploadMeshData(false);
                    }
                }

                if (updateLiquid)
                {
                    m_VertexBuffer.Clear();
                    m_TrianglesBuffer.Clear();

                    await Task.Factory.StartNew(BuildLiquidMesh);

                    if (m_VertexBuffer.Count > 0 && m_TrianglesBuffer.Count > 0)
                    {
                        if (LiquidMesh == null)
                        {
                            LiquidMesh = new Mesh
                            {
                                indexFormat = SystemInfo.supports32bitsIndexBuffer ? IndexFormat.UInt32 : IndexFormat.UInt16,
                                bounds = s_ChunkBounds
                            };
                            LiquidMesh.MarkDynamic();
                        }
                        else
                        {
                            LiquidMesh.Clear();
                        }

                        LiquidMesh.SetVertexBufferParams(m_VertexBuffer.Count, VertexLayout);
                        LiquidMesh.SetVertexBufferData(m_VertexBuffer, 0, 0, m_VertexBuffer.Count);
                        LiquidMesh.SetTriangles(m_TrianglesBuffer, 0);
                        LiquidMesh.UploadMeshData(false);
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

        private void BuildSolidMesh()
        {
            WorldManager world = WorldManager.Active;
            Profiler.BeginSample("Build Solid Mesh");

            try
            {
                for (int dx = 0; dx < ChunkWidth; dx++)
                {
                    for (int dz = 0; dz < ChunkWidth; dz++)
                    {
                        int rx = PositionX + dx;
                        int rz = PositionZ + dz;

                        int height = GetHighestNonAirYPrivate(dx, dz) + 1;

                        for (int ry = 0; ry < height; ry++)
                        {
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

                                            AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                                            AddCubeVertexDataPX(dx, ry, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, m_VertexBuffer);
                                        }

                                        if (world.IsBlockTransparent(rx - 1, ry, rz))
                                        {
                                            float lb = (world.GetFinalLightLevel(rx - 1, ry, rz) + world.GetFinalLightLevel(rx - 1, ry - 1, rz) + world.GetFinalLightLevel(rx - 1, ry, rz + 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float rb = (world.GetFinalLightLevel(rx - 1, ry, rz) + world.GetFinalLightLevel(rx - 1, ry - 1, rz) + world.GetFinalLightLevel(rx - 1, ry, rz - 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rt = (world.GetFinalLightLevel(rx - 1, ry, rz) + world.GetFinalLightLevel(rx - 1, ry + 1, rz) + world.GetFinalLightLevel(rx - 1, ry, rz - 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float lt = (world.GetFinalLightLevel(rx - 1, ry, rz) + world.GetFinalLightLevel(rx - 1, ry + 1, rz) + world.GetFinalLightLevel(rx - 1, ry, rz + 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz + 1)) * 0.25f * OverMaxLight;

                                            AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                                            AddCubeVertexDataNX(dx, ry, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, m_VertexBuffer);
                                        }

                                        if (world.IsBlockTransparent(rx, ry + 1, rz))
                                        {
                                            float lb = (world.GetFinalLightLevel(rx, ry + 1, rz) + world.GetFinalLightLevel(rx, ry + 1, rz - 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz) + world.GetFinalLightLevel(rx - 1, ry + 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rb = (world.GetFinalLightLevel(rx, ry + 1, rz) + world.GetFinalLightLevel(rx, ry + 1, rz - 1) + world.GetFinalLightLevel(rx + 1, ry + 1, rz) + world.GetFinalLightLevel(rx + 1, ry + 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rt = (world.GetFinalLightLevel(rx, ry + 1, rz) + world.GetFinalLightLevel(rx, ry + 1, rz + 1) + world.GetFinalLightLevel(rx + 1, ry + 1, rz) + world.GetFinalLightLevel(rx + 1, ry + 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float lt = (world.GetFinalLightLevel(rx, ry + 1, rz) + world.GetFinalLightLevel(rx, ry + 1, rz + 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz) + world.GetFinalLightLevel(rx - 1, ry + 1, rz + 1)) * 0.25f * OverMaxLight;

                                            AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                                            AddCubeVertexDataPY(dx, ry, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, m_VertexBuffer);
                                        }

                                        if (ry > 0 && world.IsBlockTransparent(rx, ry - 1, rz))
                                        {
                                            float lb = (world.GetFinalLightLevel(rx, ry - 1, rz) + world.GetFinalLightLevel(rx, ry - 1, rz - 1) + world.GetFinalLightLevel(rx + 1, ry - 1, rz) + world.GetFinalLightLevel(rx + 1, ry - 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rb = (world.GetFinalLightLevel(rx, ry - 1, rz) + world.GetFinalLightLevel(rx, ry - 1, rz - 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz) + world.GetFinalLightLevel(rx - 1, ry - 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rt = (world.GetFinalLightLevel(rx, ry - 1, rz) + world.GetFinalLightLevel(rx, ry - 1, rz + 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz) + world.GetFinalLightLevel(rx - 1, ry - 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float lt = (world.GetFinalLightLevel(rx, ry - 1, rz) + world.GetFinalLightLevel(rx, ry - 1, rz + 1) + world.GetFinalLightLevel(rx + 1, ry - 1, rz) + world.GetFinalLightLevel(rx + 1, ry - 1, rz + 1)) * 0.25f * OverMaxLight;

                                            AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                                            AddCubeVertexDataNY(dx, ry, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, m_VertexBuffer);
                                        }

                                        if (world.IsBlockTransparent(rx, ry, rz + 1))
                                        {
                                            float lb = (world.GetFinalLightLevel(rx, ry, rz + 1) + world.GetFinalLightLevel(rx, ry - 1, rz + 1) + world.GetFinalLightLevel(rx + 1, ry, rz + 1) + world.GetFinalLightLevel(rx + 1, ry - 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float rb = (world.GetFinalLightLevel(rx, ry, rz + 1) + world.GetFinalLightLevel(rx, ry - 1, rz + 1) + world.GetFinalLightLevel(rx - 1, ry, rz + 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float rt = (world.GetFinalLightLevel(rx, ry, rz + 1) + world.GetFinalLightLevel(rx, ry + 1, rz + 1) + world.GetFinalLightLevel(rx - 1, ry, rz + 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float lt = (world.GetFinalLightLevel(rx, ry, rz + 1) + world.GetFinalLightLevel(rx, ry + 1, rz + 1) + world.GetFinalLightLevel(rx + 1, ry, rz + 1) + world.GetFinalLightLevel(rx + 1, ry + 1, rz + 1)) * 0.25f * OverMaxLight;

                                            AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                                            AddCubeVertexDataPZ(dx, ry, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, m_VertexBuffer);
                                        }

                                        if (world.IsBlockTransparent(rx, ry, rz - 1))
                                        {
                                            float lb = (world.GetFinalLightLevel(rx, ry, rz - 1) + world.GetFinalLightLevel(rx, ry - 1, rz - 1) + world.GetFinalLightLevel(rx - 1, ry, rz - 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rb = (world.GetFinalLightLevel(rx, ry, rz - 1) + world.GetFinalLightLevel(rx, ry - 1, rz - 1) + world.GetFinalLightLevel(rx + 1, ry, rz - 1) + world.GetFinalLightLevel(rx + 1, ry - 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rt = (world.GetFinalLightLevel(rx, ry, rz - 1) + world.GetFinalLightLevel(rx, ry + 1, rz - 1) + world.GetFinalLightLevel(rx + 1, ry, rz - 1) + world.GetFinalLightLevel(rx + 1, ry + 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float lt = (world.GetFinalLightLevel(rx, ry, rz - 1) + world.GetFinalLightLevel(rx, ry + 1, rz - 1) + world.GetFinalLightLevel(rx - 1, ry, rz - 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz - 1)) * 0.25f * OverMaxLight;

                                            AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                                            AddCubeVertexDataNZ(dx, ry, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, m_VertexBuffer);
                                        }
                                    }
                                    break;
                                case BlockVertexType.PerpendicularQuads:
                                    {
                                        float light = GetFinalLightLevelPrivate(dx, ry, dz) * OverMaxLight;

                                        AddPerpendicularQuadsTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                                        AddPerpendicularQuadsVertexDataFirstQuad(dx, ry, dz, light, block, m_VertexBuffer);

                                        AddPerpendicularQuadsTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                                        AddPerpendicularQuadsVertexDataSecondQuad(dx, ry, dz, light, block, m_VertexBuffer);
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

        private void BuildLiquidMesh()
        {
            WorldManager world = WorldManager.Active;
            Profiler.BeginSample("Build Liquid Mesh");

            try
            {
                for (int dx = 0; dx < ChunkWidth; dx++)
                {
                    for (int dz = 0; dz < ChunkWidth; dz++)
                    {
                        int rx = PositionX + dx;
                        int rz = PositionZ + dz;

                        int height = GetHighestNonAirYPrivate(dx, dz) + 1;

                        for (int ry = 1; ry < height; ry++)
                        {
                            BlockType type = GetBlockTypePrivateUnchecked(dx, ry, dz);
                            Block block = world.DataManager.GetBlockByType(type);

                            if (block.VertexType != BlockVertexType.Cube || !block.HasAllFlags(BlockFlags.Liquid))
                                continue;

                            float light = GetFinalLightLevelPrivate(dx, ry, dz) * OverMaxLight;

                            if (world.IsBlockTransparentAndNotWater(rx + 1, ry, rz))
                            {
                                AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                                AddCubeVertexDataPX(dx, ry, dz, light, light, light, light, block, m_VertexBuffer);
                            }

                            if (world.IsBlockTransparentAndNotWater(rx - 1, ry, rz))
                            {
                                AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                                AddCubeVertexDataNX(dx, ry, dz, light, light, light, light, block, m_VertexBuffer);
                            }

                            //if (world.IsBlockTransparentAndNotWater(rx, ry + 1, rz))
                            if (world.GetBlockType(rx, ry + 1, rz) != BlockType.Water)
                            {
                                AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                                AddCubeVertexDataPY(dx, ry, dz, light, light, light, light, block, m_VertexBuffer);
                            }

                            if (world.IsBlockTransparentAndNotWater(rx, ry - 1, rz))
                            {
                                AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                                AddCubeVertexDataNY(dx, ry, dz, light, light, light, light, block, m_VertexBuffer);
                            }

                            if (world.IsBlockTransparentAndNotWater(rx, ry, rz + 1))
                            {
                                AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                                AddCubeVertexDataPZ(dx, ry, dz, light, light, light, light, block, m_VertexBuffer);
                            }

                            if (world.IsBlockTransparentAndNotWater(rx, ry, rz - 1))
                            {
                                AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                                AddCubeVertexDataNZ(dx, ry, dz, light, light, light, light, block, m_VertexBuffer);
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
            int height = m_HeightMap[(localX << 4) | localZ];

            for (int y = height + 1; y < WorldHeight; y++)
            {
                m_SkyLights[(localX << 12) | (y << 4) | localZ] = SkyLight;
            }

            int light = SkyLight;
            int opacity = 0;

            do
            {
                BlockType type = GetBlockTypePrivateUnchecked(localX, height, localZ);
                Block block = WorldManager.Active.DataManager.GetBlockByType(type);

                light = Mathf.Clamp(light - opacity, 0, MaxNonAirBlockSkyLightValue);
                m_SkyLights[(localX << 12) | (height << 4) | localZ] = (byte)light;

                opacity = block.LightOpacity;

            } while (--height > -1);
        }

        internal void GetRawBlockData(out byte[] blocks, out byte[] states)
        {
            blocks = m_Blocks;
            states = m_BlockStates;
        }

        public void OnSaved()
        {
            IsModified = false;
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