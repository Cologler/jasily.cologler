using JetBrains.Annotations;

namespace System
{
    public static class ArrayExtensions
    {
        public static void CheckRange<T>([NotNull] this T[] array, int startIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (startIndex < 0 || startIndex >= array.Length) throw new ArgumentOutOfRangeException(nameof(startIndex));
        }

        public static void CheckRange<T>([NotNull] this T[] array, int startIndex, int count)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (count < 0 || count > array.Length) throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex < 0 || startIndex >= array.Length) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex + count > array.Length) throw new ArgumentException();
        }

        public static int IndexOf<T>([NotNull] this T[] array, T value) => Array.IndexOf(array, value);

        public static T[] ToArray<T>([NotNull] this T[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            
            var ret = new T[array.Length];
            Array.Copy(array, ret, array.Length);
            return ret;
        }

        public static T[] ToArray<T>([NotNull] this T[] array, int count) => array.ToArray(0, count);

        public static T[] ToArray<T>([NotNull] this T[] array, int startIndex, int count)
        {
            array.CheckRange(startIndex, count);

            count = Math.Min(count, array.Length);
            var ret = new T[count];
            Array.Copy(array, startIndex, ret, 0, count);
            return ret;
        }
    }
}