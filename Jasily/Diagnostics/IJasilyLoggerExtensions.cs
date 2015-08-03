using System.Runtime.CompilerServices;

namespace Jasily.Diagnostics
{
    public static class IJasilyLoggerExtensions
    {
        public static void Log<T>(this IJasilyLoggerObject<T> obj,
            JasilyLoggerMode mode,
            string message,
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            obj.GetLogger().WriteLine<T>(mode, message, member, line);
        }
    }
}