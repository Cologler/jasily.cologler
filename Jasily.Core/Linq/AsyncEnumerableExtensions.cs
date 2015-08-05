using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class AsyncEnumerableExtensions
    {
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
                if (!await predicateAsync(item))
                    return false;
            }

            return true;
        }
        public static async Task<bool> AnyAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, Task<bool>> predicateAsync, bool continueOnFailed)
        {
            if (!continueOnFailed) return await source.AnyAsync(predicateAsync);

            var result = false;
            foreach (var item in source)
                result |= await predicateAsync(item);
            return result;
        }
    }
}