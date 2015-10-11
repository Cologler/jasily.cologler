using System.Threading.Tasks;

#if WINDOWS_UWP

using Windows.UI.Core;

#endif

namespace System.Windows
{
    public abstract class JasilyDispatcher
    {
        public abstract void Invoke(Action action);

        public abstract Task InvokeAsync(Action action);

        public abstract void BeginInvoke(Action action);

        public abstract bool CheckAccess();

        public static JasilyDispatcher GetUIDispatcher() =>
#if WINDOWS_UWP
        UWPDispatcher.UIDispatcher;
#elif DESKTOP
        DesktopDispatcher.UIDispatcher;
#elif WINDOWS_PHONE_80
        RTDispatcher.UIDispatcher;
#else
        null;
#endif
    }
}