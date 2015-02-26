using System.Windows.Threading;

namespace System.Windows
{
    public static class JasilyDispatcher
    {
#if WPF
        /// <summary>
        /// 用在其上创建了 System.Windows.Threading.Dispatcher 的线程的指定参数异步执行指定委托。
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="a">对采用 args 中指定参数的方法的委托，该委托将被推送到 System.Windows.Threading.Dispatcher 事件队列中。</param>
        /// <returns></returns>
        public static DispatcherOperation BeginInvoke(this Dispatcher dispatcher, Action a)
        {
            return dispatcher.BeginInvoke(a, null);
        }
#endif
    }
}
