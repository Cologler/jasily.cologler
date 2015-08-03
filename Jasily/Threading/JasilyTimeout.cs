using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Jasily.Threading
{
    /// <summary>
    /// alse see: http://referencesource.microsoft.com/#mscorlib/system/threading/SpinWait.cs,b8cdeb634d79d613
    /// 使用算法来构造从生成 JasilyTimeout 开始到将此超时传递给系统 API 的时间。
    /// </summary>
    public struct JasilyTimeout
    {
        private readonly int originalMillisecondsTimeout;
        private readonly uint created;

        public JasilyTimeout(int originalMillisecondsTimeout)
        {
            Contract.Assert(originalMillisecondsTimeout != Timeout.Infinite);

            this.originalMillisecondsTimeout = originalMillisecondsTimeout;
            this.created = CurrentTickCount;
        }

        private static uint CurrentTickCount => (uint)Environment.TickCount;

        /// <summary>
        /// 返回从 Timeout 减去构造 JasilyTimeout 到调用 Value.Get() 的时间
        /// </summary>
        public int Value
        {
            get
            {
                var elapsedMilliseconds = CurrentTickCount - this.created;
                return elapsedMilliseconds > int.MaxValue
                    ? 0
                    : Math.Max(this.originalMillisecondsTimeout - (int) elapsedMilliseconds, 0);
            }
        }

        public bool IsTimeout => this.Value <= 0;

        public static implicit operator JasilyTimeout(int milliseconds) => new JasilyTimeout(milliseconds);
    }
}