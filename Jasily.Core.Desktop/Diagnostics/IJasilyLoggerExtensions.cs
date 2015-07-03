using System.Runtime.CompilerServices;

namespace System.Diagnostics
{
    public static class IJasilyLoggerExtensions
    {
        public static void Log<T>(this IJasilyLoggerObject<T> obj,
            JasilyLogger.LoggerMode mode,
            string message,
            JasilyLogger logger = null,
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            (logger ?? JasilyLogger.Current).WriteLine<T>(mode, message, member, line);
        }
    }
}