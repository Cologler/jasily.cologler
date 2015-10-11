using System.Threading.Tasks;
using System.Windows.Threading;

namespace System.Windows
{
    internal sealed class RTDispatcher : JasilyDispatcher
    {
        public static RTDispatcher UIDispatcher { get; }
            = new RTDispatcher(Deployment.Current.Dispatcher);

        public Dispatcher Dispatcher { get; }

        public RTDispatcher(Dispatcher dispatcher)
        {
            this.Dispatcher = dispatcher;
        }

        public override void Invoke(Action action)
        {
            throw new NotSupportedException();
        }

        public override Task InvokeAsync(Action action)
        {
            throw new NotSupportedException();
        }

        public override void BeginInvoke(Action action) => this.Dispatcher.BeginInvoke(action);

        public override bool CheckAccess() => this.Dispatcher.CheckAccess();
    }
}