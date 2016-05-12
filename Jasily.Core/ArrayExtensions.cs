using System.Collections.Generic;
using JetBrains.Annotations;

namespace System
{
    public static class ArrayExtensions
    {
        public static void CheckArray<T>(this T[] array, int index)
        {
            if (array == null) throw new ArgumentNullException();
            if (index < 0 || index >= array.Length) throw new ArgumentOutOfRangeException();
        }

        public static void CheckArray<T>(this T[] array, int index, int count)
        {
            if (array == null) throw new ArgumentNullException();
            if (index < 0 || index >= array.Length) throw new ArgumentOutOfRangeException();
            if (count < 0) throw new ArgumentOutOfRangeException();
            if (index + count > array.Length) throw new ArgumentOutOfRangeException();
        }

        public static IEnumerable<T> Enumerate<T>(this T[] array, int index)
        {
            array.CheckArray(index);
            for (var i = 0; i < array.Length; i++) yield return array[i + index];
        }

        public static IEnumerable<T> Enumerate<T>(this T[] array, int index, int count)
        {
            array.CheckArray(index, count);
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