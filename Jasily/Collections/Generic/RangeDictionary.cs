using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Jasily.Collections.Generic
{
    public static class RangeDictionary
    {
        public static RangeDictionary<TKey, TValue> Create<TKey, TValue>(Range<TKey> maxRange)
            where TKey : struct, IComparable<TKey>
        {
            return new RangeDictionary<TKey, TValue>(maxRange);
        }
    }

    public class RangeDictionary<TKey, TValue> : IEnumerable<KeyValuePair<Range<TKey>, TValue>>
        where TKey : struct, IComparable<TKey>
    {
        private readonly Range<TKey> maxRange;
        private readonly List<Range<TKey>> ranges = new List<Range<TKey>>();
        private readonly List<TValue> values = new List<TValue>();

        [NotNull]
        public IComparer<TKey> Comparer { get; }

        public RangeDictionary(Range<TKey> maxRange)
        {
            this.maxRange = maxRange;
            this.Comparer = Comparer<TKey>.Default;
        }

        private class AddComparer : IComparer<Range<TKey>>
        {
            public int Compare(Range<TKey> x, Range<TKey> y)
            {
                var mode = x.Overlaps(y);
                switch (mode)
                {
                    case Range<TKey>.RangeOverlapsMode.AABB:
                        return -1;

                    case Range<TKey>.RangeOverlapsMode.BBAA:
                        return 1;

                    case Range<TKey>.RangeOverlapsMode.ABAB:
                    case Range<TKey>.RangeOverlapsMode.BABA:
                    case Range<TKey>.RangeOverlapsMode.ABBA:
                    case Range<TKey>.RangeOverlapsMode.BAAB:
                    case Range<TKey>.RangeOverlapsMode.BA_A:
                    case Range<TKey>.RangeOverlapsMode.A_AB:
                    case Range<TKey>.RangeOverlapsMode.A_BA:
                    case Range<TKey>.RangeOverlapsMode.A__A:
                    case Range<TKey>.RangeOverlapsMode.AB_A:
                        throw new ArgumentException("data overlaps.");

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// if key range overlaps, it will throw a ArgumentException;
        /// </summary>
        /// <param name="range"></param>
        /// <param name="value"></param>
        public void Add(Range<TKey> range, TValue value)
        {
            if (!this.maxRange.Contains(range)) throw new ArgumentOutOfRangeException();

            if (this.ranges.Count == 0)
            {
                this.ranges.Add(range);
                this.values.Add(value);
            }
            else
            {
                var index = this.ranges.BinarySearch(range, Singleton.Instance<AddComparer>());
                Debug.Assert(index < 0);
                this.ranges.Insert(~index, range);
                this.values.Insert(~index, value);
            }
        }

        public int Count
        {
            get
            {
                Debug.Assert(this.ranges.Count == this.values.Count);
                return this.ranges.Count;
            }
        }

        private class FindComparer : IComparer<Range<TKey>>
        {
            public int Compare(Range<TKey> x, Range<TKey> y)
            {
                var mode = x.Overlaps(y);
                switch (mode)
                {
                    case Range<TKey>.RangeOverlapsMode.AABB:
                        return -1;

                    case Range<TKey>.RangeOverlapsMode.BBAA:
                    case Range<TKey>.RangeOverlapsMode.BABA:
                    case Range<TKey>.RangeOverlapsMode.BAAB:
                    case Range<TKey>.RangeOverlapsMode.BA_A:
                    case Range<TKey>.RangeOverlapsMode.ABAB:
                    case Range<TKey>.RangeOverlapsMode.A_AB:
                        return 1;

                    case Range<TKey>.RangeOverlapsMode.ABBA:
                    case Range<TKey>.RangeOverlapsMode.A_BA:
                    case Range<TKey>.RangeOverlapsMode.A__A:
                    case Range<TKey>.RangeOverlapsMode.AB_A:
                        return 0;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool ContainsKey(TKey key)
        {
            if (this.maxRange.Contains(key) && this.Count > 0)
            {
                var index = this.ranges.BinarySearch(new Range<TKey>(key), Singleton.Instance<FindComparer>());
                if (index >= 0) return true;
            }

            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (this.maxRange.Contains(key) && this.Count > 0)
            {
                var index = this.ranges.BinarySearch(new Range<TKey>(key), Singleton.Instance<FindComparer>());
                if (index >= 0)
                {
                    value = this.values[index];
                    return true;
                }
            }

            value = default(TValue);
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (!this.TryGetValue(key, out value)) throw new KeyNotFoundException();
                return value;
            }
        }

        private class SetComparer : IComparer<Range<TKey>>
        {
            public int Compare(Range<TKey> x, Range<TKey> y)
            {
                var mode = x.Overlaps(y);
                switch (mode)
                {
                    case Range<TKey>.RangeOverlapsMode.AABB:
                        return -1;

                    case Range<TKey>.RangeOverlapsMode.ABAB:
                    case Range<TKey>.RangeOverlapsMode.A_AB:

                    case Range<TKey>.RangeOverlapsMode.BBAA:
                    case Range<TKey>.RangeOverlapsMode.BABA:
                    case Range<TKey>.RangeOverlapsMode.BAAB:
                    case Range<TKey>.RangeOverlapsMode.BA_A:
                        return 1;

                    case Range<TKey>.RangeOverlapsMode.ABBA:
                    case Range<TKey>.RangeOverlapsMode.A__A:
                    case Range<TKey>.RangeOverlapsMode.A_BA:
                    case Range<TKey>.RangeOverlapsMode.AB_A:
                        return 0;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void InternalArraySet(int index, Range<TKey> key, Container<TValue> value)
        {
            if (value.HasValue)
            {
                this.ranges[index] = key;
                this.values[index] = value.Value;
            }
            else
            {
                this.ranges.RemoveAt(index);
                this.values.RemoveAt(index);
            }
        }

        private void InternalArrayInsert(int index, Range<TKey> key, Container<TValue> value)
        {
            if (value.HasValue)
            {
                this.ranges.Insert(index, key);
                this.values.Insert(index, value.Value);
            }
        }

        private void InternalArrayAdd(Range<TKey> key, Container<TValue> value)
        {
            if (value.HasValue)
            {
                this.ranges.Add(key);
                this.values.Add(value.Value);
            }
        }

        private void InternalArrayRemoveAt(int index)
        {
            this.ranges.RemoveAt(index);
            this.values.RemoveAt(index);
        }

        private void InternalSet(Range<TKey> key, Container<TValue> value)
        {
            if (!this.maxRange.Contains(key)) throw new ArgumentOutOfRangeException();

            if (this.ranges.Count == 0)
            {
                this.InternalArrayAdd(key, value);
            }
            else
            {
                var index = this.ranges.BinarySearch(key, Singleton.Instance<SetComparer>());
                if (index >= 0)
                {
                    var r = this.ranges[index];
                    var v = this.values[index];
                    var mode = r.Overlaps(key);
                    switch (mode)
                    {
                        case Range<TKey>.RangeOverlapsMode.ABBA:
                            this.ranges[index] = new Range<TKey>(CriticalValue.Reverse(key.Max), r.Max);
                            this.InternalArrayInsert(index, key, value);
                            this.ranges.Insert(index, new Range<TKey>(r.Min, CriticalValue.Reverse(key.Min)));
                            this.values.Insert(index, v);
                            break;

                        case Range<TKey>.RangeOverlapsMode.A__A:
                            this.InternalArraySet(index, key, value);
                            break;

                        case Range<TKey>.RangeOverlapsMode.A_BA:
                            this.ranges[index] = new Range<TKey>(CriticalValue.Reverse(key.Max), r.Max);
                            this.InternalArrayInsert(index, key, value);
                            break;

                        case Range<TKey>.RangeOverlapsMode.AB_A:
                            this.ranges[index] = new Range<TKey>(r.Min, CriticalValue.Reverse(key.Min));
                            this.InternalArrayInsert(index + 1, key, value);
                            break;

                        default:
                            throw new InvalidOperationException();
                    }
                }
                else
                {
                    index = ~index;
                    if (index < this.ranges.Count)
                    {
                        var r = this.ranges[index];
                        var v = this.values[index];
                        var mode = r.Overlaps(key);
                        switch (mode)
                        {
                            case Range<TKey>.RangeOverlapsMode.ABAB:
                                this.InternalArrayInsert(index + 1, key, value);
                                this.ranges[index] = new Range<TKey>(r.Min, CriticalValue.Reverse(key.Min));
                                break;

                            case Range<TKey>.RangeOverlapsMode.BBAA:
                                this.InternalArrayInsert(index, key, value);
                                break;

                            case Range<TKey>.RangeOverlapsMode.BABA:
                                this.InternalArrayInsert(index, key, value);
                                this.ranges[index + 1] = new Range<TKey>(CriticalValue.Reverse(key.Max), r.Max);
                                break;

                            case Range<TKey>.RangeOverlapsMode.A_AB:
                            case Range<TKey>.RangeOverlapsMode.BA_A:
                                this.InternalArraySet(index, key, value);
                                break;

                            case Range<TKey>.RangeOverlapsMode.BAAB:
                                this.InternalArraySet(index, key, value);
                                if (value.HasValue) index++;
                                while (index < this.ranges.Count)
                                {
                                    mode = this.ranges[index].Overlaps(key);
                                    switch (mode)
                                    {
                                        case Range<TKey>.RangeOverlapsMode.BBAA:
                                            return;

                                        case Range<TKey>.RangeOverlapsMode.BABA:
                                            this.ranges[index] = new Range<TKey>(CriticalValue.Reverse(key.Max), this.ranges[index].Max);
                                            return;

                                        case Range<TKey>.RangeOverlapsMode.BAAB:
                                            this.InternalArrayRemoveAt(index);
                                            break;

                                        case Range<TKey>.RangeOverlapsMode.BA_A:
                                            this.InternalArrayRemoveAt(index);
                                            return;

                                        default:
                                            throw new InvalidOperationException();
                                    }
                                }
                                break;

                            default:
                                throw new InvalidOperationException();
                        }
                    }
                    else
                    {
                        this.InternalArrayAdd(key, value);
                    }
                }
            }
        }

        public void Set(Range<TKey> key, TValue value) => this.InternalSet(key, new Container<TValue>(value));

        public void RemoveKey(Range<TKey> key) => this.InternalSet(key, new Container<TValue>());

        public IEnumerator<KeyValuePair<Range<TKey>, TValue>> GetEnumerator()
        {
            Debug.Assert(this.ranges.Count == this.values.Count);
            return
                this.ranges.Select((t, i) => new KeyValuePair<Range<TKey>, TValue>(t, this.values[i])).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}