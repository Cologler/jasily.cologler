using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Jasily.Diagnostics
{
    public class JasilyLogger : IJasilyLoggerObject<JasilyLogger>
    {
#if !DEBUG
        object SyncRoot;
        private List<JasilyLoggerData> logs;
#endif
        private static int loggerCount;

        private int LoggerId { get; }

        public event EventHandler<JasilyLoggerData> RealTimeTrackEvent;

        public JasilyLogger()
        {
            this.LoggerId = Interlocked.Increment(ref loggerCount);
            this.LogInfo(JasilyLoggerMode.Track, $"logger system {this.LoggerId} start on {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:mmm")}");
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
            return ((byte)value & (byte)flag) == (byte)flag;
        }

        public void WriteInfo<T>(JasilyLoggerMode mode, string message,
            [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
            => this.WriteInfo(mode, typeof(T), message, member, line);

        public void WriteInfo(JasilyLoggerMode mode, Type type, string message,
            [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
            => this.WriteLine(mode, type, "INFO", message, member, line);

        public void WriteError<T>(JasilyLoggerMode mode, string message,
            [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
            => this.WriteError(mode, typeof(T), message, member, line);

        public void WriteError(JasilyLoggerMode mode, Type type, string message,
            [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
            => this.WriteLine(mode, type, "ERROR", message, member, line);

        public void WriteException<T>(JasilyLoggerMode mode, Exception e,
            [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
            => this.WriteException(mode, typeof(T), e, member, line);

        public void WriteException(JasilyLoggerMode mode, Type type, Exception e,
            [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            if (this.NeedLog(mode))
            {
                do
                {
                    this.WriteLine(mode, type, "THROW", FormatException(e), member, line);
                } while ((e = e.InnerException) != null);
            }
        }

        private static string FormatException(Exception e) => e.ToString();

        private void WriteLine(JasilyLoggerMode mode, Type type, string messageType, string message, string member, int line)
        {
            if (this.NeedLog(mode))
            {
                this.RawOutput(mode, new JasilyLoggerData(messageType, message, type, member, line));
            }
        }

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

        JasilyLogger IJasilyLoggerObject.GetLogger() => this;
    }
}