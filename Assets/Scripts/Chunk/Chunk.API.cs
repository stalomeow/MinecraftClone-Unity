using Minecraft.BlocksData;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Minecraft.WorldConsts;

namespace Minecraft
{
    public sealed partial class Chunk
    {
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

            BlockType type = GetBlockTypePrivateUnchecked(localX, y, localZ);
            Block block = WorldManager.Active.DataManager.GetBlockByType(type);

            // 液体的mesh不计算环境光照
            MeshDirtyFlags flags = block.HasAnyFlag(BlockFlags.Liquid) ? MeshDirtyFlags.Both : MeshDirtyFlags.SolidMesh;
            int sectionIndex = Mathf.FloorToInt(y * OverChunkWidth);

            lock (m_SyncLock)
            {
                m_BlockLights[(localX << 12) | (y << 4) | localZ] = value;
                SetMeshDirtyWithoutLock(flags, sectionIndex);
            }
        }

        public BlockType GetBlockType(int worldX, int y, int worldZ)
        {
            int index = ((worldX - PositionX) << 12) | (y << 4) | (worldZ - PositionZ);
            return index < 0 || index >= m_Blocks.Length ? BlockType.Air : (BlockType)m_Blocks[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private BlockType GetBlockTypePrivateUnchecked(int localX, int y, int localZ) => (BlockType)m_Blocks[(localX << 12) | (y << 4) | localZ];

        // return: 是否设置成功
        public bool SetBlockType(int worldX, int y, int worldZ, BlockType value, byte state = 0, bool lightBlocks = true, bool tickBlocks = true, bool updateNeighborSections = true)
        {
            int localX = worldX - PositionX;
            int localZ = worldZ - PositionZ;

            int index = (localX << 12) | (y << 4) | localZ;

            if (index < 0 || index >= m_Blocks.Length)
                return false;

            BlockType previousBlockType = (BlockType)m_Blocks[index];

            if (previousBlockType == value)
                return false;

            WorldManager world = WorldManager.Active;
            ChunkManager manager = world.ChunkManager;
            DataManager dataManager = world.DataManager;

            Block previousBlock = dataManager.GetBlockByType(previousBlockType);
            Block block = dataManager.GetBlockByType(value);

            int sectionIndex = Mathf.FloorToInt(y * OverSectionHeight);
            int yInSection = y - sectionIndex * SectionHeight;

            bool flag1 = previousBlock.HasAnyFlag(BlockFlags.Liquid);
            bool flag2 = block.HasAnyFlag(BlockFlags.Liquid);

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

                if (updateNeighborSections)
                {
                    if (yInSection == 0 && sectionIndex > 0)
                    {
                        SetMeshDirtyWithoutLock(MeshDirtyFlags.Both, sectionIndex - 1);
                    }
                    else if (yInSection == SectionHeight - 1 && sectionIndex < SectionCountInChunk - 1)
                    {
                        SetMeshDirtyWithoutLock(MeshDirtyFlags.Both, sectionIndex + 1);
                    }
                }

                MeshDirtyFlags dirtyFlags;

                if (flag1 != flag2)
                {
                    dirtyFlags = MeshDirtyFlags.Both;
                }
                else
                {
                    dirtyFlags = flag2 ? MeshDirtyFlags.LiquidMesh : MeshDirtyFlags.SolidMesh;
                }

                SetMeshDirtyWithoutLock(dirtyFlags, sectionIndex);
            }

            if (previousBlock.HasAnyFlag(BlockFlags.NeedsRandomTick))
            {
                m_TickRefCounts[sectionIndex]--;
            }

            if (block.HasAnyFlag(BlockFlags.NeedsRandomTick))
            {
                m_TickRefCounts[sectionIndex]++;
            }

            if (previousBlock.VertexType != BlockVertexType.None)
            {
                if (previousBlock.HasAnyFlag(BlockFlags.Liquid))
                {
                    m_LiquidCounts[sectionIndex]--;
                }
                else
                {
                    m_SolidCounts[sectionIndex]--;
                }
            }

            if (block.VertexType != BlockVertexType.None)
            {
                if (block.HasAnyFlag(BlockFlags.Liquid))
                {
                    m_LiquidCounts[sectionIndex]++;
                }
                else
                {
                    m_SolidCounts[sectionIndex]++;
                }
            }

            if (tickBlocks)
            {
                manager.TickBlock(worldX, y, worldZ);
            }

            if (lightBlocks)
            {
                manager.LightBlock(worldX, y, worldZ);
            }

            if (updateNeighborSections)
            {
                if (localX == 0)
                {
                    Chunk chunk = manager.GetChunk(PositionX - ChunkWidth, PositionZ);
                    chunk?.SetMeshDirty(MeshDirtyFlags.Both, sectionIndex);
                }
                else if (localX == ChunkWidth - 1)
                {
                    Chunk chunk = manager.GetChunk(PositionX + ChunkWidth, PositionZ);
                    chunk?.SetMeshDirty(MeshDirtyFlags.Both, sectionIndex);
                }

                if (localZ == 0)
                {
                    Chunk chunk = manager.GetChunk(PositionX, PositionZ - ChunkWidth);
                    chunk?.SetMeshDirty(MeshDirtyFlags.Both, sectionIndex);
                }
                else if (localZ == ChunkWidth - 1)
                {
                    Chunk chunk = manager.GetChunk(PositionX, PositionZ + ChunkWidth);
                    chunk?.SetMeshDirty(MeshDirtyFlags.Both, sectionIndex);
                }
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

        private void SetMeshDirty(MeshDirtyFlags flags, int dirtySectionIndex)
        {
            lock (m_SyncLock)
            {
                m_MeshDirtyFlags |= flags;
                m_DirtyMeshIndexes |= (ushort)(1 << dirtySectionIndex);
            }
        }

        private void SetMeshDirtyWithoutLock(MeshDirtyFlags flags, int dirtySectionIndex)
        {
            m_MeshDirtyFlags |= flags;
            m_DirtyMeshIndexes |= (ushort)(1 << dirtySectionIndex);
        }
    }
}