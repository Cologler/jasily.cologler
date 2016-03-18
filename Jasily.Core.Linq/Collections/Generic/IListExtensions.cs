using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace System.Collections.Generic
{
    public static class IListExtensions
    {
        public static IReadOnlyList<T> AsReadOnly<T>([NotNull] this IList<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new ReadOnlyCollection<T>(source);
        }
    }
}