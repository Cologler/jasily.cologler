using System;
using System.Threading;
using System.Threading.Tasks;
#if DESKTOP
using System.Windows;
using System.Windows.Threading;
#endif

#if  WINDOWS_UWP
using Windows.UI.Core;
using global::Windows.ApplicationModel.Core;
#endif

namespace Jasily.Threading
{
    public sealed class UISynchronizationContext : SynchronizationContext
    {
        public static async Task<SynchronizationContext> GetForCurrentViewAsync()
        {
            SynchronizationContext sc = null;
#if WINDOWS_UWP
            var dispatcher = (CoreApplication.GetCurrentView() ?? CoreApplication.MainView)?.Dispatcher;
            if (dispatcher == null) throw new NotSupportedException();
            if (dispatcher.HasThreadAccess) return Current;
            await dispatcher.RunAsync(CoreDispatcherPriority.High, () => sc = Current);
            return sc;
#elif DESKTOP
            var dispatcher = Application.Current.Dispatcher;
            if (dispatcher.CheckAccess()) return Current;
            await dispatcher.InvokeAsync(() => sc = Current);
            return sc;
#else
            throw new NotSupportedException();
#endif
        }
    }
}