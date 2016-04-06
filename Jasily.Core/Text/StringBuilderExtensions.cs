using JetBrains.Annotations;

namespace System.Text
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder Append([NotNull] this StringBuilder builder, string value, int startIndex)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (value == null) return builder;
            if (value.Length < startIndex) throw new ArgumentOutOfRangeException(nameof(startIndex));
            return value.Length == startIndex ? builder : builder.Append(value, startIndex, value.Length - startIndex);
        }
    }
}