
namespace System.Windows
{
    public static class JasilyDispatcher
    {
#if DESKTOP
        /// <summary>
        /// 用在其上创建了 System.Windows.Threading.Dispatcher 的线程的指定参数异步执行指定委托。
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="a">对采用 args 中指定参数的方法的委托，该委托将被推送到 System.Windows.Threading.Dispatcher 事件队列中。</param>
        /// <returns></returns>
        public static System.Windows.Threading.DispatcherOperation BeginInvoke(this System.Windows.Threading.Dispatcher dispatcher, Action a)
        {
            return dispatcher.BeginInvoke(a, null);
        }
        public static System.Windows.Threading.DispatcherOperation BeginInvoke<T>(this System.Windows.Threading.Dispatcher dispatcher, Action<T> a, T arg)
        {
            return dispatcher.BeginInvoke(a, arg);
        }
        public static System.Windows.Threading.DispatcherOperation BeginInvoke<T1, T2>(this System.Windows.Threading.Dispatcher dispatcher, Action<T1, T2> a, T1 arg1, T2 arg2)
        {
            return dispatcher.BeginInvoke(a, arg1, arg2);
        }

        public static bool CheckAccessOrInvoke(this System.Windows.Threading.Dispatcher dispatcher, Action action)
        {
            if (dispatcher.CheckAccess()) return true;

            dispatcher.Invoke(action);

            return false;
        }
        public static bool CheckAccessOrInvoke<T>(this System.Windows.Threading.Dispatcher dispatcher, Action<T> action, T t)
        {
            if (dispatcher.CheckAccess()) return true;

            dispatcher.Invoke(action, t);

            return false;
        }
        public static bool CheckAccessOrInvoke<T1, T2>(this System.Windows.Threading.Dispatcher dispatcher, Action<T1, T2> action, T1 t1, T2 t2)
        {
            if (dispatcher.CheckAccess()) return true;

            dispatcher.Invoke(action, t1, t2);

            return false;
        }

        public static bool CheckAccessOrBeginInvoke(this System.Windows.Threading.Dispatcher dispatcher, Action action)
        {
            if (dispatcher.CheckAccess()) return true;

            dispatcher.BeginInvoke(action);

            return false;
        }
        public static bool CheckAccessOrBeginInvoke<T>(this System.Windows.Threading.Dispatcher dispatcher, Action<T> action, T t)
        {
            if (dispatcher.CheckAccess()) return true;

            dispatcher.BeginInvoke(action, t);

            return false;
        }
        public static bool CheckAccessOrBeginInvoke<T1, T2>(this System.Windows.Threading.Dispatcher dispatcher, Action<T1, T2> action, T1 t1, T2 t2)
        {
            if (dispatcher.CheckAccess()) return true;

            dispatcher.BeginInvoke(action, t1, t2);

            return false;
        }

        public static System.Windows.Threading.Dispatcher GetUIDispatcher()
        {
            return global::System.Windows.Application.Current.Dispatcher;
        }
        /// <summary>
        /// get UI dispatcher
        /// </summary>
        /// <returns></returns>
        public static System.Windows.Threading.Dispatcher GetUIDispatcher(this object obj)
        {
            return JasilyDispatcher.GetUIDispatcher();
        }
#endif

#if WINDOWS_PHONE_80
        /// <summary>
        /// get UI dispatcher
        /// </summary>
        /// <returns></returns>
        public static System.Windows.Threading.Dispatcher GetDispatcher()
        {
            return Deployment.Current.Dispatcher;
        }
#endif
    }
}
