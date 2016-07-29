using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace System.Linq
{
    public static class Enumerator
    {
        public static IEnumerable<T> Take<T>([NotNull] this IEnumerator<T> enumerator, int count)
        {
            if (enumerator == null) throw new ArgumentNullException(nameof(enumerator));
            if (count < 1) throw new ArgumentOutOfRangeException(nameof(count), count, "must > 0.");
            return TakeIterator(enumerator, count);
        }

        internal static IEnumerable<T> TakeIterator<T>([NotNull] this IEnumerator<T> enumerator, int count)
        {
            Debug.Assert(enumerator != null && count >= 0);
            while (count > 0 && enumerator.MoveNext())
            {
                yield return enumerator.Current;
                count--;
            }
        }
    }
}