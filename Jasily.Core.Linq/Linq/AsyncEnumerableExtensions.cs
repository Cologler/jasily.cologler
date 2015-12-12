using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class AsyncEnumerableExtensions
    {
        #region to

        public static async Task<T[]> ToArrayAsync<T>(this IEnumerable<Task<T>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return await Task.WhenAll(source);
        }
        public static async Task<T[]> ToArrayAsync<T>(this IEnumerable<Task<T>> source, CancellationToken token)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return (await source.ToListAsync(token)).ToArray();
        }

        public static async Task<List<T>> ToListAsync<T>(this IEnumerable<Task<T>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var collection = source as ICollection;
            var result = collection == null ? new List<T>() : new List<T>(collection.Count);
            foreach (var item in source)
            {
                result.Add(await item);
            }
            return result;
        }
        public static async Task<List<T>> ToListAsync<T>(this IEnumerable<Task<T>> source, CancellationToken token)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var collection = source as ICollection;
            var result = collection == null ? new List<T>() : new List<T>(collection.Count);
            foreach (var item in source)
            {
                token.ThrowIfCancellationRequested();
                result.Add(await item);
            }
            return result;
        }

        #endregion

        #region all or any

        public static async Task<bool> AllAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, Task<bool>> predicateAsync)
        {
            foreach (var item in source)
            {
                if (!await predicateAsync(item))
                    return false;
            }

            return true;
        }
        public static async Task<bool> AllAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, Task<bool>> predicateAsync, bool continueOnFailed)
        {
            if (!continueOnFailed) return await source.AllAsync(predicateAsync);

            var result = true;
            foreach (var item in source)
                result &= await predicateAsync(item);
            return result;
        }

        public static async Task<bool> AnyAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, Task<bool>> predicateAsync)
        {
            foreach (var item in source)
            {
                if (await predicateAsync(item))
                    return true;
            }

            return false;
        }
        public static async Task<bool> AnyAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, Task<bool>> predicateAsync, bool continueOnFailed)
        {
            if (!continueOnFailed) return await source.AnyAsync(predicateAsync);

            var result = false;
            foreach (var item in source)
                result |= await predicateAsync(item);
            return result;
        }

        #endregion
    }
}