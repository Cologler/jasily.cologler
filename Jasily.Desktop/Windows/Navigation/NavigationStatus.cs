using System;
using System.Windows.Navigation;

namespace Jasily.Desktop.Windows.Navigation
{
    public sealed class NavigationStatus : IDisposable
    {
        private readonly NavigationService service;

        public NavigationStatus(NavigationService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.Status = NavigationServiceStatus.Unknown;
            this.service = service;
            this.service.Navigating += this.Service_Navigating;
            this.service.Navigated += this.Service_Navigated;
        }

        private void Service_Navigated(object sender, NavigationEventArgs e)
        {
            this.Status = NavigationServiceStatus.Navigated;
        }

        private void Service_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            this.Status = NavigationServiceStatus.Navigating;
        }

        public NavigationServiceStatus Status { get; private set; }

        public void Dispose()
        {
            this.service.Navigating -= this.Service_Navigating;
            this.service.Navigated -= this.Service_Navigated;
        }
    }
}