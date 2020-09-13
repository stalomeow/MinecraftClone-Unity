using Minecraft.BlocksData;
using Minecraft.Collections;
using System;
using System.Threading;
using static Minecraft.WorldConsts;

namespace Minecraft
{
    /// <summary>
    /// 表示一份线程安全的chunk数据
    /// </summary>
    public sealed class ChunkData : IDisposable
    {
        private readonly byte[] m_Blocks; // 所有方块信息
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

        public void GetRawBlockData(out byte[] blocks, out byte[] states)
        {
            blocks = m_Blocks;
            states = m_BlockStates;
        }


        public BlockType GetBlockType(int localX, int y, int localZ)
        {
            m_ReadWriteLock.EnterReadLock();

            try
            {
                return (BlockType)m_Blocks[(localX << 12) | (y << 4) | localZ];
            }
            finally
            {
                m_ReadWriteLock.ExitReadLock();
            }
        }

        public void SetBlockType(int localX, int y, int localZ, BlockType value)
        {
            m_ReadWriteLock.EnterWriteLock();

            try
            {
                m_Blocks[(localX << 12) | (y << 4) | localZ] = (byte)value;
            }
            finally
            {
                m_ReadWriteLock.ExitWriteLock();
            }
        }

        public bool SetBlockType(int localX, int y, int localZ, BlockType value, out BlockType previousValue)
        {
            int index = (localX << 12) | (y << 4) | localZ;
            byte v = (byte)value;

            m_ReadWriteLock.EnterUpgradeableReadLock();

            try
            {
                byte p = m_Blocks[index];
                previousValue = (BlockType)p;

                if (p != v)
                {
                    m_ReadWriteLock.EnterWriteLock();

                    try
                    {
                        m_Blocks[index] = v;
                    }
                    finally
                    {
                        m_ReadWriteLock.ExitWriteLock();
                    }

                    return true;
                }

                return false;
            }
            finally
            {
                m_ReadWriteLock.ExitUpgradeableReadLock();
            }
        }

        public byte GetBlockState(int localX, int y, int localZ)
        {
            m_ReadWriteLock.EnterReadLock();

            try
            {
                return m_BlockStates[(localX << 12) | (y << 4) | localZ];
            }
            finally
            {
                m_ReadWriteLock.ExitReadLock();
            }
        }

        public bool SetBlockState(int localX, int y, int localZ, byte value)
        {
            int index = (localX << 12) | (y << 4) | localZ;

            m_ReadWriteLock.EnterUpgradeableReadLock();

            try
            {
                if (m_BlockStates[index] != value)
                {
                    m_ReadWriteLock.EnterWriteLock();

                    try
                    {
                        m_BlockStates[index] = value;
                    }
                    finally
                    {
                        m_ReadWriteLock.ExitWriteLock();
                    }

                    return true;
                }
                return false;
            }
            finally
            {
                m_ReadWriteLock.ExitUpgradeableReadLock();
            }
        }

        public byte GetSkyLight(int localX, int y, int localZ)
        {
            m_ReadWriteLock.EnterReadLock();

            try
            {
                return m_SkyLights[(localX << 12) | (y << 4) | localZ];
            }
            finally
            {
                m_ReadWriteLock.ExitReadLock();
            }
        }

        public void SetSkyLight(int localX, int y, int localZ, byte value)
        {
            m_ReadWriteLock.EnterWriteLock();

            try
            {
                m_SkyLights[(localX << 12) | (y << 4) | localZ] = value;
            }
            finally
            {
                m_ReadWriteLock.ExitWriteLock();
            }
        }

        public byte GetBlockLight(int localX, int y, int localZ)
        {
            m_ReadWriteLock.EnterReadLock();

            try
            {
                return m_BlockLights[(localX << 12) | (y << 4) | localZ];
            }
            finally
            {
                m_ReadWriteLock.ExitReadLock();
            }
        }

        public void SetBlockLight(int localX, int y, int localZ, byte value)
        {
            m_ReadWriteLock.EnterWriteLock();

            try
            {
                m_BlockLights[(localX << 12) | (y << 4) | localZ] = value;
            }
            finally
            {
                m_ReadWriteLock.ExitWriteLock();
            }
        }



        public byte GetTopNonAirIndex(int localX, int localZ)
        {
            m_ReadWriteLock.EnterReadLock();

            try
            {
                return m_HeightMap[(localX << 4) | localZ];
            }
            finally
            {
                m_ReadWriteLock.ExitReadLock();
            }
        }

        public void SetTopNonAirIndex(int localX, int localZ, byte value)
        {
            m_ReadWriteLock.EnterWriteLock();

            try
            {
                m_HeightMap[(localX << 4) | localZ] = value;
            }
            finally
            {
                m_ReadWriteLock.ExitWriteLock();
            }
        }


        public int GetTickRefCount(int sectionIndex)
        {
            m_ReadWriteLock.EnterReadLock();

            try
            {
                return m_TickRefCounts[sectionIndex];
            }
            finally
            {
                m_ReadWriteLock.ExitReadLock();
            }
        }

        public void IncreaseTickRefCount(int sectionIndex)
        {
            m_ReadWriteLock.EnterWriteLock();

            try
            {
                checked
                {
                    m_TickRefCounts[sectionIndex]++;
                }
            }
            finally
            {
                m_ReadWriteLock.ExitWriteLock();
            }
        }

        public void DecreaseTickRefCount(int sectionIndex)
        {
            m_ReadWriteLock.EnterWriteLock();

            try
            {
                checked
                {
                    m_TickRefCounts[sectionIndex]--;
                }
            }
            finally
            {
                m_ReadWriteLock.ExitWriteLock();
            }
        }

        public int GetRenderableSolidCount(int sectionIndex)
        {
            m_ReadWriteLock.EnterReadLock();

            try
            {
                return (int)(m_RenderableCounts[sectionIndex] & 0xFFFF);
            }
            finally
            {
                m_ReadWriteLock.ExitReadLock();
            }
        }

        public void IncreaseRenderableSolidCount(int sectionIndex)
        {
            m_ReadWriteLock.EnterWriteLock();

            try
            {
                checked
                {
                    m_RenderableCounts[sectionIndex]++;
                }
            }
            finally
            {
                m_ReadWriteLock.ExitWriteLock();
            }
        }

        public void DecreaseRenderableSolidCount(int sectionIndex)
        {
            m_ReadWriteLock.EnterWriteLock();

            try
            {
                checked
                {
                    m_RenderableCounts[sectionIndex]--;
                }
            }
            finally
            {
                m_ReadWriteLock.ExitWriteLock();
            }
        }

        public int GetRenderableLiquidCount(int sectionIndex)
        {
            m_ReadWriteLock.EnterReadLock();

            try
            {
                return (int)((m_RenderableCounts[sectionIndex] >> 16) & 0xFFFF);
            }
            finally
            {
                m_ReadWriteLock.ExitReadLock();
            }
        }

        public void IncreaseRenderableLiquidCount(int sectionIndex)
        {
            m_ReadWriteLock.EnterWriteLock();

            try
            {
                checked
                {
                    m_RenderableCounts[sectionIndex] += 0x10000;
                }
            }
            finally
            {
                m_ReadWriteLock.ExitWriteLock();
            }
        }

        public void DecreaseRenderableLiquidCount(int sectionIndex)
        {
            m_ReadWriteLock.EnterWriteLock();

            try
            {
                checked
                {
                    m_RenderableCounts[sectionIndex] -= 0x10000;
                }
            }
            finally
            {
                m_ReadWriteLock.ExitWriteLock();
            }
        }
    }
}