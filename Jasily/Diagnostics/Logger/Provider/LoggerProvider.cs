using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Jasily.Diagnostics.Logger.Provider
{
    public abstract class LoggerProvider : IGetKey<string>
    {
        private static int loggerCount;
        private static LoggerProvider @default;
        private static Dictionary<string, LoggerProvider> loggers;

        protected readonly object SyncRoot = new object();

        public void Register()
        {
            if (Interlocked.Increment(ref loggerCount) == 1)
            {
                @default = this;
            }
            else
            {
                if (loggers == null)
                {
                    Interlocked.CompareExchange(ref loggers, new Dictionary<string, LoggerProvider>(), null);
                }

                lock (loggers)
                {
                    if (loggers.Count == 0) loggers.Add(@default);
                    loggers.Add(this);
                }
            }
        }

        public static T GetLogger<T>(string id = null) where T : LoggerProvider
        {
            var logger = InternalGetLogger<T>(true, id);
            if (logger == null) throw new InvalidOperationException();
            return logger;
        }

        public static T TryGetLogger<T>(string id = null) where T : LoggerProvider
            => InternalGetLogger<T>(false, id);

        private static T InternalGetLogger<T>(bool requireUnique, string id) where T : LoggerProvider
        {
            if (loggers != null)
            {
                lock (loggers)
                {
                    if (id != null)
                    {
                        return loggers.GetValueOrDefault(id) as T;
                    }
                    else
                    {
                        return requireUnique
                            ? loggers.Values.OfType<T>().Single()
                            : loggers.Values.OfType<T>().FirstOrDefault();
                    }
                }
            }
            else if (@default != null)
            {
                if (id == null || id == @default.Id) return @default as T;
            }


            return null;
        }

        public abstract void Write(string message);

        public virtual Task WriteAsync(string message)
            => Task.Run(() => this.Write(message));

        public virtual Task ThreadSafeWriteAsync(string message)
        {
            return Task.Run(() =>
            {
                lock (this.SyncRoot)
                {
                    this.Write(message);
                }
            });
        }

        public abstract void WriteLine(string message);

        public virtual Task WriteLineAsync(string message)
            => Task.Run(() => this.WriteLine(message));

        public virtual Task ThreadSafeWriteLineAsync(string message)
        {
            return Task.Run(() =>
            {
                lock (this.SyncRoot)
                {
                    this.WriteLine(message);
                }
            });
        }

        public abstract string Id { get; }

        [Conditional("DEBUG")]
        public void WriteIfDebug(string message) => this.Write(message);

        [Conditional("DEBUG")]
        public void WriteLineIfDebug(string message) => this.WriteLine(message);

        string IGetKey<string>.GetKey() => this.Id;
    }
}