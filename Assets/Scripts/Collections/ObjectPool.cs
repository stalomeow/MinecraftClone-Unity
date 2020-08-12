using System;

namespace Minecraft.Collections
{
    /// <summary>
    /// 表示一个对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> where T : IReusableObject, new()
    {
        private const int MinGrow = 4;

        private T[] m_Pool;
        private int m_MaxObjCount;
        private int m_Size;

        public int MaxObjectCount => m_MaxObjCount;

        public int ObjectCount => m_Size;

        public ObjectPool(int maxObjectCount) : this(0, maxObjectCount) { }

        public ObjectPool(int capacity, int maxObjectCount)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            m_Pool = capacity == 0 ? Array.Empty<T>() : new T[capacity];
            m_MaxObjCount = maxObjectCount >= capacity ? maxObjectCount : throw new ArgumentOutOfRangeException(nameof(maxObjectCount));
            m_Size = 0;
        }

        public T Allocate()
        {
            if (m_Size == 0)
            {
                T newObj = new T();
                newObj.OnAllocated();
                return newObj;
            }

            int i = --m_Size;
            T result = m_Pool[i];
            result.OnAllocated();
            m_Pool[i] = default;
            return result;
        }

        public void Free(T obj)
        {
            obj.OnFree(m_Size == m_MaxObjCount);

            if (m_Size < m_MaxObjCount)
            {
                if (m_Size == m_Pool.Length)
                {
                    Grow(MinGrow);
                }

                m_Pool[m_Size++] = obj;
            }
        }

        public void Fill(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (m_Size + count > m_Pool.Length)
            {
                Grow(count);
            }

            while (count-- > 0 && m_Size < m_MaxObjCount)
            {
                m_Pool[m_Size++] = new T();
            }
        }

        public void Clear()
        {
            for (int i = 0; i < m_Size; i++)
            {
                m_Pool[i].OnFree(true);
                m_Pool[i] = default;
            }
        }

        private void Grow(int grow)
        {
            int newCapacity = m_Pool.Length << 1;

            if (newCapacity < m_Pool.Length + grow)
            {
                newCapacity = m_Pool.Length + grow;
            }

            if (newCapacity > m_MaxObjCount)
            {
                newCapacity = m_MaxObjCount;
            }

            T[] array = new T[newCapacity];
            Array.Copy(m_Pool, 0, array, 0, m_Size);
            m_Pool = array;
        }
    }
}