using System;
using System.Collections;
using System.Collections.Generic;
using Minecraft.Lua;

namespace Minecraft.Collections
{
    public class LRULinkedMap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, ILuaCallCSharp
    {
        private class LRUNode
        {
            public TKey Key;
            public TValue Value;
            public int HashCode;
            public LRUNode Before; // 双向链表的上一个节点
            public LRUNode After; // 双向链表的下一个节点
            public LRUNode Next; // 同一个桶中的下一个节点
        }


        private readonly LRUNode[] m_Buckets;
        private readonly IEqualityComparer<TKey> m_Comparer;
        private int m_Count;
        private int m_Version;
        private LRUNode m_Head;
        private LRUNode m_Tail;
        private LRUNode m_FreeList;

        public event Action<TKey, TValue> OnValueRemoved;

        public LRULinkedMap(int capacity) : this(capacity, null) { }

        public LRULinkedMap(int capacity, IEqualityComparer<TKey> comparer)
        {
            m_Buckets = new LRUNode[HashUtility.GetPrimeCapacity(capacity)];
            m_Comparer = comparer ?? EqualityComparer<TKey>.Default;
            m_Count = 0;
            m_Version = 0;
            m_Head = null;
            m_Tail = null;
            m_FreeList = null;
        }


        public int Count => m_Count;
        public int Capacity => m_Buckets.Length;
        public IEqualityComparer<TKey> Comparer => m_Comparer;

        public TValue this[TKey key]
        {
            get
            {
                LRUNode node = FindNode(key) ?? throw new KeyNotFoundException();
                return node.Value;
            }
            set => AddOrSet(key, value);
        }


        private LRUNode FindNode(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            int hashCode = m_Comparer.GetHashCode(key) & int.MaxValue; // <=> abs(hash)
            LRUNode node = m_Buckets[hashCode % m_Buckets.Length];

            while (node != null)
            {
                if (node.HashCode == hashCode && m_Comparer.Equals(node.Key, key))
                {
                    MoveToHead(node);
                    return node;
                }

                node = node.Next;
            }

            return null;
        }

        public bool ContainsKey(TKey key)
        {
            return FindNode(key) != null;
        }

        public void AddOrSet(TKey key, TValue value)
        {
            LRUNode node = FindNode(key);

            if (node != null)
            {
                node.Value = value;
                return;
            }

            if (m_Count == m_Buckets.Length)
            {
                RemoveTail();
            }

            if (m_FreeList != null)
            {
                node = m_FreeList;
                m_FreeList = m_FreeList.Next;
            }
            else
            {
                node = new LRUNode();
            }

            int hashCode = m_Comparer.GetHashCode(key) & int.MaxValue; // <=> abs(hash)
            int index = hashCode % m_Buckets.Length;
            InitializeNode(node, m_Buckets[index], key, value, hashCode);
            m_Buckets[index] = node;
            m_Count++;

            MoveToHead(node);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            LRUNode node = FindNode(key);

            if (node == null)
            {
                value = default;
                return false;
            }

            value = node.Value;
            return true;
        }

        public bool Remove(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            int hashCode = m_Comparer.GetHashCode(key) & int.MaxValue; // <=> abs(hash)
            int index = hashCode % m_Buckets.Length;
            LRUNode previousNode = null;
            LRUNode node = m_Buckets[index];

            while (node != null)
            {
                if (node.HashCode == hashCode && m_Comparer.Equals(node.Key, key))
                {
                    if (previousNode == null)
                    {
                        m_Buckets[index] = node.Next;
                    }
                    else
                    {
                        previousNode.Next = node.Next;
                    }

                    node.Next = m_FreeList;
                    m_FreeList = node;
                    m_Count--;
                    m_Version++;

                    OnValueRemoved?.Invoke(node.Key, node.Value);
                    return true;
                }

                previousNode = node;
                node = node.Next;
            }

            return false;
        }

        public void Clear()
        {
            LRUNode node = m_Head;

            while (node != null)
            {
                node.Next = m_FreeList;
                m_FreeList = node;
                node = node.After;
            }

            Array.Clear(m_Buckets, 0, m_Buckets.Length);
            m_Head = m_Tail = null;
            m_Count = 0;
            m_Version++;
        }

        private void InitializeNode(LRUNode node, LRUNode next, TKey key, TValue value, int hashCode)
        {
            node.Key = key;
            node.Value = value;
            node.HashCode = hashCode;
            node.Next = next;
            node.Before = null;
            node.After = null;
        }

        private void MoveToHead(LRUNode node)
        {
            if (ReferenceEquals(m_Head, node))
            {
                return;
            }

            m_Version++;

            if (m_Head == null || m_Tail == null)
            {
                node.Before = null;
                node.After = null;
                m_Head = m_Tail = node;
                return;
            }

            if (node.Before != null)
            {
                node.Before.After = node.After;
            }

            if (node.After != null)
            {
                node.After.Before = node.Before;
            }

            if (ReferenceEquals(m_Tail, node))
            {
                m_Tail = node.Before;
            }

            node.Before = null;
            node.After = m_Head;
            m_Head.Before = node;
            m_Head = node;
        }

        private void RemoveTail()
        {
            if (m_Tail == null)
            {
                return;
            }

            LRUNode node = m_Tail;
            m_Tail = node.Before;

            if (m_Tail == null)
            {
                m_Head = null;
            }
            else
            {
                m_Tail.After = null;
            }

            Remove(node.Key);
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private LRULinkedMap<TKey, TValue> m_Map;
            private LRUNode m_CurrentNode;
            private int m_Version;

            public Enumerator(LRULinkedMap<TKey, TValue> map)
            {
                m_Map = map;
                m_CurrentNode = map.m_Head;
                m_Version = map.m_Version;
            }

            public KeyValuePair<TKey, TValue> Current
            {
                get
                {
                    if (m_Map == null)
                    {
                        throw new ObjectDisposedException(nameof(Enumerator));
                    }

                    if (m_Map.m_Version != m_Version)
                    {
                        throw new InvalidOperationException("Collection was modified.");
                    }

                    if (m_CurrentNode == null)
                    {
                        return default;
                    }

                    return new KeyValuePair<TKey, TValue>(m_CurrentNode.Key, m_CurrentNode.Value);
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                m_Map = null;
                m_CurrentNode = null;
            }

            public bool MoveNext()
            {
                if (m_Map == null)
                {
                    throw new ObjectDisposedException(nameof(Enumerator));
                }

                if (m_Map.m_Version != m_Version)
                {
                    throw new InvalidOperationException("Collection was modified.");
                }

                m_CurrentNode = m_CurrentNode.After;
                return m_CurrentNode != null;
            }

            public void Reset()
            {
                m_CurrentNode = m_Map.m_Head;
                m_Version = m_Map.m_Version;
            }
        }
    }
}
