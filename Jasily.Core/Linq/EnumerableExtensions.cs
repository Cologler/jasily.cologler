using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using JetBrains.Annotations;

namespace System.Linq
{
    /// <summary>
    /// extensions for linq Enumerable
    /// </summary>
    public static partial class EnumerableExtensions
    {
        /// <summary>
        /// CancellationToken support for enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <param name="checkCycle"></param>
        /// <returns></returns>
        public static IEnumerable<T> EnumerateWith<T>([NotNull] this IEnumerable<T> source,
            CancellationToken token, uint checkCycle = 30)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (checkCycle == 0) throw new ArgumentOutOfRangeException(nameof(checkCycle));

            return EnumerateWithIterator(source, token, checkCycle);
        }

        private static IEnumerable<T> EnumerateWithIterator<T>([NotNull] this IEnumerable<T> source,
            CancellationToken token, uint checkCycle)
        {
            Debug.Assert(source != null);
            Debug.Assert(checkCycle > 0);

            using (var enumerator = source.GetEnumerator())
            {
                if (checkCycle == 1)
                {
                    while (enumerator.MoveNext())
                    {
                        token.ThrowIfCancellationRequested();
                        yield return enumerator.Current;
                    }
                }
                else
                {
                    while (true)
                    {
                        token.ThrowIfCancellationRequested();
                        for (var i = 0u; i < checkCycle; i++)
                        {
                            if (enumerator.MoveNext()) yield return enumerator.Current;
                            else yield break;
                        }
                    }
                }
            }
        }

        #region to

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
                    while (itor.MoveNext()) yield return new[] { itor.Current }.Concat(itor.TakeIterator(count));
                }
            }
        }

        /// <summary>
        /// use selector to split source to two enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static Tuple<IEnumerable<T>, IEnumerable<T>> Split<T>([NotNull] this IEnumerable<T> source, Func<T, bool> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var left = new List<T>();
            var right = new List<T>();

            foreach (var item in source)
            {
                if (selector(item))
                {
                    left.Add(item);
                }
                else
                {
                    right.Add(item);
                }
            }
            return Tuple.Create<IEnumerable<T>, IEnumerable<T>>(left.AsReadOnly(), right.AsReadOnly());
        }

        #endregion

        #region giveup

        /// <summary>
        /// Skip() will skip left of items, but Giveup() will skip right of items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> GiveUp<T>([NotNull] this IEnumerable<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            var collection = source as ICollection<T>;
            if (collection != null)
            {
                return count > collection.Count ? Enumerable.Empty<T>() : collection.Take(collection.Count - count);
            }

            return GiveUpIterator(source, count);
        }

        private static IEnumerable<T> GiveUpIterator<T>([NotNull] IEnumerable<T> source, int count)
        {
            Debug.Assert(source != null);

            using (var mover = source.GetEnumerator())
            {
                while (count-- > 0)
                {
                    if (!mover.MoveNext()) yield break;
                }

                using (var taker = source.GetEnumerator())
                {
                    while (mover.MoveNext() && taker.MoveNext())
                    {
                        yield return taker.Current;
                    }
                }
            }
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

        public static IOrderedEnumerable<T> OrderBy<T>([NotNull] this IEnumerable<T> source, IComparer<T> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.OrderBy(z => z, comparer);
        }

        public static IOrderedEnumerable<T> OrderBy<T>([NotNull] this IEnumerable<T> source,
            [NotNull] Comparison<T> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (comparison == null) throw new ArgumentNullException(nameof(comparison));
            return source.OrderBy(z => z, Comparer<T>.Create(comparison));
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>([NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector, [NotNull] Comparison<TKey> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (comparison == null) throw new ArgumentNullException(nameof(comparison));
            return source.OrderBy(keySelector, Comparer<TKey>.Create(comparison));
        }

        public static IOrderedEnumerable<T> OrderByDescending<T>([NotNull] this IEnumerable<T> source,
            [NotNull] IComparer<T> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            return source.OrderByDescending(z => z, comparer);
        }

        public static IOrderedEnumerable<T> OrderByDescending<T>([NotNull] this IEnumerable<T> source,
            [NotNull] Comparison<T> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (comparison == null) throw new ArgumentNullException(nameof(comparison));
            return source.OrderByDescending(z => z, Comparer<T>.Create(comparison));
        }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(
            [NotNull] this IEnumerable<TSource> source, [NotNull] Func<TSource, TKey> keySelector,
            [NotNull] Comparison<TKey> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (comparison == null) throw new ArgumentNullException(nameof(comparison));
            return source.OrderByDescending(keySelector, Comparer<TKey>.Create(comparison));
        }

        #endregion

        #region random

        public static T RandomTake<T>([NotNull] this List<T> source, Random random = null)
            => RandomTake(source as IList<T>, random);

        public static T RandomTake<T>([NotNull] this IList<T> source, Random random = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            switch (source.Count)
            {
                case 0: return default(T);
                case 1: return source[0];
            }
            random = random ?? Singleton.ThreadStaticInstance<Random>();
            return source[random.Next(source.Count)];
        }

        public static T RandomTake<T>([NotNull] this IReadOnlyList<T> source, Random random = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            switch (source.Count)
            {
                case 0: return default(T);
                case 1: return source[0];
            }
            random = random ?? Singleton.ThreadStaticInstance<Random>();
            return source[random.Next(source.Count)];
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static T RandomTake<T>([NotNull] this IEnumerable<T> source, Random random = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var list = source as IList<T>;
            if (list != null) return list.RandomTake(random);

            var rList = source as IReadOnlyList<T>;
            if (rList != null) return rList.RandomTake(random);

            var c = source.TryGetCount();
            if (c < 0)
            {
                return ((IList<T>)source.ToArray()).RandomTake(random);
            }

            switch (c)
            {
                case 0:
                    return default(T);
                case 1:
                    return source.First();
                default:
                    return source.Skip((random ?? Singleton.ThreadStaticInstance<Random>()).Next(c)).First();
            }
        }

        public static IEnumerable<T> RandomSort<T>([NotNull] this IEnumerable<T> source, Random random = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return (source as IList<T>)?.RandomSort(random) ??
                   (source as ICollection<T>)?.RandomSort(random) ??
                   (source.ToArray()).RandomSort(random);
        }

        public static IEnumerable<T> RandomSort<T>([NotNull] this ICollection<T> source, Random random = null)
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

        public static IEnumerable<T> RandomSort<T>([NotNull] this IList<T> source, Random random = null)
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

        public static int TryGetCount<T>([NotNull] this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return (source as ICollection<T>)?.Count ??
                   (source as ICollection)?.Count ??
                   (source as IReadOnlyCollection<T>)?.Count ?? -1;
        }

        public static int TryGetCount([NotNull] this IEnumerable source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return (source as ICollection)?.Count ?? -1;
        }

        public static int Count([NotNull] this IEnumerable source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var collection = source as ICollection;
            if (collection != null) return collection.Count;

            var count = 0;
            var itor = source.GetEnumerator();
            while (itor.MoveNext()) count++;
            return count;
        }

        public static long LongCount([NotNull] this IEnumerable source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var collection = source as ICollection;
            if (collection != null) return collection.Count;

            var count = 0L;
            var itor = source.GetEnumerator();
            while (itor.MoveNext()) count++;
            return count;
        }

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

        #region min & max

        public static T MaxOrDefault<T>([NotNull] IEnumerable<T> source, T @default) where T : IComparable<T>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var itor = source.GetEnumerator())
            {
                if (!itor.MoveNext()) return @default;
                var item = itor.Current;
                while (itor.MoveNext())
                {
                    item = item.Max(itor.Current);
                }
                return item;
            }
        }

        public static T MinOrDefault<T>([NotNull] IEnumerable<T> source, T @default) where T : IComparable<T>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var itor = source.GetEnumerator())
            {
                if (!itor.MoveNext()) return @default;
                var item = itor.Current;
                while (itor.MoveNext())
                {
                    item = item.Min(itor.Current);
                }
                return item;
            }
        }

        #endregion

        #region sum

        public static long LongSum([NotNull] this IEnumerable<int> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.Aggregate(0L, (current, item) => current + item);
        }

        #endregion

        #region ignore Exception

        /// <summary>
        /// ignore some Exception from MoveNext() of Enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> Ignore<T, TException>([NotNull] this IEnumerable<T> source)
            where TException : Exception
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var itor = source.GetEnumerator())
            {
                while (true)
                {
                    bool moveNext;
                    try
                    {
                        moveNext = itor.MoveNext();
                    }
                    catch (TException)
                    {
                        continue;
                    }
                    if (moveNext)
                    {
                        yield return itor.Current;
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
        }

        /// <summary>
        /// ignore some Exception from MoveNext() of Enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="source"></param>
        /// <param name="exceptionFilter"></param>
        /// <returns></returns>
        public static IEnumerable<T> Ignore<T, TException>([NotNull] this IEnumerable<T> source,
            [NotNull] Func<TException, bool> exceptionFilter)
            where TException : Exception
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (exceptionFilter == null) throw new ArgumentNullException(nameof(exceptionFilter));

            using (var itor = source.GetEnumerator())
            {
                while (true)
                {
                    bool moveNext;
                    try
                    {
                        moveNext = itor.MoveNext();
                    }
                    catch (TException error) when (exceptionFilter(error))
                    {
                        continue;
                    }
                    if (moveNext)
                    {
                        yield return itor.Current;
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
        }

        #endregion

        public static T Index<T>([NotNull] this IEnumerable<T> source, int index)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            var list = source as IList<T>;
            if (list != null)
            {
                if (list.Count <= index) throw new IndexOutOfRangeException();
                return list[index];
            }

            var rolist = source as IReadOnlyList<T>;
            if (rolist != null)
            {
                if (rolist.Count <= index) throw new IndexOutOfRangeException();
                return rolist[index];
            }

            using (var itor = source.Skip(index).GetEnumerator())
            {
                if (!itor.MoveNext()) throw new IndexOutOfRangeException();
                return itor.Current;
            }
        }

        public static object GetOrCreateSyncRoot<T>(this IEnumerable<T> enumerable)
            => (enumerable as ICollection)?.SyncRoot ?? new object();

        public static EnumerableHelper<T> As2<T>([CanBeNull] this IEnumerable<T> source) => new EnumerableHelper<T>(source);

        /// <summary>
        /// let static IEnumerable&lt;T&gt;.Func&lt;T, T2&gt;(this IEnumerable&lt;T&gt; source) => 
        /// IEnumerable&lt;T&gt;.Func&lt;T2&gt;()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public struct EnumerableHelper<T>
        {
            private readonly IEnumerable<T> baseEnumerable;

            public EnumerableHelper([CanBeNull] IEnumerable<T> baseEnumerable)
            {
                // can be null if next func accept null argument.
                this.baseEnumerable = baseEnumerable;
            }

            public IEnumerable<T> Ignore<TException>() where TException : Exception
                => this.baseEnumerable.Ignore<T, TException>();

            public IEnumerable<T> Ignore<TException>([NotNull] Func<TException, bool> exceptionFilter)
                where TException : Exception
                => this.baseEnumerable.Ignore(exceptionFilter);
        }
    }
}