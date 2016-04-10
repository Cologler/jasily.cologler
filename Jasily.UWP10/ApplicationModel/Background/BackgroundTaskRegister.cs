using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Jasily.ApplicationModel.Background
{
    public abstract class BackgroundTaskRegister : IGetKey<string>
    {
        protected abstract string TaskName { get; }

        string IGetKey<string>.GetKey() => this.TaskName;

        protected abstract void Register();

        public static async Task TryRegister(params BackgroundTaskRegister[] registers)
        {
            if (registers == null) throw new ArgumentNullException(nameof(registers));

            await TryRegister((IEnumerable<BackgroundTaskRegister>)registers);
        }

        public static async Task TryRegister(IEnumerable<BackgroundTaskRegister> registers)
        {
            if (registers == null) throw new ArgumentNullException(nameof(registers));

            var status = await BackgroundExecutionManager.RequestAccessAsync();
            if (status == BackgroundAccessStatus.Unspecified || status == BackgroundAccessStatus.Denied)
            {
                if (Debugger.IsAttached) Debugger.Break();
                return;
            }

            var dict = new Dictionary<string, BackgroundTaskRegister>();
            dict.AddRange(registers);

            foreach (var task in BackgroundTaskRegistration.AllTasks.Values)
            {
                if (!dict.Remove(task.Name))
                {
                    task.Unregister(false);
                }
            }

            foreach (var register in dict.Values)
            {
                register.Register();
            }
        }
    }
}