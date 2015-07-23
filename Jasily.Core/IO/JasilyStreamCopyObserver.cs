using System.EventArgses;

namespace System.IO
{
    public class JasilyStreamCopyObserver : IObserver<long>, IDisposable
    {
        public event EventHandler<JasilyProgressChangedEventArgs> ProgressChanged;

        public JasilyStreamCopyObserver(long total)
        {
            this.Total = total;
        }

        public long Total { get; }

        /// <summary>
        /// 通知观察者，提供程序已完成发送基于推送的通知。
        /// </summary>
        void IObserver<long>.OnCompleted()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 通知观察者，提供程序遇到错误情况。
        /// </summary>
        /// <param name="error">一个提供有关错误的附加信息的对象。</param>
        void IObserver<long>.OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 向观察者提供新数据。
        /// </summary>
        /// <param name="value">当前的通知信息。</param>
        void IObserver<long>.OnNext(long value)
        {
            this.ProgressChanged.Fire(this, new JasilyProgressChangedEventArgs(value, this.Total));
        }

        /// <summary>
        /// only clear event handler. T_T
        /// </summary>
        public void Dispose()
        {
            this.ProgressChanged = null;
        }
    }
}