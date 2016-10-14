using System.Collections.Generic;
using JetBrains.Annotations;

namespace System.Linq
{
    public static partial class EnumerableOverride
    {
        [CanBeNull]
        public static List<T> NullIfEmpty<T>([CanBeNull] this List<T> item)
            => item == null || item.Count == 0 ? null : item;

        public static T RandomTake<T>([NotNull] this List<T> source, [NotNull] Random random, bool throwIfEmpty = false)
            => (source as IList<T>).RandomTake(random, throwIfEmpty);
    }
}