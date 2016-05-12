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

        /// <summary>ָʾ��ǰ�����Ƿ����ͬһ���͵���һ������</summary>
        /// <returns>�����ǰ������� <paramref name="other" /> ��������Ϊ true������Ϊ false��</returns>
        /// <param name="other">��˶�����бȽϵĶ���</param>
        public bool Equals(CachedHashCode<T> other) => EqualityComparer<T>.Default.Equals(this.Item, other.Item);

        /// <summary>ָʾ��ǰ�����Ƿ����ͬһ���͵���һ������</summary>
        /// <returns>�����ǰ������� <paramref name="other" /> ��������Ϊ true������Ϊ false��</returns>
        /// <param name="other">��˶�����бȽϵĶ���</param>
        public bool Equals(T other) => EqualityComparer<T>.Default.Equals(this.Item, other);

        /// <summary>���ش�ʵ���Ĺ�ϣ���롣</summary>
        /// <returns>һ�� 32 λ�з������������Ǹ�ʵ���Ĺ�ϣ���롣</returns>
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