using JetBrains.Annotations;

namespace System.Linq
{
    public static partial class EnumerableOverride
    {
        [NotNull]
        public static string EmptyIfNull([CanBeNull] this string str) => str ?? string.Empty;

        [CanBeNull]
        public static string NullIfEmpty<T>([CanBeNull] this string str) => string.IsNullOrEmpty(str) ? null : str;
    }
}