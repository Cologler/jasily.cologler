using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using JetBrains.Annotations;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        #region to

        public static TSource[] ToArray<TSource>([NotNull] this IEnumerable<TSource> source, CancellationToken token)
            => source.ToList(token).ToArray();

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, CancellationToken token)
        {
            var result = new Dictionary<TKey, TSource>();
            foreach (var item in source)
            {
                token.ThrowIfCancellationRequested();
                result.Add(keySelector(item), item);
            }
            return result;
        }
        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken token)
        {
            var result = new Dictionary<TKey, TElement>();
            foreach (var item in source)
            {
                token.ThrowIfCancellationRequested();
                result.Add(keySelector(item), elementSelector(item));
            }
            return result;
        }
        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken token)
        {
            var result = new Dictionary<TKey, TSource>(comparer);
            foreach (var item in source)
            {
                token.ThrowIfCancellationRequested();
                result.Add(keySelector(item), item);
            }
            return result;
        }
        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken token)
        {
            var result = new Dictionary<TKey, TElement>(comparer);
            foreach (var item in source)
            {
                token.ThrowIfCancellationRequested();
                result.Add(keySelector(item), elementSelector(item));
            }
            return result;
        }

        public static List<T> ToList<T>([NotNull] this IEnumerable<T> source, CancellationToken token, int detect = 30)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (detect <= 0) throw new ArgumentOutOfRangeException(nameof(detect));

            // ReSharper disable once PossibleMultipleEnumeration
            var count = source.TryGetCount();
            var result = count >= 0 ? new List<T>(count) : new List<T>();
            if (detect == 1)
            {
                // ReSharper disable once PossibleMultipleEnumeration
                foreach (var item in source)
                {
                    token.ThrowIfCancellationRequested();
                    result.Add(item);
                }
                return result;
            }
            else
            {
                // ReSharper disable once PossibleMultipleEnumeration
                using (var itor = source.GetEnumerator())
                {
                    while (true)
                    {
                        token.ThrowIfCancellationRequested();
                        for (var i = 0; i < detect; i++)
                        {
                            if (!itor.MoveNext()) return result;
                            result.Add(itor.Current);
                        }
                    }
                }
            }
        }

        public static List<T> ToList<T>([NotNull] this IEnumerable<T> source, int capacity)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var list = new List<T>(capacity);
            list.AddRange(source);
            return list;
        }

        public static int CopyToArray<T>([NotNull] this IEnumerable<T> source, [NotNull] T[] array, int arrayIndex, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (arrayIndex > array.Length || count > array.Length - arrayIndex) throw new ArgumentException();

            var i = arrayIndex;
            foreach (var item in source.Take(count))
            {
                array[i] = item;
                i++;
            }
            return i - arrayIndex;
        }

        #endregion

        #region split

        public static IEnumerable<IEnumerable<TSource>> SplitChunks<TSource>([NotNull] this IEnumerable<TSource> source, int chunkSize)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (chunkSize < 1) throw new ArgumentOutOfRangeException(nameof(chunkSize), chunkSize, "must > 0.");

            if (chunkSize == 1)
            {
                foreach (var item in source) yield return new[] { item };
            }
            else // chunkSize > 1
            {
                var count = chunkSize - 1; // > 0
                using (var itor = source.GetEnumerator())
                {
                    if (itor.MoveNext()) yield return new[] { itor.Current }.Concat(itor.Take(count));
                }
            }
        }

        public static Tuple<IEnumerable<T>, IEnumerable<T>> Split<T>([NotNull] this IEnumerable<T> source, Func<T, bool> trueToLeft)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (trueToLeft == null) throw new ArgumentNullException(nameof(trueToLeft));

            var array = source as T[] ?? source.ToArray();
            return Tuple.Create(array.Where(trueToLeft), array.Where(z => !trueToLeft(z)));
        }

        #endregion

        #region giveup

        /// <summary>
        /// Skip() will skip left of items, but Giveup() will skip right of items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="giveup"></param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> Giveup<T>([NotNull] this IEnumerable<T> source, uint giveup)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var itorMove = source.GetEnumerator())
            {
                while (giveup-- > 0)
                {
                    if (!itorMove.MoveNext()) yield break;
                }

                using (var itorTake = source.GetEnumerator())
                {
                    while (itorMove.MoveNext() && itorTake.MoveNext())
                    {
                        yield return itorTake.Current;
                    }
                }
            }
        }

        #endregion

        #region Foreach

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var item in source) action(item);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<int, T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            var index = 0;
            foreach (var item in source) action(index++, item);
        }

        #endregion

        #region all or any

        public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, bool continueOnFailed)
        {
            return continueOnFailed
                ? source.Aggregate(true, (current, item) => current & predicate(item))
                : source.All(predicate);
        }

        public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, bool continueOnFailed)
        {
            return continueOnFailed
                ? source.Aggregate(false, (current, item) => current | predicate(item))
                : source.Any(predicate);
        }

        #endregion

        #region orderby

        public static IOrderedEnumerable<T> OrderBy<T>([NotNull]
            this IEnumerable<T> source, IComparer<T> comparer)
            => source.OrderBy(z => z, comparer);

        public static IOrderedEnumerable<T> OrderBy<T>([NotNull]
            this IEnumerable<T> source, Comparison<T> comparison)
            => source.OrderBy(z => z, Comparer<T>.Create(comparison));

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>([NotNull]
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Comparison<TKey> comparison)
            => source.OrderBy(keySelector, Comparer<TKey>.Create(comparison));

        public static IOrderedEnumerable<T> OrderByDescending<T>([NotNull]
            this IEnumerable<T> source, IComparer<T> comparer)
            => source.OrderByDescending(z => z, comparer);

        public static IOrderedEnumerable<T> OrderByDescending<T>([NotNull]
            this IEnumerable<T> source, Comparison<T> comparison)
            => source.OrderByDescending(z => z, Comparer<T>.Create(comparison));

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>([NotNull]
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Comparison<TKey> comparison)
            => source.OrderByDescending(keySelector, Comparer<TKey>.Create(comparison));

        #endregion

        #region random

        public static T RandomTake<T>(this IList<T> t, Random random = null)
        {
            switch (t.Count)
            {
                case 0:
                    return default(T);
                case 1:
                    return t[0];
            }

            random = random ?? Singleton.ThreadStaticInstance<Random>();
            var n = random.Next(t.Count);
            return t[n];
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static T RandomTake<T>(this IEnumerable<T> t, Random random = null)
        {
            var list = t as IList<T>;
            if (list != null) return list.RandomTake(random);

            var c = t.TryGetCount();
            if (c < 0)
            {
                return t.ToArray().RandomTake(random);
            }

            switch (c)
            {
                case 0:
                    return default(T);
                case 1:
                    return t.First();
                default:
                    return t.Skip((random ?? Singleton.ThreadStaticInstance<Random>()).Next(c)).First();
            }
        }

        public static IEnumerable<T> RandomSort<T>(this IEnumerable<T> source, Random random = null)
        {
            return (source as IList<T>)?.RandomSort(random) ??
                   (source as ICollection<T>)?.RandomSort(random) ??
                   (source.ToArray()).RandomSort(random);
        }

        public static IEnumerable<T> RandomSort<T>(this ICollection<T> source, Random random = null)
        {
            var count = source.Count;
            if (count == 0) yield break;
            if (count == 1) yield return source.First();
            var array = Enumerable.Range(0, count).ToArray();
            var cache = new List<T>(count);
            random = random ?? Singleton.ThreadStaticInstance<Random>();
            using (var itor = source.GetEnumerator())
            {
                while (count > 0)
                {
                    var index = random.Next(count);
                    while (index >= cache.Count)
                    {
                        if (!itor.MoveNext()) throw new InvalidOperationException("collection was changed");
                        cache.Add(itor.Current);
                    }
                    yield return cache[array[index]];
                    array[index] = array[count - 1];
                    count--;
                }
            }
        }

        public static IEnumerable<T> RandomSort<T>(this IList<T> source, Random random = null)
        {
            var count = source.Count;
            if (count == 0) yield break;
            if (count == 1) yield return source[0];
            var array = Enumerable.Range(0, count).ToArray();
            random = random ?? Singleton.ThreadStaticInstance<Random>();
            while (count > 0)
            {
                var index = random.Next(count);
                yield return source[array[index]];
                array[index] = array[count - 1];
                count--;
            }
        }

        #endregion

        #region count

        public static int TryGetCount<T>(this IEnumerable<T> source)
            => (source as ICollection<T>)?.Count ??
               (source as ICollection)?.Count ??
               (source as IReadOnlyCollection<T>)?.Count ??
               -1;

        public static int TryGetCount(this IEnumerable source)
            => (source as ICollection)?.Count ?? -1;

        #endregion

        #region modify enumerable

        #region insert

        [PublicAPI]
        public static IEnumerable<T> Insert<T>([NotNull] this IEnumerable<T> source, int index, T item)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var i = 0;
            using (var itor = source.GetEnumerator())
            {
                while (i < index && itor.MoveNext())
                {
                    yield return itor.Current;
                    i++;
                }

                if (i == index)
                {
                    yield return item;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }

                while (itor.MoveNext())
                {
                    yield return itor.Current;
                }
            }
        }

        [PublicAPI]
        public static IEnumerable<T> InsertToEnd<T>([NotNull] IEnumerable<T> source, T next)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            foreach (var item in source) yield return item;
            yield return next;
        }

        [PublicAPI]
        public static IEnumerable<T> InsertToStart<T>([NotNull] IEnumerable<T> source, T next)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            yield return next;
            foreach (var item in source) yield return item;
        }

        #endregion

        #region join

        public static IEnumerable<T> JoinWith<T>(this IEnumerable<T> source, T spliter)
        {
            using (var itor = source.GetEnumerator())
            {
                if (!itor.MoveNext()) yield break;
                while (true)
                {
                    yield return itor.Current;
                    if (!itor.MoveNext()) yield break;
                    yield return spliter;
                }
            }
        }

        public static IEnumerable<T> JoinWith<T>(this IEnumerable<T> source, Func<T> spliterFunc)
        {
            using (var itor = source.GetEnumerator())
            {
                if (!itor.MoveNext()) yield break;
                while (true)
                {
                    yield return itor.Current;
                    if (!itor.MoveNext()) yield break;
                    yield return spliterFunc();
                }
            }
        }

        public static IEnumerable<T> JoinWith<T>(this IEnumerable<T> source, Action action)
        {
            using (var itor = source.GetEnumerator())
            {
                if (!itor.MoveNext()) yield break;
                while (true)
                {
                    yield return itor.Current;
                    if (!itor.MoveNext()) yield break;
                    action();
                }
            }
        }

        #endregion

        #endregion

        #region zip

        public static IEnumerable<Tuple2<TFirst, TSecond>> Zip<TFirst, TSecond>([NotNull] this IEnumerable<TFirst> first,
            [NotNull] IEnumerable<TSecond> second) => first.Zip(second, Tuple2.Create);

        #endregion

        #region for struct

        public static T? FirstOrNull<T>([NotNull] this IEnumerable<T> source, [NotNull] Func<T, bool> predicate)
            where T : struct
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            foreach (var item in source.Where(predicate)) return item;
            return null;
        }

        #endregion
    }
}