
#if DESKTOP

using System.Windows.Threading;

#endif

namespace System.Windows
{
    public static class DispatcherExtensions
    {
#if DESKTOP
        public static DispatcherOperation BeginInvoke(this Dispatcher dispatcher, Action a)
        {
            return dispatcher.BeginInvoke(a, null);
        }
        public static DispatcherOperation BeginInvoke<T>(this Dispatcher dispatcher, Action<T> a, T arg)
        {
            return dispatcher.BeginInvoke(a, arg);
        }
        public static DispatcherOperation BeginInvoke<T1, T2>(this Dispatcher dispatcher, Action<T1, T2> a, T1 arg1, T2 arg2)
        {
            return dispatcher.BeginInvoke(a, arg1, arg2);
        }

        public static bool CheckAccessOrInvoke(this Dispatcher dispatcher, Action action)
        {
            if (dispatcher.CheckAccess()) return true;

            dispatcher.Invoke(action);

            return false;
        }
        public static bool CheckAccessOrInvoke<T>(this Dispatcher dispatcher, Action<T> action, T t)
        {
            if (dispatcher.CheckAccess()) return true;

            dispatcher.Invoke(action, t);

            return false;
        }
        public static bool CheckAccessOrInvoke<T1, T2>(this Dispatcher dispatcher, Action<T1, T2> action, T1 t1, T2 t2)
        {
            if (dispatcher.CheckAccess()) return true;

            dispatcher.Invoke(action, t1, t2);

            return false;
        }

        public static bool CheckAccessOrBeginInvoke(this Dispatcher dispatcher, Action action)
        {
            if (dispatcher.CheckAccess()) return true;

            dispatcher.BeginInvoke(action);

            return false;
        }
        public static bool CheckAccessOrBeginInvoke<T>(this Dispatcher dispatcher, Action<T> action, T t)
        {
            if (dispatcher.CheckAccess()) return true;

            dispatcher.BeginInvoke(action, t);

            return false;
        }
        public static bool CheckAccessOrBeginInvoke<T1, T2>(this Dispatcher dispatcher, Action<T1, T2> action, T1 t1, T2 t2)
        {
            if (dispatcher.CheckAccess()) return true;

            dispatcher.BeginInvoke(action, t1, t2);

            return false;
        }
#endif

        public static JasilyDispatcher GetUIDispatcher(this object obj) => JasilyDispatcher.GetUIDispatcher();
    }
}
