using System;
using System.Collections;
using System.Collections.Generic;
using Minecraft.Lua;

namespace Minecraft.Collections
{
    /// <summary>
    /// 表示根据对象优先级排列的队列
    /// </summary>
    /// <typeparam name="T">元素对象的类型</typeparam>
    public class PriorityQueue<T> : IReadOnlyCollection<T>, ILuaCallCSharp
    {
        private const int DefaultCapacity = 4;
        private const int MinGrow = 4;

        private T[] m_Array;
        private int m_Size;
        private int m_Version;
        private readonly IComparer<T> m_Comparer;

        public int Count => m_Size;

        public PriorityQueue() : this(0, Comparer<T>.Default) { }

        public PriorityQueue(Comparison<T> comparison) : this(0, Comparer<T>.Create(comparison)) { }

        public PriorityQueue(IComparer<T> comparer) : this(0, comparer) { }

        public PriorityQueue(int capacity) : this(capacity, Comparer<T>.Default) { }

        public PriorityQueue(int capacity, Comparison<T> comparison) : this(capacity, Comparer<T>.Create(comparison)) { }

        public PriorityQueue(int capacity, IComparer<T> comparer)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            m_Array = capacity == 0 ? Array.Empty<T>() : new T[capacity];
            m_Size = 0;
            m_Version = 0;
            m_Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        public PriorityQueue(IEnumerable<T> collections) : this(collections, Comparer<T>.Default) { }

        public PriorityQueue(IEnumerable<T> collections, Comparison<T> comparison) : this(collections, Comparer<T>.Create(comparison)) { }

        public PriorityQueue(IEnumerable<T> collections, IComparer<T> comparer)
        {
            if (collections == null)
                throw new ArgumentNullException(nameof(collections));

            m_Array = new T[DefaultCapacity];
            m_Size = 0;
            m_Version = 0;
            m_Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));

            foreach (T item in collections)
            {
                Enqueue(item);
            }
        }

        public void Clear()
        {
            Array.Clear(m_Array, 0, m_Size);

            m_Size = 0;
            m_Version++;
        }

        public bool Contains(T item)
        {
            return Contains(item, EqualityComparer<T>.Default);
        }

        public bool Contains(T item, IEqualityComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            for (int i = 0; i < m_Size; i++)
            {
                if (comparer.Equals(item, m_Array[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (m_Size > 0)
            {
                Array.Copy(m_Array, 0, array, arrayIndex, m_Size);
            }
        }

        public T Dequeue()
        {
            if (m_Size == 0)
                throw new InvalidOperationException("队列长度为0");

            int i = --m_Size;
            m_Version++;

            T first = m_Array[0];

            if (i == 0)
            {
                m_Array[0] = default;
                return first;
            }

            T last = m_Array[i];
            m_Array[i] = default;
            SiftDown(last, 0);
            return first;
        }

        public T Peek()
        {
            return m_Size > 0 ? m_Array[0] : throw new InvalidOperationException("队列长度为0");
        }

        public void Enqueue(T item)
        {
            if (m_Size == m_Array.Length)
            {
                Grow();
            }

            int i = m_Size++;
            m_Version++;

            if (i == 0)
            {
                m_Array[0] = item;
            }
            else
            {
                SiftUp(item, i);
            }
        }

        /// <summary>
        /// 注：无法保证元素的顺序
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            if (m_Size == 0)
            {
                return Array.Empty<T>();
            }

            T[] result = new T[m_Size];
            Array.Copy(m_Array, 0, result, 0, m_Size);
            return result;
        }

        public void TrimExcess()
        {
            int threshold = (int)(m_Array.Length * 0.9f);

            if (m_Size < threshold)
            {
                if (m_Size == 0)
                {
                    m_Array = Array.Empty<T>();
                }
                else
                {
                    T[] array = new T[m_Size];
                    Array.Copy(m_Array, 0, array, 0, m_Size);
                    m_Array = array;
                }
            }
        }

        private void SiftUp(T item, int i)
        {
            //小顶堆
            while (i > 0)
            {
                int parentIndex = (i - 1) >> 1;
                T parent = m_Array[parentIndex];

                if (m_Comparer.Compare(item, parent) >= 0)
                    break;

                m_Array[i] = parent;
                i = parentIndex;
            }
            m_Array[i] = item;
        }

        private void SiftDown(T item, int i)
        {
            int half = m_Size >> 1;

            //小顶堆
            while (i < half)
            {
                int childIndex = (i << 1) + 1;
                int rightIndex = childIndex + 1;

                T child = m_Array[childIndex];

                if ((rightIndex < m_Size) && (m_Comparer.Compare(child, m_Array[rightIndex]) >= 0))
                    child = m_Array[childIndex = rightIndex];

                if (m_Comparer.Compare(item, child) < 0)
                    break;

                m_Array[i] = child;
                i = childIndex;
            }

            m_Array[i] = item;
        }

        private void Grow()
        {
            int newCapacity = m_Array.Length << 1;

            if (newCapacity < m_Array.Length + MinGrow)
            {
                newCapacity = m_Array.Length + MinGrow;
            }

            T[] array = new T[newCapacity];
            Array.Copy(m_Array, 0, array, 0, m_Size);
            m_Array = array;
        }

        /// <summary>
        /// 获取迭代器
        /// 注：迭代无法保证元素的顺序
        /// </summary>
        /// <returns></returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }


        public struct Enumerator : IEnumerator<T>
        {
            private readonly PriorityQueue<T> m_Queue;
            private readonly int m_Version;
            private int m_Index; // -1 = not started, -2 = ended/disposed
            private T m_Current;

            public T Current
            {
                get
                {
                    switch (m_Index)
                    {
                        case -1:
                            throw new InvalidOperationException("迭代没有开始");
                        case -2:
                            throw new InvalidOperationException("迭代器已经被释放");
                        default:
                            return m_Current;
                    }
                }
            }

            internal Enumerator(PriorityQueue<T> queue)
            {
                m_Queue = queue;
                m_Version = queue.m_Version;
                m_Index = -1;
                m_Current = default;
            }

            public void Dispose()
            {
                m_Index = -2;
                m_Current = default;
            }

            public bool MoveNext()
            {
                if (m_Version != m_Queue.m_Version)
                    throw new InvalidOperationException("迭代时修改集合元素");

                if (m_Index == -2)
                    return false;

                m_Index++;

                if (m_Index == m_Queue.m_Size)
                {
                    m_Index = -2;
                    m_Current = default;
                    return false;
                }

                m_Current = m_Queue.m_Array[m_Index];
                return true;
            }

            public void Reset()
            {
                if (m_Version != m_Queue.m_Version)
                    throw new InvalidOperationException("迭代时修改集合元素");

                m_Index = -1;
                m_Current = default;
            }

            object IEnumerator.Current => Current;
        }
    }
}