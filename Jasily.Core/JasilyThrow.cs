
using System.Runtime.CompilerServices;

namespace System
{
    public static class JasilyThrow
    {
        public static T ThrowIfNull<T>(this T obj,
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            if (obj == null)
            {
                var message = $"null reference on file [{file}] at line [{line}]";
                throw new NullReferenceException(message);
            }

            return obj;
        }
    }
}
