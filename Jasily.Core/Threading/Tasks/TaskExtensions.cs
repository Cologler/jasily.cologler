using System.Collections.Generic;
using JetBrains.Annotations;

namespace System.Threading.Tasks
{
    public static class TaskExtensions
    {
        public static async Task<IEnumerable<T>> AsTask<T>([NotNull] this IEnumerable<Task<T>> tasks) => await Task.WhenAll(tasks);

        public static Task<TOut> GetAsync<TIn, TOut>([NotNull] this TIn obj, [NotNull] Func<TIn, TOut> getter)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (getter == null) throw new ArgumentNullException(nameof(getter));
            return Task.Run(() => getter(obj));
        }

        public static Task DoAsync<T>([NotNull] this T obj, [NotNull] Action<T> action)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (action == null) throw new ArgumentNullException(nameof(action));
            return Task.Run(() => action(obj));
        }
    }
}
