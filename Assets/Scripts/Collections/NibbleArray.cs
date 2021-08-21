using System;
using System.Collections;
using System.Collections.Generic;
using Minecraft.Lua;

namespace Minecraft.Collections
{
    /// <summary>
    /// 元素大小为4位的数组
    /// </summary>
    public class NibbleArray : IReadOnlyList<byte>, IEnumerable<byte>, ILuaCallCSharp
    {
        private readonly byte[] m_Data;

        public int Length => m_Data.Length << 1;

        public byte this[int index]
        {
            // index 为偶数，保存在后 4 位；index 为奇数保存在前 4 位
            // 下面这个是闲着无聊写得...
            get => (byte)((m_Data[index >> 1] >> ((index & 1) << 2)) & 15);
            set => m_Data[index >> 1] = (byte)((m_Data[index >> 1] & (15 << ((~index & 1) << 2))) | ((value & 15) << ((index & 1) << 2)));
        }

        public NibbleArray(int length)
        {
            if ((length & 1) == 1)
            {
                length++;
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
                yield return (byte)(m_Data[i] & 15);// 偶数 index
                yield return (byte)((m_Data[i] >> 4) & 15);// 奇数 index
            }
        }


        int IReadOnlyCollection<byte>.Count => Length;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}