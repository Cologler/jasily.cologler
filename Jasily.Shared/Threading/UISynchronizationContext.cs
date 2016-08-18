using System;
using System.Threading;

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
    public partial class UISynchronizationContext : SynchronizationContext
    {
        public static SynchronizationContext GetForCurrentView()
        {
#if DESKTOP || WINDOWS_UWP
            return new UISynchronizationContext();
#endif
            throw new NotImplementedException();
        }
    }

#if DESKTOP
    public partial class UISynchronizationContext
    {
        private readonly Dispatcher dispatcher;

        public UISynchronizationContext()
        {
            this.dispatcher = Application.Current.Dispatcher;
            if (this.dispatcher == null) throw new NotSupportedException();
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            if (d == null) throw new ArgumentNullException(nameof(d));
            this.dispatcher.InvokeAsync(() => d(state));
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            if (d == null) throw new ArgumentNullException(nameof(d));
            if (this.dispatcher.CheckAccess())
            {
                d(state);
            }
            else
            {
                this.dispatcher.Invoke(() => d(state));
            }
        }
    }
#endif

#if WINDOWS_UWP
    public partial class UISynchronizationContext
    {
        private readonly CoreDispatcher dispatcher;

        public UISynchronizationContext()
        {
            this.dispatcher = (CoreApplication.GetCurrentView() ?? CoreApplication.MainView)?.Dispatcher;
            if (this.dispatcher == null) throw new NotSupportedException();
        }

        public UISynchronizationContext(CoreDispatcher dispatcher)
        {
            if (dispatcher == null) throw new ArgumentNullException(nameof(dispatcher));
            this.dispatcher = dispatcher;
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            if (d == null) throw new ArgumentNullException(nameof(d));
            this.dispatcher?.RunAsync(CoreDispatcherPriority.Normal, () => d(state));
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            if (d == null) throw new ArgumentNullException(nameof(d));
            if (this.dispatcher.HasThreadAccess)
            {
                d(state);
            }
            else
            {
                this.dispatcher?.RunAsync(CoreDispatcherPriority.High, () => d(state)).GetAwaiter().GetResult();
            }
        }
    }
#endif
}