using System.Threading.Tasks;
using System.Windows.Threading;

namespace System.Windows
{
    internal sealed class DesktopDispatcher : JasilyDispatcher
    {
        public static DesktopDispatcher UIDispatcher { get; } = new DesktopDispatcher(Application.Current.Dispatcher);

        public Dispatcher Dispatcher { get; }

        public DesktopDispatcher(Dispatcher dispatcher)
        {
            this.Dispatcher = dispatcher;
        }

        public override void Invoke(Action action) => this.Dispatcher.Invoke(action);

        public override async Task InvokeAsync(Action action) => await this.Dispatcher.InvokeAsync(action);

        public override void BeginInvoke(Action action) => this.Dispatcher.BeginInvoke(action);

        public override bool CheckAccess() => this.Dispatcher.CheckAccess();
    }
}