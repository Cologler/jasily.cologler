using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        #region to

        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source, CancellationToken token)
        {
            return source.ToList(token).ToArray();
        }

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

        public static List<T> ToList<T>(this IEnumerable<T> source, CancellationToken token)
        {
            var collection = source as ICollection;

            var result = collection == null ? new List<T>() : new List<T>(collection.Count);
            foreach (var item in source)
            {
                token.ThrowIfCancellationRequested();
                result.Add(item);
            }
            return result;
        }

        #endregion

        #region split

        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source, int chunkSize)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (chunkSize < 1) throw new ArgumentOutOfRangeException(nameof(chunkSize), chunkSize, "must > 0.");

            var list = new List<TSource>(chunkSize);
            foreach (var item in source)
            {
                list.Add(item);
                if (list.Count == chunkSize)
                {
                    yield return list;
                    list = new List<TSource>(chunkSize);
                }
            }
            if (list.Count != 0) yield return list;
        }

        public static Tuple<IEnumerable<T>, IEnumerable<T>> Split<T>(this IEnumerable<T> source, Func<T, bool> trueToLeft)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (trueToLeft == null) throw new ArgumentNullException(nameof(trueToLeft));

            return Tuple.Create(source.Where(trueToLeft), source.Where(z => !trueToLeft(z)));
        }

        #endregion

        #region giveup

        public static IEnumerable<T> Giveup<T>(this IEnumerable<T> source, int giveup)
        {
            var collection = source as ICollection<T>;
            if (collection != null)
            {
                if (collection.Count <= giveup) yield break;
                foreach (var item in collection.Take(collection.Count - giveup))
                {
                    yield return item;
                }
            }
            else
            {
                var queue = new Queue<T>(giveup);
                using (var itor = source.GetEnumerator())
                {
                    while (giveup > 0)
                    {
                        if (!itor.MoveNext()) yield break;
                        giveup--;
                        queue.Enqueue(itor.Current);
                    }
                    while (itor.MoveNext())
                    {
                        yield return queue.Dequeue();
                        queue.Enqueue(itor.Current);
                    }
                }
            }
        }

        #endregion

        #region Foreach

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var item in source) action(item);
            return source;
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

        public static IOrderedEnumerable<T> OrderBy<T>(
            this IEnumerable<T> source, IComparer<T> comparer)
            => source.OrderBy(z => z, comparer);

        public static IOrderedEnumerable<T> OrderBy<T>(
            this IEnumerable<T> source, Comparison<T> comparison)
            => source.OrderBy(z => z, Comparer<T>.Create(comparison));

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Comparison<TKey> comparison)
            => source.OrderBy(keySelector, Comparer<TKey>.Create(comparison));

        public static IOrderedEnumerable<T> OrderByDescending<T>(
            this IEnumerable<T> source, IComparer<T> comparer)
            => source.OrderByDescending(z => z, comparer);

        public static IOrderedEnumerable<T> OrderByDescending<T>(
            this IEnumerable<T> source, Comparison<T> comparison)
            => source.OrderByDescending(z => z, Comparer<T>.Create(comparison));

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Comparison<TKey> comparison)
            => source.OrderByDescending(keySelector, Comparer<TKey>.Create(comparison));

        #endregion

        #region random

        public static T RandomTake<T>(this IList<T> t, Random random = null)
        {
            if (t.Count == 0) return default(T);
            if (t.Count == 1) return t[0];
            random = random ?? RandomExtensions.RandomNumberGenerator;
            return t[random.Next(t.Count)];
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
            random = random ?? RandomExtensions.RandomNumberGenerator;
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
            random = random ?? RandomExtensions.RandomNumberGenerator;
            while (count > 0)
            {
                var index = random.Next(count);
                yield return source[array[index]];
                array[index] = array[count - 1];
                count--;
            }
        }

        #endregion
    }
}