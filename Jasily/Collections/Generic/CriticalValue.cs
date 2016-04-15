using System;
using System.Diagnostics;

namespace Jasily.Collections.Generic
{
    /// <summary>
    /// 临界值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("{Value}")]
    public struct CriticalValue<T> : IEquatable<CriticalValue<T>>, IComparable<CriticalValue<T>>, IComparable<T>, IEquatable<T>
        where T : struct, IComparable<T>
    {
        public T Value { get; }

        public bool Include { get; }

        public CriticalValue(T value, bool include)
        {
            this.Value = value;
            this.Include = include;
        }

        public static bool operator <(CriticalValue<T> left, CriticalValue<T> right) => left.CompareTo(right) < 0;

        public static bool operator <(CriticalValue<T> left, T right) => left.CompareTo(right) < 0;

        public static bool operator <(T right, CriticalValue<T> left) => left.CompareTo(right) > 0;

        public static bool operator >(CriticalValue<T> left, CriticalValue<T> right) => left.CompareTo(right) > 0;

        public static bool operator >(CriticalValue<T> left, T right) => left.CompareTo(right) > 0;

        public static bool operator >(T right, CriticalValue<T> left) => left.CompareTo(right) < 0;

        public static bool operator ==(CriticalValue<T> left, CriticalValue<T> right) => left.Equals(right);

        public static bool operator ==(CriticalValue<T> left, T right) => left.Equals(right);

        public static bool operator ==(T right, CriticalValue<T> left) => left.Equals(right);

        public static bool operator !=(CriticalValue<T> left, CriticalValue<T> right) => !(left == right);

        public static bool operator !=(CriticalValue<T> left, T right) => !(left == right);

        public static bool operator !=(T right, CriticalValue<T> left) => !(left == right);

        public static bool operator >=(CriticalValue<T> left, CriticalValue<T> right) => left.CompareTo(right) >= 0;

        public static bool operator <=(CriticalValue<T> left, CriticalValue<T> right) => left.CompareTo(right) <= 0;

        public int CompareTo(CriticalValue<T> other)
        {
            var r = this.Value.CompareTo(other.Value);
            if (r != 0) return r;
            if (this.Include == other.Include) return 0;
            return this.Include ? 1 : -1;
        }

        public int CompareTo(T other)
        {
            var r = this.Value.CompareTo(other);
            if (r != 0) return r;
            return this.Include ? 0 : -1;
        }

        public bool Equals(CriticalValue<T> other) => this.CompareTo(other) == 0;

        public bool Equals(T other) => this.CompareTo(other) == 0;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj is CriticalValue<T>) return this.Equals((CriticalValue<T>)obj);
            if (obj is T) return this.Equals((T)obj);
            return false;
        }

        public override int GetHashCode() => this.Value.GetHashCode();
    }

    public static class CriticalValue
    {
        public static CriticalValue<T> Reverse<T>(CriticalValue<T> other) where T : struct, IComparable<T>
            => new CriticalValue<T>(other.Value, !other.Include);
    }
}