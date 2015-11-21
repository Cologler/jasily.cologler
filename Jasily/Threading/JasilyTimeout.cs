using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Jasily.Threading
{
    /// <summary>
    /// alse see: http://referencesource.microsoft.com/#mscorlib/system/threading/SpinWait.cs,b8cdeb634d79d613
    /// 使用算法来构造从生成 JasilyTimeout 开始到将此超时传递给系统 API 的时间。
    /// </summary>
    public class JasilyTimeout
    {
        private readonly uint created;

        private JasilyTimeout(int origin)
        {
            Contract.Assert(origin == Timeout.Infinite || origin > 0);

            this.Value = origin;
            this.created = CurrentTickCount;
        }

        /// <summary>
        /// 大约 49.7 天重置
        /// </summary>
        private static uint CurrentTickCount => (uint)Environment.TickCount;

        public int Value { get; }

        /// <summary>
        /// 返回从 JasilyTimeout 构造开始到当前的毫秒时间
        /// </summary>
        public uint TimeOffset => CurrentTickCount - this.created;

        /// <summary>
        /// 剩下的时间
        /// </summary>
        public int LeftTime
        {
            get
            {
                var offset = this.TimeOffset;
                return offset > this.Value ? 0 : (int)(this.Value - offset);
            }
        }

        public bool IsTimeout => this.TimeOffset >= this.Value;

        public static implicit operator JasilyTimeout(int milliseconds)
        {
            if (milliseconds == Timeout.Infinite)
                return InfiniteTimeSpan;
            if (milliseconds <= 0)
                throw new ArgumentOutOfRangeException(nameof(milliseconds), milliseconds, "value must > 0");
            return new JasilyTimeout(milliseconds);
        }

        public static readonly JasilyTimeout InfiniteTimeSpan
            = new JasilyTimeout(Timeout.Infinite);
    }
}