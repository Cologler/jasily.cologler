using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Jasily
{
    public static class JasilyNullable
    {
        /// <summary>
        /// faster then Nullable.Compare()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <returns></returns>
        public static int Compare<T>(T? n1, T? n2) where T : struct, IComparable<T>
        {
            if (n1.HasValue)
            {
                return n2.HasValue ? n1.Value.CompareTo(n2.Value) : 1;
            }
            if (n2.HasValue) return -1;
            return 0;
        }

        public static int Compare<T>(T? n1, T? n2, [NotNull] IComparer<T> comparer) where T : struct
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            if (n1.HasValue)
            {
                return n2.HasValue ? comparer.Compare(n1.Value, n2.Value) : 1;
            }
            if (n2.HasValue) return -1;
            return 0;
        }

        public static int Compare<T>(T? n1, T? n2, [NotNull] Comparison<T> comparison) where T : struct
        {
            if (comparison == null) throw new ArgumentNullException(nameof(comparison));

            if (n1.HasValue)
            {
                return n2.HasValue ? comparison(n1.Value, n2.Value) : 1;
            }
            if (n2.HasValue) return -1;
            return 0;
        }
    }
}