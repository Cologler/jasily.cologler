using System.Threading.Tasks;
using Windows.UI.Core;

namespace System.Windows
{
    public sealed class UWPDispatcher : JasilyDispatcher
    {
        public static UWPDispatcher UIDispatcher { get; }
            = new UWPDispatcher(global::Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher);

        public CoreDispatcher Dispatcher { get; }

        public UWPDispatcher(CoreDispatcher dispatcher)
        {
            this.Dispatcher = dispatcher;
        }

        public override async Task InvokeAsync(Action action)
            => await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(action));

        public override async void BeginInvoke(Action action)
            => await this.InvokeAsync(action);

        public override bool CheckAccess() => this.Dispatcher.HasThreadAccess;
    }
}