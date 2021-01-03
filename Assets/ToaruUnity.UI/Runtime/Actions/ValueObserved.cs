using System;
using System.Collections.Generic;

namespace ToaruUnity.UI
{
    public struct ValueObserved<T> : IEquatable<ValueObserved<T>>
    {
        private T m_Value;
        private bool m_Changed;


        public T Value
        {
            get => m_Value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(m_Value, value))
                {
                    m_Value = value;
                    m_Changed = true;
                }
            }
        }


        public bool ApplyChanges()
        {
            bool value = m_Changed;
            m_Changed = false;
            return value;
        }


        public ValueObserved(T value) : this(value, false) { }

        public ValueObserved(bool changed) : this(default, changed) { }

        public ValueObserved(T value, bool changed)
        {
            m_Value = value;
            m_Changed = changed;
        }


        public override string ToString() 
            => m_Value?.ToString();

        public override bool Equals(object obj) 
            => obj is ValueObserved<T> other && Equals(other);

        public bool Equals(ValueObserved<T> other)
            => (m_Changed == other.m_Changed) && EqualityComparer<T>.Default.Equals(m_Value, other.Value);

        public override int GetHashCode() 
            => EqualityComparer<T>.Default.GetHashCode(m_Value) + m_Changed.GetHashCode();


        public static bool operator ==(ValueObserved<T> left, ValueObserved<T> right) => left.Equals(right);

        public static bool operator !=(ValueObserved<T> left, ValueObserved<T> right) => !(left == right);

        public static implicit operator T(ValueObserved<T> observed) => observed.Value;

        public static explicit operator ValueObserved<T>(T value) => new ValueObserved<T>(value);
    }
}