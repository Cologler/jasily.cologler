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
        /// ֪ͨ�۲��ߣ��ṩ��������ɷ��ͻ������͵�֪ͨ��
        /// </summary>
        void IObserver<long>.OnCompleted()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ֪ͨ�۲��ߣ��ṩ�����������������
        /// </summary>
        /// <param name="error">һ���ṩ�йش���ĸ�����Ϣ�Ķ���</param>
        void IObserver<long>.OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ��۲����ṩ�����ݡ�
        /// </summary>
        /// <param name="value">��ǰ��֪ͨ��Ϣ��</param>
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