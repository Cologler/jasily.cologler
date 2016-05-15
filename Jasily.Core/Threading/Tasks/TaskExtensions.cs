using System.Collections.Generic;

namespace System.Threading.Tasks
{
    public static class TaskExtensions
    {
        public static async Task<IEnumerable<T>> AsTask<T>(this IEnumerable<Task<T>> source)
        {
            return await Task.WhenAll(source);
        }
    }
}
