using JetBrains.Annotations;
using System.Collections.ObjectModel;

namespace System.Collections.Generic
{
    public static class IListExtensions
    {
        public static IReadOnlyList<T> AsReadOnly<T>([NotNull] this IList<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new ReadOnlyCollection<T>(source);
        }

        public static IEnumerable<T> Skip<T>([NotNull] this IList<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            for (var i = count; i < source.Count; i++) yield return source[i];
        }
    }
}