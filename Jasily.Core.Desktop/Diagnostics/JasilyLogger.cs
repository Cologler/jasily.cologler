using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Diagnostics
{
    public class JasilyLogger
    {
        /// <summary>
        /// if don't use this instance, please set to null.
        /// </summary>
        public static JasilyLogger Current { get; set; }

        static JasilyLogger()
        {
            Current = new JasilyLogger();
            Current.WriteLine<JasilyLogger>(LoggerMode.Track, String.Format("logger system start on {0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:mmm")));
        }

#if !DEBUG
        object SyncRoot;
        private List<string> Logs;
#endif

        public JasilyLogger()
        {
#if !DEBUG
            Logs = new List<string>();
#endif
        }

        public void WriteLine<T>(
            LoggerMode mode,
            string message,
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
#if !DEBUG
            if (mode == LoggerMode.Debug) return;
#endif

            this.WriteLine(mode, message, typeof(T), member, line);
        }

        public void WriteLine(
            LoggerMode mode,
            string message,
            Type type,
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
#if !DEBUG
            if (mode == LoggerMode.Debug) return;
#endif

            this.RawWriteLine(Format(type.FullName, mode, message, member, line));
        }

        private static string Format(string typeFullName, LoggerMode mode, string message, string member, int line)
        {
            return String.Format("[{0}] [{1}.{2}] ({3}) {4}",
                Thread.CurrentThread.ManagedThreadId.ToString().PadLeft(2, '0'),
                typeFullName,
                member,
                line,
                message);
        }

        public void RawWriteLine(string message)
        {
#if DEBUG
            Debug.WriteLine(message);
#else
            lock (SyncRoot)
            {
                Logs.Add(String.Concat(DateTime.Now.ToString("HH:mm:ss"), " ", message));
            }
#endif
        }

        public enum LoggerMode
        {
            Debug,

            Release,

            Track
        }
    }
}