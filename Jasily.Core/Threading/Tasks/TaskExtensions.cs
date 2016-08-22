using System.Collections.Generic;
using JetBrains.Annotations;

namespace System.Threading.Tasks
{
    public static class TaskExtensions
    {
        #region for static

        public static async Task<T[]> WhenAllAsync<T>([NotNull] this IEnumerable<Task<T>> tasks) => await Task.WhenAll(tasks);

        #endregion

        public static T StartIfAllowed<T>([NotNull] this T task, TaskScheduler scheduler = null) where T : Task
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            if (task.Status == TaskStatus.Created)
            {
                if (scheduler == null)
                {
                    task.Start();
                }
                else
                {
                    task.Start(scheduler);
                }
            }
            return task;
        }
    }
}
