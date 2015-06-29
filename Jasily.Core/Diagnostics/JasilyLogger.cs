using System.Runtime.CompilerServices;

namespace System.Diagnostics
{
    public class JasilyLogger
    {
#if !DEBUG
        static object SyncRoot;
        private static List<string> Logs;
#endif

        public JasilyLogger()
        {
            
        }

        public void WriteLine(LoggerMode mode, string message,
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
#if !DEBUG
            if (mode == UCLoggerMode.Debug) return;
#endif

            //this.RawWriteLine(Format());
        }

        private static string Format(string typeFullName, LoggerMode mode, string message, string member, int line)
        {
            return String.Format("[{0}] [{1}.{2}] ({3}) {4}", /* Thread.CurrentThread.ManagedThreadId.ToString().PadLeft(2, '0') */ "unknown",
                typeFullName, member, line, message);
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