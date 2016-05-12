using System;
using System.Collections.Generic;

namespace Jasily
{
    public struct CachedHashCode<T> : IEquatable<CachedHashCode<T>>, IEquatable<T>
    {
        private bool hasHashCode;
        private int hashCode;

        public CachedHashCode(T item)
        {
            this.Item = item;
            this.hashCode = 0;
            this.hasHashCode = false;
        }

        public T Item { get; }

        #region Overrides of ValueType

        /// <summary>指示当前对象是否等于同一类型的另一个对象。</summary>
        /// <returns>如果当前对象等于 <paramref name="other" /> 参数，则为 true；否则为 false。</returns>
        /// <param name="other">与此对象进行比较的对象。</param>
        public bool Equals(CachedHashCode<T> other) => EqualityComparer<T>.Default.Equals(this.Item, other.Item);

        /// <summary>指示当前对象是否等于同一类型的另一个对象。</summary>
        /// <returns>如果当前对象等于 <paramref name="other" /> 参数，则为 true；否则为 false。</returns>
        /// <param name="other">与此对象进行比较的对象。</param>
        public bool Equals(T other) => EqualityComparer<T>.Default.Equals(this.Item, other);

        /// <summary>返回此实例的哈希代码。</summary>
        /// <returns>一个 32 位有符号整数，它是该实例的哈希代码。</returns>
        public override int GetHashCode()
        {
            if (!this.hasHashCode)
            {
                this.hashCode = this.Item?.GetHashCode() ?? 0;
                this.hasHashCode = true;
            }
            return this.hashCode;
        }

        #endregion
    }
}