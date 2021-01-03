using System;
using System.Collections;
using System.Collections.Generic;

namespace ToaruUnity.UI
{
    internal sealed class HybridDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private const int CutoverPoint = 9;
        private const int InitialDictSize = 13;

        private List<KeyValuePair<TKey, TValue>> m_List;
        private Dictionary<TKey, TValue> m_Dict;
        private readonly IEqualityComparer<TKey> m_KeyComparer;


        public TValue this[TKey key]
        {
            get
            {
                if (m_Dict != null)
                {
                    return m_Dict[key];
                }

                if (GetValueFromList(key, out TValue value))
                {
                    return value;
                }

                throw new KeyNotFoundException($"'{key}' Not Found");
            }
            set
            {
                if (m_Dict != null)
                {
                    m_Dict[key] = value;
                }
                else
                {
                    SetValueToList(key, value, false);
                }
            }
        }

        public int Count
        {
            get
            {
                if (m_Dict != null)
                {
                    return m_Dict.Count;
                }

                if (m_List != null)
                {
                    return m_List.Count;
                }

                return 0;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                if (m_Dict != null)
                {
                    return m_Dict.Keys;
                }

                List<KeyValuePair<TKey, TValue>> list = GetList();
                TKey[] keys = new TKey[list.Count];

                for (int i = 0; i < list.Count; i++)
                {
                    keys[i] = list[i].Key;
                }

                return keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                if (m_Dict != null)
                {
                    return m_Dict.Values;
                }

                List<KeyValuePair<TKey, TValue>> list = GetList();
                TValue[] keys = new TValue[list.Count];

                for (int i = 0; i < list.Count; i++)
                {
                    keys[i] = list[i].Value;
                }

                return keys;
            }
        }

        public HybridDictionary(IEqualityComparer<TKey> keyComparer)
        {
            m_KeyComparer = keyComparer ?? throw new ArgumentNullException(nameof(keyComparer));
        }


        private List<KeyValuePair<TKey, TValue>> GetList()
        {
            return m_List ?? (m_List = new List<KeyValuePair<TKey, TValue>>());
        }

        private bool GetValueFromList(TKey key, out TValue value)
        {
            return GetValueFromList(key, out value, out _);
        }

        private bool GetValueFromList(TKey key, out TValue value, out int index)
        {
            List<KeyValuePair<TKey, TValue>> list = GetList();

            for (int i = 0; i < list.Count; i++)
            {
                KeyValuePair<TKey, TValue> entry = list[i];

                if (m_KeyComparer.Equals(entry.Key, key))
                {
                    value = entry.Value;
                    index = i;
                    return true;
                }
            }

            value = default;
            index = default;
            return false;
        }

        private void SetValueToList(TKey key, TValue value, bool add)
        {
            List<KeyValuePair<TKey, TValue>> list = GetList();
            KeyValuePair<TKey, TValue> item = new KeyValuePair<TKey, TValue>(key, value);

            for (int i = 0; i < list.Count; i++)
            {
                KeyValuePair<TKey, TValue> entry = list[i];

                if (m_KeyComparer.Equals(entry.Key, key))
                {
                    if (add)
                    {
                        throw new ArgumentException($"{key} has already existed", nameof(key));
                    }

                    list[i] = item;
                    return;
                }
            }

            if (list.Count >= CutoverPoint - 1)
            {
                ChangeOver();
                m_Dict[key] = value;
            }
            else
            {
                list.Add(item);
            }
        }

        private void ChangeOver()
        {
            if (m_List != null)
            {
                m_Dict = new Dictionary<TKey, TValue>(m_List.Count);

                for (int i = 0; i < m_List.Count; i++)
                {
                    KeyValuePair<TKey, TValue> entry = m_List[i];
                    m_Dict.Add(entry.Key, entry.Value);
                }

                m_List = null;
            }
            else
            {
                m_Dict = new Dictionary<TKey, TValue>(InitialDictSize);
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (m_Dict != null)
            {
                m_Dict.Add(key, value);
            }
            else
            {
                SetValueToList(key, value, true);
            }
        }

        public bool Remove(TKey key)
        {
            if (m_Dict != null)
            {
                return m_Dict.Remove(key);
            }

            List<KeyValuePair<TKey, TValue>> list = GetList();

            for (int i = 0; i < list.Count; i++)
            {
                KeyValuePair<TKey, TValue> entry = list[i];

                if (m_KeyComparer.Equals(entry.Key, key))
                {
                    list.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public void Clear()
        {
            m_Dict?.Clear();
            m_List?.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            if (m_Dict != null)
            {
                return m_Dict.ContainsKey(key);
            }

            return GetValueFromList(key, out _);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (m_Dict != null)
            {
                return m_Dict.TryGetValue(key, out value);
            }

            return GetValueFromList(key, out value);
        }

        public bool TryGetAndRemoveValue(TKey key, out TValue value)
        {
            if (m_Dict != null)
            {
                if (m_Dict.TryGetValue(key, out value))
                {
                    m_Dict.Remove(key);
                    return true;
                }

                return false;
            }

            if (GetValueFromList(key, out value, out int index))
            {
                m_List.RemoveAt(index);
                return true;
            }

            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (m_Dict != null)
            {
                return m_Dict.GetEnumerator();
            }

            return GetList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}