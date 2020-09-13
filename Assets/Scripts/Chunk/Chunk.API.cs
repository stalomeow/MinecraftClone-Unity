using Minecraft.BlocksData;
using UnityEngine;
using static Minecraft.WorldConsts;

namespace Minecraft
{
    public sealed partial class Chunk
    {
        public int GetTopNonAirIndex(int worldX, int worldZ)
        {
            return m_Data.GetTopNonAirIndex(worldX - PositionX, worldZ - PositionZ);
        }

        public byte GetBlockLight(int worldX, int y, int worldZ)
        {
            int localX = worldX - PositionX;
            int localZ = worldZ - PositionZ;

            return (y >= WorldHeight || y < 0) ? (byte)0 : m_Data.GetBlockLight(localX, y, localZ);
        }

        public void SetBlockLight(int worldX, int y, int worldZ, byte value)
        {
            if (y >= WorldHeight || y < 0)
                return;

            int localX = worldX - PositionX;
            int localZ = worldZ - PositionZ;
            int sectionIndex = Mathf.FloorToInt(y * OverChunkWidth);

            m_Data.SetBlockLight(localX, y, localZ, value);

            SetMeshDirty(sectionIndex, MeshDirtyFlags.Both);
        }

        public BlockType GetBlockType(int worldX, int y, int worldZ)
        {
            if (y >= WorldHeight || y < 0)
            {
                return BlockType.Air;
            }

            int localX = worldX - PositionX;
            int localZ = worldZ - PositionZ;

            return m_Data.GetBlockType(localX, y, localZ);
        }

        public bool SetBlockType(int worldX, int y, int worldZ, BlockType value, byte state = 0, bool lightBlocks = true, bool tickBlocks = true, bool updateNeighborSections = true)
        {
            if (y >= WorldHeight || y < 0)
            {
                return false;
            }

            int localX = worldX - PositionX;
            int localZ = worldZ - PositionZ;

            if (!m_Data.SetBlockType(localX, y, localZ, value, out BlockType previousBlockType))
            {
                return false;
            }

            m_Data.SetBlockState(localX, y, localZ, state);
            UpdateHeightMapAndSkyLight(localX, y, localZ);


            WorldManager world = WorldManager.Active;
            ChunkManager manager = world.ChunkManager;
            DataManager dataManager = world.DataManager;

            Block previousBlock = dataManager.GetBlockByType(previousBlockType);
            Block block = dataManager.GetBlockByType(value);

            int sectionIndex = Mathf.FloorToInt(y * OverSectionHeight);
            int yInSection = y - sectionIndex * SectionHeight;

            if (previousBlock.HasAnyFlag(BlockFlags.NeedsRandomTick))
            {
                m_Data.DecreaseTickRefCount(sectionIndex);
            }

            if (block.HasAnyFlag(BlockFlags.NeedsRandomTick))
            {
                m_Data.IncreaseTickRefCount(sectionIndex);
            }

            if (previousBlock.VertexType != BlockVertexType.None)
            {
                if (previousBlock.HasAnyFlag(BlockFlags.Liquid))
                {
                    m_Data.DecreaseRenderableLiquidCount(sectionIndex);
                }
                else
                {
                    m_Data.DecreaseRenderableSolidCount(sectionIndex);
                }
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

            SetMeshDirty(sectionIndex, GetDirtyFlags(previousBlock, block));

            if (lightBlocks)
            {
                manager.LightBlock(worldX, y, worldZ);
            }

            if (tickBlocks)
            {
                manager.TickBlock(worldX, y, worldZ);
            }            

            if (updateNeighborSections)
            {
                if (localX == 0)
                {
                    Chunk chunk = manager.GetChunk(PositionX - ChunkWidth, PositionZ);
                    chunk?.SetMeshDirty(sectionIndex, MeshDirtyFlags.Both);
                }
                else if (localX == ChunkWidth - 1)
                {
                    Chunk chunk = manager.GetChunk(PositionX + ChunkWidth, PositionZ);
                    chunk?.SetMeshDirty(sectionIndex, MeshDirtyFlags.Both);
                }

                if (yInSection == 0 && sectionIndex > 0)
                {
                    SetMeshDirty(sectionIndex - 1, MeshDirtyFlags.Both);
                }
                else if (yInSection == SectionHeight - 1 && sectionIndex < SectionCountInChunk - 1)
                {
                    SetMeshDirty(sectionIndex + 1, MeshDirtyFlags.Both);
                }

                if (localZ == 0)
                {
                    Chunk chunk = manager.GetChunk(PositionX, PositionZ - ChunkWidth);
                    chunk?.SetMeshDirty(sectionIndex, MeshDirtyFlags.Both);
                }
                else if (localZ == ChunkWidth - 1)
                {
                    Chunk chunk = manager.GetChunk(PositionX, PositionZ + ChunkWidth);
                    chunk?.SetMeshDirty(sectionIndex, MeshDirtyFlags.Both);
                }
            }

            return m_IsModified = true;
        }

        private MeshDirtyFlags GetDirtyFlags(Block previous, Block current)
        {
            bool flag1 = previous.HasAnyFlag(BlockFlags.Liquid);
            bool flag2 = current.HasAnyFlag(BlockFlags.Liquid);

            if (flag1 != flag2)
                return MeshDirtyFlags.Both;

            return flag2 ? MeshDirtyFlags.LiquidMesh : MeshDirtyFlags.SolidMesh;
        }

        private void UpdateHeightMapAndSkyLight(int localX, int y, int localZ)
        {
            int height = m_Data.GetTopNonAirIndex(localX, localZ);

            if (y >= height)
            {
                for (int i = y; i > -1; i--)
                {
                    if (m_Data.GetBlockType(localX, i, localZ) != BlockType.Air)
                    {
                        m_Data.SetTopNonAirIndex(localX, localZ, (byte)i); // 至少会有一个非空方块，比如基岩
                        height = i;
                        break;
                    }
                }
            }

            UpdateSkyLightData(localX, localZ, height);
        }

        public byte GetBlockState(int worldX, int y, int worldZ)
        {
            if (y >= WorldHeight || y < 0)
                return default;

            int localX = worldX - PositionX;
            int localZ = worldZ - PositionZ;

            return m_Data.GetBlockState(localX, y, localZ);
        }

        public void SetBlockState(int worldX, int y, int worldZ, byte value)
        {
            if (y >= WorldHeight || y < 0)
                return;

            int localX = worldX - PositionX;
            int localZ = worldZ - PositionZ;

            if (m_Data.SetBlockState(localX, y, localZ, value))
            {
                m_IsModified = true;
            }
        }

        public byte GetFinalLightLevel(int worldX, int y, int worldZ)
        {
            return GetFinalLightLevelPrivate(worldX - PositionX, y, worldZ - PositionZ);
        }

        private byte GetFinalLightLevelPrivate(int localX, int y, int localZ)
        {
            if (y >= WorldHeight || y < 0)
            {
                return MaxLight; // default
            }

            BlockType type = m_Data.GetBlockType(localX, y, localZ);
            Block block = WorldManager.Active.DataManager.GetBlockByType(type);

            byte skyLight = (byte)Mathf.Clamp(m_Data.GetSkyLight(localX, y, localZ) - SkyLightSubtracted, 0, MaxLight);
            byte blockLight = m_Data.GetBlockLight(localX, y, localZ);
            byte light = block.LightValue;

            // MAX(skyLight, blockLight, emission)

            if (skyLight > light)
                light = skyLight;

            if (blockLight > light)
                light = blockLight;

            return light;
        }
    }
}