using JetBrains.Annotations;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class AsyncEnumerableExtensions
    {
        #region to

        public static async Task<T[]> ToArrayAsync<T>(this IEnumerable<T> source)
            => await Task.Run(() => source.ToArray());

        public static async Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TKey, TSource>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
            => await Task.Run(() => source.ToDictionary(keySelector));

        public static async Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TKey, TSource>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer)
            => await Task.Run(() => source.ToDictionary(keySelector, comparer));

        public static async Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TSource, TElement>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
            => await Task.Run(() => source.ToDictionary(keySelector, elementSelector));

        public static async Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TSource, TElement>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
            => await Task.Run(() => source.ToDictionary(keySelector, elementSelector, comparer));

        public static async Task<List<T>> ToListAsync<T>(this IEnumerable<T> source)
            => await Task.Run(() => source.ToList());

        #endregion

        #region combine

        public static async Task<T[]> CombineToArrayAsync<T>([NotNull] this IEnumerable<Task<T>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return await Task.WhenAll(source);
        }
        public static async Task<T[]> CombineToArrayAsync<T>([NotNull] this IEnumerable<Task<T>> source, CancellationToken token)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return (await source.CombineToListAsync(token)).ToArray();
        }

        public static async Task<List<T>> CombineToListAsync<T>([NotNull] this IEnumerable<Task<T>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return (await Task.WhenAll(source)).ToList();
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static async Task<List<T>> CombineToListAsync<T>([NotNull] this IEnumerable<Task<T>> source, CancellationToken token)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var count = source.TryGetCount();
            var result = count < 0 ? new List<T>() : new List<T>(count);
            foreach (var item in source)
            {
                token.ThrowIfCancellationRequested();
                result.Add(await item);
            }
            return result;
        }

        #endregion

        #region all or any

        public static async Task<bool> AllAsync<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate, bool continueOnFailed = false)
            => await Task.Run(() => source.All(predicate, continueOnFailed));

        public static async Task<bool> AllAsync<TSource>(this IEnumerable<TSource> source,
            Func<TSource, Task<bool>> predicateAsync, bool continueOnFailed = false)
        {
            if (continueOnFailed)
            {
                var result = true;
                foreach (var item in source)
                    result &= await predicateAsync(item);
                return result;
            }
            else
            {
                foreach (var item in source)
                {
                    if (!await predicateAsync(item))
                        return false;
                }
                return true;
            }
        }

        public static async Task<bool> AnyAsync<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate, bool continueOnFailed = false)
            => await Task.Run(() => source.Any(predicate, continueOnFailed));

        public static async Task<bool> AnyAsync<TSource>(this IEnumerable<TSource> source,
            Func<TSource, Task<bool>> predicateAsync, bool continueOnFailed = false)
        {
            if (continueOnFailed)
            {
                var result = false;
                foreach (var item in source)
                    result |= await predicateAsync(item);
                return result;
            }
            else
            {
                foreach (var item in source)
                {
                    if (await predicateAsync(item))
                        return true;
                }
                return false;
            }
        }

        #endregion
    }
}