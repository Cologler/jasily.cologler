using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jasily.Collections.Generic
{
    public sealed class NullableHashSet<T> : ISet<T>
    {
        private readonly HashSet<T> innerSet;
        private readonly Container<object> nullContainer = new Container<object>();

        public NullableHashSet()
        {
            this.CheckTypeNullable();
            this.innerSet = new HashSet<T>();
        }

        public NullableHashSet(IEnumerable<T> collection)
        {
            this.CheckTypeNullable();
            this.innerSet = new HashSet<T>(this.WhereOnAdd(collection));
        }

        public NullableHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            this.CheckTypeNullable();
            this.innerSet = new HashSet<T>(this.WhereOnAdd(collection), comparer);
        }

        public NullableHashSet(IEqualityComparer<T> comparer)
        {
            this.CheckTypeNullable();
            this.innerSet = new HashSet<T>(comparer);
        }

        private void CheckTypeNullable()
        {
            var type = typeof(T).GetTypeInfo();
            if (type.IsValueType && type.GetGenericTypeDefinition() != typeof(Nullable<>)) throw new ArgumentException();
        }

        private IEnumerable<T> WhereOnAdd(IEnumerable<T> collection)
        {
            return collection.Where(z =>
            {
                if (ReferenceEquals(null, z))
                {
                    this.SetNull();
                    return false;
                }
                return true;
            });
        }

        private void SetNull() => this.nullContainer.SetValue(null);

        private void RemoveNull() => this.nullContainer.RemoveValue();

        private bool ContainNull() => this.nullContainer.HasValue;

        public Enumerator GetEnumerator() => new Enumerator(this);

        public struct Enumerator : IEnumerator<T>
        {
            private readonly NullableHashSet<T> parent;
            private bool isReset;
            private HashSet<T>.Enumerator? innerEnumerator;

            internal Enumerator(NullableHashSet<T> parent)
            {
                this.parent = parent;
                this.isReset = true;
                this.Current = default(T);
                this.innerEnumerator = null;
            }

            public void Reset()
            {
                this.isReset = true;
                this.Current = default(T);
                this.innerEnumerator = null;
            }

            object IEnumerator.Current => this.Current;

            public T Current { get; private set; }

            public void Dispose()
            {
                this.Reset();
            }

            public bool MoveNext()
            {
                if (this.isReset)
                {
                    this.isReset = false;

                    if (this.parent.ContainNull())
                    {
                        this.Current = (T)(object)null;
                        return true;
                    }
                }

                if (this.innerEnumerator == null)
                    this.innerEnumerator = this.parent.innerSet.GetEnumerator();

                if (this.innerEnumerator.Value.MoveNext())
                {
                    this.Current = this.innerEnumerator.Value.Current;
                    return true;
                }
                return false;
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public bool Add(T item)
        {
            if (!ReferenceEquals(null, item)) return this.innerSet.Add(item);

            if (this.ContainNull()) return false;
            this.SetNull();
            return true;
        }

        public void ExceptWith([NotNull] IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            if (this.Count == 0) return;

            if (other == this)
            {
                this.Clear();
                return;
            }

            foreach (var element in other) this.Remove(element);
        }

        public void IntersectWith([NotNull] IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            if (this.ContainNull())
            {
                this.RemoveNull();
                this.innerSet.IntersectWith(this.WhereOnAdd(other));
            }
            else
            {
                this.innerSet.IntersectWith(other.Where(z => !ReferenceEquals(null, z)));
            }
        }

        public bool IsProperSubsetOf([NotNull] IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (other == this) return false;

            throw new System.NotImplementedException();

            var sc = this.Count;
            var count = (other as ICollection<T>)?.Count ?? -1;
            if (count != -1 && sc >= count) return false;
            count = (other as ICollection)?.Count ?? -1;
            if (count != -1 && sc >= count) return false;

            var containNull = false;
            var where = other.Where(z =>
            {
                if (ReferenceEquals(null, z))
                {
                    containNull = true;
                    return false;
                }
                else
                {
                    return true;
                }
            });
            var isSubset = this.innerSet.IsSubsetOf(where);
            return this.innerSet.IsProperSubsetOf(where) && containNull == this.ContainNull();
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (other == this) return true;

            var containNull = false;
            var where = other.Where(z =>
            {
                if (ReferenceEquals(null, z))
                {
                    containNull = true;
                    return false;
                }
                else
                {
                    return true;
                }
            });
            return this.innerSet.IsSubsetOf(where) && containNull == this.ContainNull();
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public bool Overlaps([NotNull] IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            return this.Count != 0 && other.Any(this.Contains);
        }

        public bool SetEquals([NotNull] IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            var otherAsSet = other as NullableHashSet<T>;
            if (otherAsSet != null && AreEqualityComparersEqual(this, otherAsSet))
            {
                return this.Count == otherAsSet.Count && other.All(this.Contains);
            }

            // ReSharper disable once PossibleMultipleEnumeration
            var count = other.TryGetCount();
            if (this.Count == 0 && count > 0) return false;

            // ReSharper disable once PossibleMultipleEnumeration
            return new NullableHashSet<T>(other, this.Comparer).SetEquals(this);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public void UnionWith([NotNull] IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            foreach (var item in other) this.Add(item);
        }

        void ICollection<T>.Add(T item) => this.Add(item);

        public void Clear()
        {
            this.RemoveNull();
            this.innerSet.Clear();
        }

        public bool Contains(T item) => ReferenceEquals(null, item) ? this.ContainNull() : this.innerSet.Contains(item);

        public void CopyTo(T[] array) => this.CopyTo(array, 0, this.Count);

        public void CopyTo(T[] array, int arrayIndex) => this.CopyTo(array, arrayIndex, this.Count);

        public void CopyTo([NotNull] T[] array, int arrayIndex, int count) => this.CopyToArray(array, arrayIndex, count);

        public bool Remove(T item)
        {
            if (!ReferenceEquals(null, item)) return this.innerSet.Remove(item);

            if (!this.ContainNull()) return false;
            this.RemoveNull();
            return true;
        }

        public int Count => (this.ContainNull() ? 1 : 0) + this.innerSet.Count;

        public bool IsReadOnly => false;

        private static bool AreEqualityComparersEqual(NullableHashSet<T> set1, NullableHashSet<T> set2)
            => set1.innerSet.Comparer.Equals(set2.innerSet.Comparer);

        public IEqualityComparer<T> Comparer => this.innerSet.Comparer;
    }
}