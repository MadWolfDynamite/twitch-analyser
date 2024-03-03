using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchStreamAnalyser.Domain.Comparers
{
    public class KeyEqualityComparer<T, TKey> : IEqualityComparer<T>
    {
        protected readonly Func<T, TKey> m_keyExtractor;

        public KeyEqualityComparer(Func<T, TKey> keyExtractor)
        {
            m_keyExtractor = keyExtractor;
        }

        public virtual bool Equals(T x, T y)
        {
            return EqualityComparer<TKey>.Default.Equals(m_keyExtractor(x), m_keyExtractor(y));
        }

        public int GetHashCode(T obj)
        {
            return m_keyExtractor(obj).GetHashCode();
        }
    }
}
