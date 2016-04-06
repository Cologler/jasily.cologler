using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Jasily.Collections.Generic
{
    public class JasilyEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly IEqualityComparer<T> baseComparer;

        public JasilyEqualityComparer([NotNull] IEqualityComparer<T> baseComparer)
        {
            if (baseComparer == null) throw new ArgumentNullException(nameof(baseComparer));
            this.baseComparer = baseComparer;
        }

        public T LastCompareItem1 { get; private set; }

        public T LastCompareItem2 { get; private set; }

        public bool Equals(T x, T y)
            => this.baseComparer.Equals(this.LastCompareItem1 = x, this.LastCompareItem2 = y);

        public int GetHashCode(T obj) => this.baseComparer.GetHashCode(obj);
    }
}