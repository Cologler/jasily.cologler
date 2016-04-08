using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;

namespace Jasily
{
    public static class DeferralExtensions
    {
        public static IDisposableDeferral<BackgroundTaskDeferral> AsDisposable(this BackgroundTaskDeferral deferral)
        {
            if (deferral == null) throw new ArgumentNullException(nameof(deferral));

            return new BackgroundTaskDeferralDisposable(deferral);
        }

        private sealed class BackgroundTaskDeferralDisposable : IDisposableDeferral<BackgroundTaskDeferral>
        {
            public BackgroundTaskDeferralDisposable(BackgroundTaskDeferral deferral)
            {
                Debug.Assert(deferral != null);
                this.Deferral = deferral;
            }

            public void Dispose() => this.Deferral.Complete();

            public BackgroundTaskDeferral Deferral { get; }
        }
    }
}