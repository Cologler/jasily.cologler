using System.Collections.Generic;
using JetBrains.Annotations;

namespace System.Linq
{
    public static class EnumeratorExtensions
    {
        public static IEnumerable<T> Take<T>([NotNull] this IEnumerator<T> itor, int count)
        {
            if (itor == null) throw new ArgumentNullException(nameof(itor));
            if (count < 1) throw new ArgumentOutOfRangeException(nameof(count), count, "must > 0.");

            while (count > 0 && itor.MoveNext())
            {
                yield return itor.Current;
                count--;
            }
        }
    }
}