using JetBrains.Annotations;

namespace System.Threading.Tasks
{
    public static class TaskExtensions
    {
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

        public static async Task<TTo> AsyncSelect<TFrom, TTo>([NotNull] this Task<TFrom> task,
            [NotNull] Func<TFrom, TTo> selector)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return await Task.Run(async () => selector(await task));
        }
    }
}
