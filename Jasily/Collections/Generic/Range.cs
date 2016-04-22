using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Jasily.Collections.Generic
{
    [DebuggerDisplay("{ToString()}")]
    public struct Range<T> : IComparable<T>, IEquatable<Range<T>>, IComparable<Range<T>>
        where T : struct, IComparable<T>
    {
        public Range(T value)
        {
            this.Min = this.Max = new CriticalValue<T>(value, true);
        }

        public Range(CriticalValue<T> min, CriticalValue<T> max)
        {
            if (min > max) throw new ArgumentException();

            this.Min = min;
            this.Max = max;
        }

        public Range(T min, T max, RangeMode mode = RangeMode.IncludeMax | RangeMode.IncludeMin)
        {
            var r = min.CompareTo(max);
            if (r > 0) throw new ArgumentException();
            if (r == 0)
            {
                if (mode != (RangeMode.IncludeMax | RangeMode.IncludeMin)) throw new ArgumentException();
                this.Min = this.Max = new CriticalValue<T>(min, true);
            }
            else
            {
                this.Min = new CriticalValue<T>(min, (mode & RangeMode.IncludeMin) == RangeMode.IncludeMin);
                this.Max = new CriticalValue<T>(max, (mode & RangeMode.IncludeMax) == RangeMode.IncludeMax);
            }
        }

        public CriticalValue<T> Min { get; }

        public CriticalValue<T> Max { get; }

        internal void CheckInitialized()
        {
            var r = this.Min.CompareTo(this.Max);
            if (r < 0) return;
            if (r == 0 && this.Min.Include && this.Max.Include) return;
            throw new InvalidOperationException("struct was not initialize.");
        }

        public int CompareTo(Range<T> other)
        {
            this.CheckInitialized();
            other.CheckInitialized();

            if (this.Min > other.Max) return 1;
            if (this.Max < other.Min) return -1;
            // 有重叠部分
            return this.Min.CompareTo(other.Min) + this.Max.CompareTo(other.Max);
        }

        public override string ToString()
        {
            this.CheckInitialized();
            return $"{(this.Min.Include ? "[" : "(")}{this.Min.Value}, {this.Max.Value}{(this.Max.Include ? "]" : ")")}";
        }

        [Pure]
        public bool Contains(Range<T> other)
        {
            this.CheckInitialized();
            other.CheckInitialized();

            var a = this.Min.CompareTo(other.Min);
            if (a > 0) return false;
            var b = this.Max.CompareTo(other.Max);
            if (b < 0) return false;
            return true;
        }

        [Pure]
        public bool Includes(Range<T> other)
        {
            this.CheckInitialized();
            other.CheckInitialized();

            return this.Min < other.Min && this.Max > other.Max;
        }

        [Pure]
        public bool Equals(Range<T> other)
        {
            this.CheckInitialized();
            other.CheckInitialized();

            return this.Min == other.Min && this.Max == other.Max;
        }

        [Pure]
        public bool Contains(T value) => this.CompareTo(value) == 0;

        [Pure]
        public int CompareTo(T value)
        {
            this.CheckInitialized();

            if (value < this.Min) return 1;
            if (value > this.Max) return -1;
            return 0;
        }

        [Pure]
        public RangeOverlapsMode Overlaps(Range<T> other)
        {
            this.CheckInitialized();
            other.CheckInitialized();

            // self is A
            if (this.Min > other.Max) return RangeOverlapsMode.BBAA;

            if (this.Max < other.Min) return RangeOverlapsMode.AABB;

            var a = this.Min.CompareTo(other.Min);
            var b = this.Max.CompareTo(other.Max);
            if (a < 0)
            {
                if (b < 0) return RangeOverlapsMode.ABAB;
                else if (b == 0) return RangeOverlapsMode.AB_A;
                else return RangeOverlapsMode.ABBA;
            }
            else if (a == 0)
            {
                if (b < 0) return RangeOverlapsMode.A_AB;
                else if (b == 0) return RangeOverlapsMode.A__A;
                else return RangeOverlapsMode.A_BA;
            }
            else
            {
                if (b < 0) return RangeOverlapsMode.BAAB;
                else if (b == 0) return RangeOverlapsMode.BA_A;
                else return RangeOverlapsMode.BABA;
            }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum RangeOverlapsMode
        {
            AABB,
            BBAA,

            ABAB,
            ABBA,
            AB_A,

            BABA,
            BAAB,
            BA_A,

            A_AB,
            A_BA,
            /// <summary>
            /// equals.
            /// </summary>
            A__A,
        }

        public static bool operator <(Range<T> left, Range<T> right)
        {
            left.CheckInitialized();
            right.CheckInitialized();

            return left.Max < right.Min;
        }

        public static bool operator >(Range<T> left, Range<T> right)
        {
            left.CheckInitialized();
            right.CheckInitialized();

            return left.Min > right.Max;
        }
    }
}