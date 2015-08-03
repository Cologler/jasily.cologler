using System;

namespace Jasily.Diagnostics
{
    public struct JasilyLoggerData
    {
        public DateTime DateTime { get; }

        public int ThreadId { get; }

        public string CallerMemberName { get; }

        public int CallerLineNumber { get; }

        public string Message { get; }

        public string TypeName { get; }

        internal JasilyLoggerData(string message, Type type, string callerMemberName, int callerLineNumber)
        {
            this.DateTime = DateTime.Now;
            this.CallerMemberName = callerMemberName;
            this.CallerLineNumber = callerLineNumber;
            this.Message = message;
            this.TypeName = type.Name;
            this.ThreadId = Environment.CurrentManagedThreadId;
        }

        public string Format(bool withCallerInfo) => withCallerInfo
            ? $"[{this.ThreadId.ToString().PadLeft(2, '0')}] [{this.TypeName}.{this.CallerMemberName}] ({this.CallerLineNumber}) {this.Message}"
            : $"[{this.ThreadId.ToString().PadLeft(2, '0')}] {this.Message}";
    }
}