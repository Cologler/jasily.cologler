using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using static System.String;

namespace Jasily.Diagnostics
{
    public class JasilyLogger : IJasilyLoggerObject<JasilyLogger>
    {
#if !DEBUG
        object syncRoot;
        private List<JasilyLoggerData> logs;
#endif
        private static int loggerCount;

        private int LoggerId { get; }

        public event EventHandler<JasilyLoggerData> RealTimeTrackEvent;

        public JasilyLogger()
        {
            this.LoggerId = Interlocked.Increment(ref loggerCount);
            this.WriteLine<JasilyLogger>(JasilyLoggerMode.Track, $"logger system {this.LoggerId} start on {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:mmm")}");
        }

        private bool NeedLog(JasilyLoggerMode mode)
        {
            if (mode == JasilyLoggerMode.NotLog) return false;
#if DEBUG
            if (HasFlag(mode, JasilyLoggerMode.Release)) return false;
#else
            if (HasFlag(mode, JasilyLoggerMode.Debug)) return false;
#endif
            return true;
        }

        private static bool HasFlag(JasilyLoggerMode value, JasilyLoggerMode flag)
        {
            return ((byte) value & (byte) flag) == (byte) flag;
        }

        public void WriteLine<T>(JasilyLoggerMode mode, string message,
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            if (this.NeedLog(mode))
            {
                this.RawOutput(mode, new JasilyLoggerData(message, typeof(T), member, line));
            }
        }

        public void WriteLine(JasilyLoggerMode mode, Type type, string message,
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            if (this.NeedLog(mode))
            {
                this.RawOutput(mode, new JasilyLoggerData(message, type, member, line));
            }
        }

        public static string GetCallerFilePath([CallerFilePath] string path = "") => path;

        private void RawOutput(JasilyLoggerMode mode, JasilyLoggerData data)
        {
            var msg = data.Format(true);

#if DEBUG
            Debug.WriteLine(msg);
#else
            Debug.WriteLine(Concat(data.DateTime.ToString("HH:mm:ss"), " ", msg));
#endif

            if (HasFlag(mode, JasilyLoggerMode.RealTimeTrack))
                this.RealTimeTrackEvent?.Invoke(this, data);
        }

        JasilyLogger IJasilyLoggerObject<JasilyLogger>.GetLogger() => this;
    }
}