using JetBrains.Annotations;

namespace System.Collections.Generic
{
    public static class Enumerable
    {
        public static void ForEach<T>([NotNull] this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var item in source) action(item);
        }

        public static void ForEach<T>([NotNull] this IEnumerable<T> source, Action<int, T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            var index = 0;
            foreach (var item in source) action(index++, item);
        }
    }
}