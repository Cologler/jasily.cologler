using System.Collections.Generic;
using JetBrains.Annotations;

namespace System.Linq
{
    public static class ListExtensions
    {
        public static IEnumerable<T> Skip<T>([NotNull] this IList<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            for (var i = count; i < source.Count; i++) yield return source[i];
        }
    }
}