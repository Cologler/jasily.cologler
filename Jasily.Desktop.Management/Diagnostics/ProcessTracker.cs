using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace Jasily.Desktop.Management.Diagnostics
{
    public sealed class ProcessTracker : IDisposable
    {
        private readonly ManagementEventWatcher startWatch;
        private readonly ManagementEventWatcher stopWatch;
        public event EventHandler<int> ProcessStarted;
        public event EventHandler<int> ProcessStoped;
        private bool isDisposed;

        public ProcessTracker()
        {
            this.startWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            this.startWatch.EventArrived += this.OnProcessStarted;

            this.stopWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
            this.stopWatch.EventArrived += this.OnProcessStoped;
        }

        public bool Start(int delay = 100)
        {
            try
            {
                this.startWatch.Start();
                this.stopWatch.Start();
                return true;
            }
            catch (ManagementException)
            {
                this.StartBackgroundLoop(delay);
            }

            return false;
        }

        public void Stop()
        {
            this.startWatch.Stop();
            this.stopWatch.Stop();
            this.isDisposed = true;
        }

        private void StartBackgroundLoop(int delay)
        {
            Task.Run(async () =>
            {
                var ids = new int[0];
                while (!this.isDisposed)
                {
                    await Task.Delay(delay);
                    var cur = Process.GetProcesses().Select(z => z.Id).ToArray();
                    if (this.ProcessStarted != null)
                    {
                        var news = cur.Except(ids).ToArray();
                        foreach (var i in news) this.ProcessStarted?.Invoke(this, i);
                    }
                    if (this.ProcessStoped != null)
                    {
                        var olds = ids.Except(cur).ToArray();
                        foreach (var i in olds) this.ProcessStoped?.Invoke(this, i);
                    }
                    ids = cur;
                }
            });
        }

        private void OnProcessStoped(object sender, EventArrivedEventArgs e)
        {
            this.ProcessStarted?.Invoke(this, int.Parse((string)e.NewEvent.Properties["ProcessId"].Value));
        }

        private void OnProcessStarted(object sender, EventArrivedEventArgs e)
        {
            this.ProcessStoped?.Invoke(this, int.Parse((string)e.NewEvent.Properties["ProcessId"].Value));
        }

        public void Dispose() => this.Stop();
    }
}