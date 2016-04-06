using JetBrains.Annotations;

namespace System.Text
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder Append([NotNull] this StringBuilder builder, [NotNull] string value, int startIndex)
        {
            if (value.Length < startIndex) throw new ArgumentOutOfRangeException(nameof(startIndex));
            return value.Length == startIndex ? builder : builder.Append(value, startIndex, value.Length - startIndex);
        }
    }
}