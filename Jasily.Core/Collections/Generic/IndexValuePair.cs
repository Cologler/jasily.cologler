﻿using System.Linq;
using JetBrains.Annotations;

namespace System.Collections.Generic
{
    public static class IndexValuePair
    {
        public static IndexValuePair<T> From<T>([NotNull] IList<T> list, int index)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            return new IndexValuePair<T>(index, list[index]);
        }

        public static IndexValuePair<T> From<T>(int index, T value) => new IndexValuePair<T>(index, value);

        public static IEnumerable<IndexValuePair<T>> EnumerateIndexValuePair<T>(this T[] array, int startIndex = 0)
        {
            array.CheckArray(startIndex);
            for (var i = startIndex; i < array.Length; i++) yield return From(array, i);
        }

        public static IEnumerable<IndexValuePair<T>> EnumerateIndexValuePair<T>(this T[] array, int startIndex, int count)
        {
            array.CheckArray(startIndex, count);
            for (var i = 0; i < count; i++) yield return From(array, i + startIndex);
        }

        public static IEnumerable<IndexValuePair<T>> EnumerateIndexValuePair<T>(this IList<T> list, int startIndex = 0)
        {
            for (var i = startIndex; i < list.Count; i++) yield return From(list, i);
        }

        public static IEnumerable<IndexValuePair<T>> EnumerateIndexValuePair<T>(this IList<T> source, int startIndex, int count)
        {
            for (var i = 0; i < count; i++) yield return From(source, i + startIndex);
        }

        public static IEnumerable<IndexValuePair<T>> EnumerateIndexValuePair<T>(this IEnumerable<T> list, int startIndex = 0)
            => list.Skip(startIndex).Select((z, i) => From(i + startIndex, z));

        public static IEnumerable<IndexValuePair<T>> EnumerateIndexValuePair<T>(this IEnumerable<T> source, int startIndex, int count)
            => source.Skip(startIndex).Take(count).Select((z, i) => From(i + startIndex, z));
    }

    public struct IndexValuePair<T>
    {
        public IndexValuePair(int index, T value)
        {
            this.Index = index;
            this.Value = value;
        }

        public int Index { get; }

        public T Value { get; }
    }
}