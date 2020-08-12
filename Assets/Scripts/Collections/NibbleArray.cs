using System;
using System.Collections;
using System.Collections.Generic;

namespace Minecraft.Collections
{
    /// <summary>
    /// 元素大小为4位的数组
    /// </summary>
    public class NibbleArray : IReadOnlyList<byte>, IEnumerable<byte>
    {
        private readonly byte[] m_Data;

        public int Length => m_Data.Length << 1;

        public byte this[int index]
        {
            //index为偶数，保存在后4位；index为奇数保存在前4位
            get => (byte)((m_Data[index >> 1] >> ((index & 1) << 2)) & 15);
            set => m_Data[index >> 1] = (byte)((m_Data[index >> 1] & (15 << ((~index & 1) << 2))) | ((value & 15) << ((index & 1) << 2)));
        }

        /// <summary>
        /// 创建一个<see cref="NibbleArray"/>的实例
        /// </summary>
        /// <param name="length">数组长度，必须为偶数</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/>为负数</exception>
        /// <exception cref="ArgumentException"><paramref name="length"/>为奇数</exception>
        public NibbleArray(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), nameof(NibbleArray) + "的长度必须为非负数");
            }

            if ((length & 1) == 1)
            {
                throw new ArgumentException(nameof(NibbleArray) + "的长度必须为偶数", nameof(length));
            }

            m_Data = new byte[length >> 1];
        }

        public void Clear()
        {
            Array.Clear(m_Data, 0, m_Data.Length);
        }

        public IEnumerator<byte> GetEnumerator()
        {
            for (int i = 0; i < m_Data.Length; i++)
            {
                yield return (byte)(m_Data[i] & 15);//偶数index
                yield return (byte)((m_Data[i] >> 4) & 15);//奇数index
            }
        }

        int IReadOnlyCollection<byte>.Count => Length;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}