using System;
using System.Runtime.CompilerServices;

namespace Jasily.Diagnostics
{
    public static class IJasilyLoggerExtensions
    {
        private static Type SelectType<T>(this IJasilyLoggerObject<T> obj) => typeof(T);
        private static Type SelectType(this IJasilyLoggerObject obj) => obj.GetType();

        public static void LogInfo(this IJasilyLoggerObject obj,
            JasilyLoggerMode mode, string message, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
            => obj.GetLogger().WriteInfo(mode, obj.SelectType(), message, member, line);

        public static void LogInfo<T>(this IJasilyLoggerObject<T> obj,
            JasilyLoggerMode mode, string message, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
            => obj.GetLogger().WriteInfo(mode, obj.SelectType(), message, member, line);

        public static void LogError(this IJasilyLoggerObject obj,
            JasilyLoggerMode mode, string message, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
            => obj.GetLogger().WriteError(mode, obj.SelectType(), message, member, line);

        public static void LogError<T>(this IJasilyLoggerObject<T> obj,
            JasilyLoggerMode mode, string message, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
            => obj.GetLogger().WriteError(mode, obj.SelectType(), message, member, line);

        public static void LogException(this IJasilyLoggerObject obj,
            JasilyLoggerMode mode, Exception e, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
            => obj.GetLogger().WriteException(mode, obj.SelectType(), e, member, line);

        public static void LogException<T>(this IJasilyLoggerObject<T> obj,
            JasilyLoggerMode mode, Exception e, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
            => obj.GetLogger().WriteException(mode, obj.SelectType(), e, member, line);
    }
}