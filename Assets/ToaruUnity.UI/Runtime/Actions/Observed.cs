using System;
using System.Collections.Generic;

namespace ToaruUnity.UI
{
    public class Observed<T> : IEquatable<Observed<T>>
    {
        private T m_Value;
        private bool m_Changed;
        private readonly IEqualityComparer<T> m_Comparer;
 

        public T Value
        {
            get => m_Value;
            set
            {
                if (!m_Comparer.Equals(m_Value, value))
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


        public Observed() :this(default, false, EqualityComparer<T>.Default) { }

        public Observed(T value) : this(value, false, EqualityComparer<T>.Default) { }

        public Observed(bool changed) : this(default, changed, EqualityComparer<T>.Default) { }

        public Observed(IEqualityComparer<T> comparer) : this(default, false, comparer) { }

        public Observed(T value, bool changed) : this(value, changed, EqualityComparer<T>.Default) { }

        public Observed(T value, IEqualityComparer<T> comparer) : this(value, false, comparer) { }

        public Observed(bool changed, IEqualityComparer<T> comparer) : this(default, changed, comparer) { }

        public Observed(T value, bool changed, IEqualityComparer<T> comparer)
        {
            m_Value = value;
            m_Changed = changed;
            m_Comparer = comparer ?? EqualityComparer<T>.Default;
        }


        public override string ToString() 
            => m_Value?.ToString();

        public override bool Equals(object obj) 
            => obj is Observed<T> other && Equals(other);

        public bool Equals(Observed<T> other)
            => (m_Changed == other.m_Changed) && (m_Comparer == other.m_Comparer) && m_Comparer.Equals(m_Value, other.Value);

        public override int GetHashCode()
            => ((m_Comparer.GetHashCode(m_Value) + m_Changed.GetHashCode()) * 31) + m_Comparer.GetHashCode();


        public static bool operator ==(Observed<T> left, Observed<T> right) => left.Equals(right);

        public static bool operator !=(Observed<T> left, Observed<T> right) => !(left == right);


        public static implicit operator T(Observed<T> observed) => observed.Value;

        public static explicit operator Observed<T>(T value) => new Observed<T>(value);
    }
}