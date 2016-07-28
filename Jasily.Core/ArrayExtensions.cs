using System.Collections.Generic;
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
            if (startIndex < 0 || startIndex >= array.Length) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > array.Length) throw new ArgumentException();
        }

        public static IEnumerable<T> Enumerate<T>(this T[] array, int index, int count)
        {
            array.CheckRange(index, count);
            for (var i = 0; i < count; i++) yield return array[i + index];
        }

        public static int IndexOf<T>([NotNull] this T[] array, T value) => Array.IndexOf(array, value);

        public static T[] ToArray<T>([NotNull] this T[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            var ret = new T[array.Length];
            Array.Copy(array, ret, array.Length);
            return ret;
        }
    }
}