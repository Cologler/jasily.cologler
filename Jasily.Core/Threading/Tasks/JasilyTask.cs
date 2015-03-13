using System.Collections.Generic;

namespace System.Threading.Tasks
{
    public static class JasilyTask
    {
        public static async Task<IEnumerable<T>> AsTask<T>(this IEnumerable<Task<T>> source)
        {
            return await Task.WhenAll(source);
        }
    }
}
