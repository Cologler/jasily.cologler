using System.Collections.Generic;
using JetBrains.Annotations;

namespace System.Linq
{
    public static class Generater
    {
        public static IEnumerable<int> Repeat(int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            for (var i = 0; i < count; i++) yield return i;
        }

        public static IEnumerable<T> Create<T>(int count) where T : new()
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            for (var i = 0; i < count; i++) yield return new T();
        }

        public static IEnumerable<T> Create<T>([NotNull] Func<T> func, int count)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            for (var i = 0; i < count; i++) yield return func();
        }

        public static IEnumerable<int> Forever()
        {
            while (true) yield return 0;
        }
    }
}