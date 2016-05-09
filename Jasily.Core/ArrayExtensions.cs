using System.Collections.Generic;

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

        public static IEnumerable<IndexValuePair<T>> EnumerateIndexValuePair<T>(this T[] array, int index)
        {
            array.CheckArray(index);
            for (var i = index; i < array.Length; i++) yield return IndexValuePair.From(array, i);
        }

        public static IEnumerable<IndexValuePair<T>> EnumerateIndexValuePair<T>(this T[] array, int index, int count)
        {
            array.CheckArray(index, count);
            for (var i = 0; i < count; i++) yield return IndexValuePair.From(array, i + index);
        }
    }
}