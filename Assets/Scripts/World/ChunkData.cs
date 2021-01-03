using Minecraft.Collections;
using Minecraft.Rendering;
using Minecraft.Serialization;
using System;
using System.IO;
using System.Threading;
using static Minecraft.WorldConsts;

namespace Minecraft
{
    /// <summary>
    /// 表示一份线程安全的chunk数据
    /// </summary>
    public sealed class ChunkData : IBinarySerializable, IDisposable
    {
        private readonly byte[] m_Blocks; // 所有方块id
        private readonly byte[] m_BlockStates; // 所有方块的状态
        private readonly NibbleArray m_SkyLights; // 每一个方块受到的天空光照值
        private readonly NibbleArray m_BlockLights; // 每一个方块受到的由其他方块引起的光照值

        private readonly byte[] m_HeightMap; // chunk 的高度图, 第一个非空方块的y

        private readonly ushort[] m_TickRefCounts; // 每一个section（高度16）需要tick的数量
        private readonly uint[] m_RenderableCounts; // 每一个section（高度16）可以被绘制的方块数量

        private readonly ReaderWriterLockSlim m_ReadWriteLock;


        public ChunkData()
        {
            m_Blocks = new byte[BlockCountInChunk];
            m_BlockStates = new byte[BlockCountInChunk];
            m_SkyLights = new NibbleArray(BlockCountInChunk);
            m_BlockLights = new NibbleArray(BlockCountInChunk);

            m_HeightMap = new byte[ChunkWidth * ChunkWidth];

            m_TickRefCounts = new ushort[SectionCountInChunk];
            m_RenderableCounts = new uint[SectionCountInChunk];

            m_ReadWriteLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }


        public void Free(bool dispose)
        {
            if (dispose)
            {
                Dispose();
            }
            else
            {
                Clear();
            }
        }

        public void Dispose()
        {
            m_ReadWriteLock.Dispose();
        }

        public void Clear()
        {
            Array.Clear(m_Blocks, 0, m_Blocks.Length);
            Array.Clear(m_BlockStates, 0, m_BlockStates.Length);

            m_SkyLights.Clear();
            m_BlockLights.Clear();

            Array.Clear(m_HeightMap, 0, m_HeightMap.Length);

            Array.Clear(m_TickRefCounts, 0, m_TickRefCounts.Length);
            Array.Clear(m_RenderableCounts, 0, m_RenderableCounts.Length);
        }


        public byte GetBlockId(int localX, int y, int localZ)
        {
            int index = (localX << 12) | (y << 4) | localZ;

            if (index < 0 || index >= m_Blocks.Length)
                return Block.AirId;

            m_ReadWriteLock.EnterReadLock();
            byte result = m_Blocks[index];
            m_ReadWriteLock.ExitReadLock();

            return result;
        }

        public void SetBlockId(int localX, int y, int localZ, byte value)
        {
            int index = (localX << 12) | (y << 4) | localZ;

            if (index < 0 || index >= m_Blocks.Length)
                return;

            m_ReadWriteLock.EnterUpgradeableReadLock();

            if (m_Blocks[index] != value)
            {
                m_ReadWriteLock.EnterWriteLock();
                m_Blocks[index] = value;
                m_ReadWriteLock.ExitWriteLock();
            }

            m_ReadWriteLock.ExitUpgradeableReadLock();
        }

        public bool SetBlockId(int localX, int y, int localZ, byte value, out byte previousValue)
        {
            bool result = false;
            int index = (localX << 12) | (y << 4) | localZ;

            if (index < 0 || index >= m_Blocks.Length)
            {
                previousValue = Block.AirId;
            }
            else
            {
                m_ReadWriteLock.EnterUpgradeableReadLock();

                previousValue = m_Blocks[index];

                if (previousValue != value)
                {
                    m_ReadWriteLock.EnterWriteLock();
                    m_Blocks[index] = value;
                    m_ReadWriteLock.ExitWriteLock();

                    result = true;
                }

                m_ReadWriteLock.ExitUpgradeableReadLock();
            }

            return result;
        }

        public byte GetBlockState(int localX, int y, int localZ)
        {
            int index = (localX << 12) | (y << 4) | localZ;

            if (index < 0 || index >= m_BlockStates.Length)
                return default;

            m_ReadWriteLock.EnterReadLock();
            byte result = m_BlockStates[index];
            m_ReadWriteLock.ExitReadLock();

            return result;
        }

        public bool SetBlockState(int localX, int y, int localZ, byte value)
        {
            int index = (localX << 12) | (y << 4) | localZ;

            if (index < 0 || index >= m_BlockStates.Length)
                return false;

            bool result = false;

            m_ReadWriteLock.EnterUpgradeableReadLock();

            if (m_BlockStates[index] != value)
            {
                m_ReadWriteLock.EnterWriteLock();
                m_BlockStates[index] = value;
                m_ReadWriteLock.ExitWriteLock();

                result = true;
            }

            m_ReadWriteLock.ExitUpgradeableReadLock();

            return result;
        }

        public byte GetSkyLight(int localX, int y, int localZ)
        {
            int index = (localX << 12) | (y << 4) | localZ;

            if (index < 0 || index >= m_SkyLights.Length)
                return LightingUtility.SkyLight;

            m_ReadWriteLock.EnterReadLock();
            byte result = m_SkyLights[index];
            m_ReadWriteLock.ExitReadLock();

            return result;
        }

        public void SetSkyLight(int localX, int y, int localZ, byte value)
        {
            int index = (localX << 12) | (y << 4) | localZ;

            if (index < 0 || index >= m_SkyLights.Length)
                return;

            m_ReadWriteLock.EnterUpgradeableReadLock();

            if (m_SkyLights[index] != value)
            {
                m_ReadWriteLock.EnterWriteLock();
                m_SkyLights[index] = value;
                m_ReadWriteLock.ExitWriteLock();
            }

            m_ReadWriteLock.ExitUpgradeableReadLock();
        }

        public byte GetBlockLight(int localX, int y, int localZ)
        {
            int index = (localX << 12) | (y << 4) | localZ;

            if (index < 0 || index >= m_BlockLights.Length)
                return default;

            m_ReadWriteLock.EnterReadLock();
            byte result = m_BlockLights[index];
            m_ReadWriteLock.ExitReadLock();

            return result;
        }

        public void SetBlockLight(int localX, int y, int localZ, byte value)
        {
            int index = (localX << 12) | (y << 4) | localZ;

            if (index < 0 || index >= m_BlockLights.Length)
                return;

            m_ReadWriteLock.EnterUpgradeableReadLock();

            if (m_BlockLights[index] != value)
            {
                m_ReadWriteLock.EnterWriteLock();
                m_BlockLights[index] = value;
                m_ReadWriteLock.ExitWriteLock();
            }

            m_ReadWriteLock.ExitUpgradeableReadLock();
        }


        public byte GetTopNonAirBlockIndex(int localX, int localZ)
        {
            int index = (localX << 4) | localZ;

            if (index < 0 || index >= m_HeightMap.Length)
                return default;

            m_ReadWriteLock.EnterReadLock();
            byte result = m_HeightMap[index];
            m_ReadWriteLock.ExitReadLock();

            return result;
        }

        public void SetTopNonAirIndex(int localX, int localZ, byte value)
        {
            int index = (localX << 4) | localZ;

            if (index < 0 || index >= m_HeightMap.Length)
                return;

            m_ReadWriteLock.EnterUpgradeableReadLock();

            if (m_HeightMap[index] != value)
            {
                m_ReadWriteLock.EnterWriteLock();
                m_HeightMap[index] = value;
                m_ReadWriteLock.ExitWriteLock();
            }

            m_ReadWriteLock.ExitUpgradeableReadLock();
        }


        public int GetTickRefCount(int sectionIndex)
        {
            if (sectionIndex < 0 || sectionIndex >= m_TickRefCounts.Length)
                return default;

            m_ReadWriteLock.EnterReadLock();
            int result = m_TickRefCounts[sectionIndex];
            m_ReadWriteLock.ExitReadLock();

            return result;
        }

        public void IncreaseTickRefCount(int sectionIndex)
        {
            if (sectionIndex < 0 || sectionIndex >= m_TickRefCounts.Length)
                return;

            m_ReadWriteLock.EnterWriteLock();
            m_TickRefCounts[sectionIndex]++;
            m_ReadWriteLock.ExitWriteLock();
        }

        public void DecreaseTickRefCount(int sectionIndex)
        {
            if (sectionIndex < 0 || sectionIndex >= m_TickRefCounts.Length)
                return;

            m_ReadWriteLock.EnterWriteLock();
            m_TickRefCounts[sectionIndex]--;
            m_ReadWriteLock.ExitWriteLock();
        }

        public int GetVisibleSolidCount(int sectionIndex)
        {
            if (sectionIndex < 0 || sectionIndex >= m_RenderableCounts.Length)
                return default;

            m_ReadWriteLock.EnterReadLock();
            int result = (int)(m_RenderableCounts[sectionIndex] & 0xFFFFu);
            m_ReadWriteLock.ExitReadLock();

            return result;
        }

        public int GetVisibleFluidCount(int sectionIndex)
        {
            if (sectionIndex < 0 || sectionIndex >= m_RenderableCounts.Length)
                return default;

            m_ReadWriteLock.EnterReadLock();
            int result = (int)((m_RenderableCounts[sectionIndex] >> 16) & 0xFFFFu);
            m_ReadWriteLock.ExitReadLock();

            return result;
        }

        public void IncreaseVisibleBlockCount(int sectionIndex, Block block)
        {
            if (sectionIndex < 0 || sectionIndex >= m_RenderableCounts.Length)
                return;

            m_ReadWriteLock.EnterWriteLock();
            m_RenderableCounts[sectionIndex] += (block.IsFluid ? 0x10000u : 1u);
            m_ReadWriteLock.ExitWriteLock();
        }

        public void DecreaseVisibleBlockCount(int sectionIndex, Block block)
        {
            if (sectionIndex < 0 || sectionIndex >= m_RenderableCounts.Length)
                return;

            m_ReadWriteLock.EnterWriteLock();
            m_RenderableCounts[sectionIndex] -= (block.IsFluid ? 0x10000u : 1u);
            m_ReadWriteLock.ExitWriteLock();
        }


        void IBinarySerializable.Serialize(Stream stream)
        {
            m_ReadWriteLock.EnterReadLock();

            try
            {
                stream.Write(m_Blocks, 0, BlockCountInChunk);
                stream.Write(m_BlockStates, 0, BlockCountInChunk);
            }
            finally
            {
                m_ReadWriteLock.ExitReadLock();
            }
        }

        void IBinarySerializable.Deserialize(World world, Stream stream)
        {
            m_ReadWriteLock.EnterWriteLock();

            try
            {
                int count = 0;

                do
                {
                    count += stream.Read(m_Blocks, count, BlockCountInChunk);

                } while (count < BlockCountInChunk);

                count = 0;

                do
                {
                    count += stream.Read(m_BlockStates, count, BlockCountInChunk);

                } while (count < BlockCountInChunk);
            }
            finally
            {
                m_ReadWriteLock.ExitWriteLock();
            }
        }
    }
}