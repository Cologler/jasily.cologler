using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace System.Collections.Generic
{
    public static class List
    {
        #region IList

        #region wrapper

        public static IReadOnlyList<T> AsReadOnly<T>([NotNull] this IList<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new ReadOnlyCollection<T>(source);
        }

        /// <summary>
        /// return a simple synchronized list which copy from http://referencesource.microsoft.com/.
        /// if you want to use better solution, use System.Collections.Concurrent.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<T> AsSynchronized<T>(IList<T> list) => new SynchronizedList<T>(list);

        private class SynchronizedList<T> : IList<T>, ICollection
        {
            private readonly IList<T> list;

            internal SynchronizedList(IList<T> list)
            {
                this.list = list;
                this.SyncRoot = list.GetOrCreateSyncRoot();
            }

            public void CopyTo(Array array, int index)
            {
                lock (this.SyncRoot)
                {
                    this.list.CopyTo(array, index);
                }
            }

            public int Count
            {
                get
                {
                    lock (this.SyncRoot)
                    {
                        return this.list.Count;
                    }
                }
            }

            public bool IsSynchronized => true;

            public object SyncRoot { get; }

            public bool IsReadOnly => this.list.IsReadOnly;

            public void Add(T item)
            {
                lock (this.SyncRoot)
                {
                    this.list.Add(item);
                }
            }

            public void Clear()
            {
                lock (this.SyncRoot)
                {
                    this.list.Clear();
                }
            }

            public bool Contains(T item)
            {
                lock (this.SyncRoot)
                {
                    return this.list.Contains(item);
                }
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                lock (this.SyncRoot)
                {
                    this.list.CopyTo(array, arrayIndex);
                }
            }

            public bool Remove(T item)
            {
                lock (this.SyncRoot)
                {
                    return this.list.Remove(item);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                lock (this.SyncRoot)
                {
                    return this.list.GetEnumerator();
                }
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                lock (this.SyncRoot)
                {
                    return this.list.GetEnumerator();
                }
            }

            public T this[int index]
            {
                get
                {
                    lock (this.SyncRoot)
                    {
                        return this.list[index];
                    }
                }
                set
                {
                    lock (this.SyncRoot)
                    {
                        this.list[index] = value;
                    }
                }
            }

            public int IndexOf(T item)
            {
                lock (this.SyncRoot)
                {
                    return this.list.IndexOf(item);
                }
            }

            public void Insert(int index, T item)
            {
                lock (this.SyncRoot)
                {
                    this.list.Insert(index, item);
                }
            }

            public void RemoveAt(int index)
            {
                lock (this.SyncRoot)
                {
                    this.list.RemoveAt(index);
                }
            }
        }

        #endregion

        #region index

        private static int InternalFindIndex<T>([NotNull] IList<T> source, int startIndex, int count, [NotNull] Predicate<T> match)
        {
            Debug.Assert(source != null);
            Debug.Assert(match != null);

            count = Math.Min(startIndex + count, source.Count);
            for (var i = startIndex; i < count; i++)
            {
                if (match(source[i])) return i;
            }
            return -1;
        }

        public static int FindIndex<T>([NotNull] this IList<T> source, int startIndex, int count, [NotNull] Predicate<T> match)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (match == null) throw new ArgumentNullException(nameof(match));

            var list = source as List<T>;
            if (list != null) return list.FindIndex(startIndex, count, match);

            var array = source as T[];
            if (array != null) return Array.FindIndex(array, startIndex, count, match);

            return InternalFindIndex(source, startIndex, count, match);
        }

        public static int FindIndex<T>([NotNull] this IList<T> source, int startIndex, [NotNull] Predicate<T> match)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (match == null) throw new ArgumentNullException(nameof(match));

            var list = source as List<T>;
            if (list != null) return list.FindIndex(startIndex, match);

            var array = source as T[];
            if (array != null) return Array.FindIndex(array, startIndex, match);

            return InternalFindIndex(source, startIndex, source.Count, match);
        }

        public static int FindIndex<T>([NotNull] this IList<T> source, [NotNull] Predicate<T> match)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (match == null) throw new ArgumentNullException(nameof(match));

            var list = source as List<T>;
            if (list != null) return list.FindIndex(match);

            var array = source as T[];
            if (array != null) return Array.FindIndex(array, match);

            return InternalFindIndex(source, 0, source.Count, match);
        }

        #endregion

        #region sort

        // ReSharper disable once SuggestBaseTypeForParameter
        private static void EndOfSort<T>(IList<T> source, List<T> list)
        {
            Debug.Assert(!ReferenceEquals(source, list));
            Debug.Assert(source.Count == list.Count);
            for (var i = 0; i < list.Count; i++)
            {
                if (!ReferenceEquals(source[i], list[i]))
                {
                    source[i] = list[i];
                }
            }
        }

        public static void Sort<T>([NotNull] this IList<T> source)
            where T : class
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var list = new List<T>(source);
            list.Sort();
            EndOfSort(source, list);
        }

        public static void Sort<T>([NotNull] this IList<T> source, [NotNull] IComparer<T> comparer)
            where T : class
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            var list = new List<T>(source);
            list.Sort(comparer);
            EndOfSort(source, list);
        }

        public static void Sort<T>([NotNull] this IList<T> source, [NotNull] Comparison<T> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (comparison == null) throw new ArgumentNullException(nameof(comparison));
            var list = new List<T>(source);
            list.Sort(comparison);
            EndOfSort(source, list);
        }

        public static void Sort<T>([NotNull] this IList<T> source, int index, int count, [NotNull] IComparer<T> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            var list = new List<T>(source);
            list.Sort(index, count, comparer);
            EndOfSort(source, list);
        }

        public static void SortBy<TSource, TKey>([NotNull] this IList<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            var list = source.OrderBy(keySelector).ToList();
            EndOfSort(source, list);
        }

        public static void SortBy<TSource, TKey>([NotNull] this IList<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector, [NotNull] IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            var list = source.OrderBy(keySelector, comparer).ToList();
            EndOfSort(source, list);
        }

        public static void SortBy<TSource, TKey>([NotNull] this IList<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector, Comparison<TKey> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            EndOfSort(source, source.OrderBy(keySelector, Comparer<TKey>.Create(comparison)).ToList());
        }

        public static void SortByDescending<TSource, TKey>([NotNull] this IList<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            EndOfSort(source, source.OrderByDescending(keySelector).ToList());
        }

        public static void SortByDescending<TSource, TKey>([NotNull] this IList<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector, [NotNull] IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            EndOfSort(source, source.OrderByDescending(keySelector, comparer).ToList());
        }

        public static void SortByDescending<TSource, TKey>([NotNull] this IList<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector, Comparison<TKey> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            EndOfSort(source, source.OrderByDescending(keySelector, Comparer<TKey>.Create(comparison)).ToList());
        }

        #region async

        public static async Task SortAsync<T>([NotNull] this IList<T> source, bool setValueOnTask = false)
            where T : class
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var list = new List<T>(source);
            await Task.Run(() => list.Sort());
            if (setValueOnTask)
            {
                await Task.Run(() => EndOfSort(source, list));
            }
            else
            {
                EndOfSort(source, list);
            }
        }

        public static async Task SortAsync<T>([NotNull] this IList<T> source, [NotNull] IComparer<T> comparer,
            bool setValueOnTask = false)
            where T : class
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            var list = new List<T>(source);
            await Task.Run(() => list.Sort(comparer));
            if (setValueOnTask)
            {
                await Task.Run(() => EndOfSort(source, list));
            }
            else
            {
                EndOfSort(source, list);
            }
        }

        public static async Task SortAsync<T>([NotNull] this IList<T> source, [NotNull] Comparison<T> comparison,
            bool setValueOnTask = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (comparison == null) throw new ArgumentNullException(nameof(comparison));
            var list = new List<T>(source);
            await Task.Run(() => list.Sort(comparison));
            if (setValueOnTask)
            {
                await Task.Run(() => EndOfSort(source, list));
            }
            else
            {
                EndOfSort(source, list);
            }
        }

        public static async Task SortAsync<T>([NotNull] this IList<T> source, int index, int count,
            [NotNull] IComparer<T> comparer, bool setValueOnTask = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            var list = new List<T>(source);
            await Task.Run(() => list.Sort(index, count, comparer));
            if (setValueOnTask)
            {
                await Task.Run(() => EndOfSort(source, list));
            }
            else
            {
                EndOfSort(source, list);
            }
        }

        public static async Task SortByAsync<TSource, TKey>([NotNull] this IList<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector, bool setValueOnTask = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            var list = await Task.Run(() => source.OrderBy(keySelector).ToList());
            if (setValueOnTask)
            {
                await Task.Run(() => EndOfSort(source, list));
            }
            else
            {
                EndOfSort(source, list);
            }
        }

        public static async Task SortByAsync<TSource, TKey>([NotNull] this IList<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector, [NotNull] IComparer<TKey> comparer,
            bool setValueOnTask = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            var list = await Task.Run(() => source.OrderBy(keySelector, comparer).ToList());
            if (setValueOnTask)
            {
                await Task.Run(() => EndOfSort(source, list));
            }
            else
            {
                EndOfSort(source, list);
            }
        }

        public static async Task SortByAsync<TSource, TKey>([NotNull] this IList<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector, [NotNull] Comparison<TKey> comparison,
            bool setValueOnTask = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (comparison == null) throw new ArgumentNullException(nameof(comparison));
            var comparer = Comparer<TKey>.Create(comparison);
            var list = await Task.Run(() => source.OrderBy(keySelector, comparer).ToList());
            if (setValueOnTask)
            {
                await Task.Run(() => EndOfSort(source, list));
            }
            else
            {
                EndOfSort(source, list);
            }
        }

        public static async Task SortByDescendingAsync<TSource, TKey>([NotNull] this IList<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector, bool setValueOnTask = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            var list = await Task.Run(() => source.OrderByDescending(keySelector).ToList());
            if (setValueOnTask)
            {
                await Task.Run(() => EndOfSort(source, list));
            }
            else
            {
                EndOfSort(source, list);
            }
        }

        public static async Task SortByDescendingAsync<TSource, TKey>([NotNull] this IList<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector, [NotNull] IComparer<TKey> comparer,
            bool setValueOnTask = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            var list = await Task.Run(() => source.OrderByDescending(keySelector, comparer).ToList());
            if (setValueOnTask)
            {
                await Task.Run(() => EndOfSort(source, list));
            }
            else
            {
                EndOfSort(source, list);
            }
        }

        public static async Task SortByDescendingAsync<TSource, TKey>([NotNull] this IList<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector, [NotNull] Comparison<TKey> comparison,
            bool setValueOnTask = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (comparison == null) throw new ArgumentNullException(nameof(comparison));
            var comparer = Comparer<TKey>.Create(comparison);
            var list = await Task.Run(() => source.OrderByDescending(keySelector, comparer).ToList());
            if (setValueOnTask)
            {
                await Task.Run(() => EndOfSort(source, list));
            }
            else
            {
                EndOfSort(source, list);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Linq

        public static IEnumerable<T> Skip<T>([NotNull] this IList<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            for (var i = count; i < source.Count; i++) yield return source[i];
        }

        #endregion
    }
}