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

        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, IComparer<T> comparer)
        {
            return source.OrderBy(z => z, comparer);
        }
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, Comparison<T> comparison)
        {
            return source.OrderBy(z => z, Comparer<T>.Create(comparison));
        }
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Comparison<TKey> comparison)
        {
            return source.OrderBy(keySelector, Comparer<TKey>.Create(comparison));
        }

        public static IOrderedEnumerable<T> OrderByDescending<T>(this IEnumerable<T> source, IComparer<T> comparer)
        {
            return source.OrderByDescending(z => z, comparer);
        }
        public static IOrderedEnumerable<T> OrderByDescending<T>(this IEnumerable<T> source, Comparison<T> comparison)
        {
            return source.OrderByDescending(z => z, Comparer<T>.Create(comparison));
        }
        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Comparison<TKey> comparison)
        {
            return source.OrderByDescending(keySelector, Comparer<TKey>.Create(comparison));
        }

        #endregion 
    }
}