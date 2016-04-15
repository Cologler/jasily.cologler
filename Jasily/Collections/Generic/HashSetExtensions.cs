using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Collections.Generic
{
    public static class HashSetExtensions
    {
        /// <summary>
        /// O(time): comparer of source is JasilyEqualityComparer ? was O(1) : O(n);
        /// because of struct alway is copy, so we require T is class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <param name="existsItem"></param>
        /// <returns></returns>
        public static bool TryGetExistsItem<T>([NotNull] this HashSet<T> source, T item, out T existsItem) where T : class
        {
            var comparer = source.Comparer;
            if (source.Contains(item))
            {
                var jc = comparer as JasilyEqualityComparer<T>;
                existsItem = jc != null
                    ? (ReferenceEquals(jc.LastCompareItem1, item) ? jc.LastCompareItem2 : jc.LastCompareItem1)
                    : source.First(z => comparer.Equals(z, item));
                return true;
            }
            else
            {
                existsItem = default(T);
                return false;
            }
        }
    }
}