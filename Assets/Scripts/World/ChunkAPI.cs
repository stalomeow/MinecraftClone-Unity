using Minecraft.Rendering;
using UnityEngine;
using static Minecraft.WorldConsts;

namespace Minecraft
{
    public sealed partial class Chunk
    {
        public void LocalToWorldPosition(ref int x, ref int z)
        {
            x += PositionX;
            z += PositionZ;
        }

        public void WorldToLocalPosition(ref int x, ref int z)
        {
            x -= PositionX;
            z -= PositionZ;
        }

        public int GetTopNonAirBlockIndex(int localX, int localZ)
        {
            return m_Data.GetTopNonAirBlockIndex(localX, localZ);
        }

        public byte GetBlockLight(int localX, int y, int localZ)
        {
            return (y >= WorldHeight || y < 0) ? (byte)0 : m_Data.GetBlockLight(localX, y, localZ);
        }

        public void SetBlockLight(int localX, int y, int localZ, byte value)
        {
            if (y >= WorldHeight || y < 0)
                return;

            m_Data.SetBlockLight(localX, y, localZ, value);

            int sectionIndex = Mathf.FloorToInt(y * OverChunkWidth);
            InvokeGeometryChangedEvent(sectionIndex);
        }

        public byte GetBlockId(int localX, int y, int localZ)
        {
            if (y >= WorldHeight || y < 0)
            {
                return Block.AirId;
            }

            return m_Data.GetBlockId(localX, y, localZ);
        }

        public bool SetBlockId(int localX, int y, int localZ, byte value, byte state = 0, bool lightBlocks = true, bool tickBlocks = true, bool updateNeighborSections = true)
        {
            if (y >= WorldHeight || y < 0)
            {
                return false;
            }

            if (!m_Data.SetBlockId(localX, y, localZ, value, out byte previousBlockId))
            {
                return false;
            }

            m_Data.SetBlockState(localX, y, localZ, state);
            UpdateHeightMapAndSkyLight(localX, y, localZ);

            Block previousBlock = World.GetBlock(previousBlockId);
            Block block = World.GetBlock(value);

            int sectionIndex = Mathf.FloorToInt(y * OverSectionHeight);
            int yInSection = y - (sectionIndex * SectionHeight);

            if (previousBlock.HasAnyFlag(BlockFlags.NeedsRandomTick))
            {
                m_Data.DecreaseTickRefCount(sectionIndex);
            }

            if (block.HasAnyFlag(BlockFlags.NeedsRandomTick))
            {
                m_Data.IncreaseTickRefCount(sectionIndex);
            }

            if (!previousBlock.MeshWriter.IsEmpty)
            {
                m_Data.DecreaseVisibleBlockCount(sectionIndex, block);
            }

            if (!block.MeshWriter.IsEmpty)
            {
                m_Data.IncreaseVisibleBlockCount(sectionIndex, block);
            }

            if (lightBlocks)
            {
                World.LightBlock(localX + PositionX, y, localZ + PositionZ);
            }

            if (tickBlocks)
            {
                World.TickBlock(localX + PositionX, y, localZ + PositionZ);
            }

            if (updateNeighborSections)
            {
                if (localX == 0)
                {
                    Chunk chunk = World.GetChunk(PositionX - ChunkWidth, PositionZ);
                    chunk?.InvokeGeometryChangedEvent(sectionIndex);
                }
                else if (localX == ChunkWidth - 1)
                {
                    Chunk chunk = World.GetChunk(PositionX + ChunkWidth, PositionZ);
                    chunk?.InvokeGeometryChangedEvent(sectionIndex);
                }

                if (yInSection == 0 && sectionIndex > 0)
                {
                    InvokeGeometryChangedEvent(sectionIndex - 1);
                }
                else if (yInSection == SectionHeight - 1 && sectionIndex < SectionCountInChunk - 1)
                {
                    InvokeGeometryChangedEvent(sectionIndex + 1);
                }

                if (localZ == 0)
                {
                    Chunk chunk = World.GetChunk(PositionX, PositionZ - ChunkWidth);
                    chunk?.InvokeGeometryChangedEvent(sectionIndex);
                }
                else if (localZ == ChunkWidth - 1)
                {
                    Chunk chunk = World.GetChunk(PositionX, PositionZ + ChunkWidth);
                    chunk?.InvokeGeometryChangedEvent(sectionIndex);
                }
            }

            InvokeGeometryChangedEvent(sectionIndex);
            return true;
        }

        private void UpdateHeightMapAndSkyLight(int localX, int y, int localZ)
        {
            int height = m_Data.GetTopNonAirBlockIndex(localX, localZ);

            if (y >= height)
            {
                for (int i = y; i > -1; i--)
                {
                    if (m_Data.GetBlockId(localX, i, localZ) != Block.AirId)
                    {
                        m_Data.SetTopNonAirIndex(localX, localZ, (byte)i); // 至少会有一个非空方块，比如基岩
                        height = i;
                        break;
                    }
                }
            }

            UpdateSkyLightData(localX, localZ, height);
        }

        public byte GetBlockState(int localX, int y, int localZ)
        {
            if (y >= WorldHeight || y < 0)
                return default;

            return m_Data.GetBlockState(localX, y, localZ);
        }

        public void SetBlockState(int localX, int y, int localZ, byte value, bool updateMesh = false)
        {
            if (y >= WorldHeight || y < 0)
                return;

            if (m_Data.SetBlockState(localX, y, localZ, value) && updateMesh)
            {
                int sectionIndex = Mathf.FloorToInt(y * OverChunkWidth);
                InvokeGeometryChangedEvent(sectionIndex);
            }
        }

        public byte GetLightValue(int localX, int y, int localZ)
        {
            if (y >= WorldHeight || y < 0)
            {
                return LightingUtility.SkyLight; // default
            }

            byte id = m_Data.GetBlockId(localX, y, localZ);
            Block block = World.GetBlock(id);

            byte skyLight = (byte)LightingUtility.ClampSkyLight(m_Data.GetSkyLight(localX, y, localZ));
            byte blockLight = m_Data.GetBlockLight(localX, y, localZ);
            byte light = block.LightValue;

            if (skyLight > light) light = skyLight;
            if (blockLight > light) light = blockLight;

            return light; // MAX(skyLight, blockLight, emission)
        }

        public bool IsSectionVisible(int sectionIndex, MeshSliceType sliceType)
        {
            switch (sliceType)
            {
                case MeshSliceType.Solid: return m_Data.GetVisibleSolidCount(sectionIndex) > 0;
                case MeshSliceType.Fluid: return m_Data.GetVisibleFluidCount(sectionIndex) > 0;
                default: return false;
            }
        }
    }
}
