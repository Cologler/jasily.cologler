using JetBrains.Annotations;

namespace System.Linq
{
    public static partial class EnumerableOverride
    {
        [NotNull]
        public static T[] EmptyIfNull<T>([CanBeNull] this T[] array)
            => array ?? Empty<T>.Array;

        [CanBeNull]
        public static T[] NullIfEmpty<T>([CanBeNull] this T[] item)
            => item == null || item.Length == 0 ? null : item;
    }
}