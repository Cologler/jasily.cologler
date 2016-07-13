using System;
using JetBrains.Annotations;

namespace Jasily
{
    public static class StringRangeExtensions
    {
        public static StringRange AsRange([NotNull] this string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            return new StringRange(str);
        }

        public static StringRange SubRange([NotNull] this string str, int startIndex)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            return new StringRange(str).SubRange(startIndex);
        }

        public static StringRange SubRange([NotNull] this string str, int startIndex, int length)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            return new StringRange(str).SubRange(startIndex, length);
        }
    }
}