using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jasily.Threading.Tasks
{
    /// <summary>
    /// use @id to sure a task was single instanse once time.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JasilySingleTaskManager<T>
    {
        public static JasilySingleTaskManager<T> Default { get; } = new JasilySingleTaskManager<T>();

        private readonly object syncRoot = new object();
        private readonly Dictionary<string, Task<T>> pool = new Dictionary<string, Task<T>>();

        public async Task<T> RunAsync([NotNull] string id, [NotNull] Func<T> func)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return await this.RunAsync(id, () => Task.Run(func));
        }
        public async Task<T> RunAsync([NotNull] string id, [NotNull] Func<Task<T>> taskFactory)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (taskFactory == null) throw new ArgumentNullException(nameof(taskFactory));

            Task<T> t;
            var isNew = false;
            lock (this.syncRoot)
            {
                if (!this.pool.TryGetValue(id, out t))
                {
                    isNew = true;
                    t = taskFactory();
                    this.pool.Add(id, t);
                }
            }

            try
            {
                return await t;
            }
            finally
            {
                if (isNew)
                {
                    lock (this.syncRoot)
                    {
                        this.pool.Remove(id);
                    }
                }
            }
        }
    }

    public class JasilySingleTaskManager : JasilySingleTaskManager<bool>
    {
        public static new JasilySingleTaskManager Default { get; } = new JasilySingleTaskManager();

        public async Task RunAsync([NotNull] string id, [NotNull] Action action)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (action == null) throw new ArgumentNullException(nameof(action));

            await this.RunAsync(id, () =>
            {
                action();
                return true;
            });
        }
        public async Task RunAsync([NotNull] string id, [NotNull] Func<Task> taskFactory)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (taskFactory == null) throw new ArgumentNullException(nameof(taskFactory));

            await this.RunAsync(id, async () =>
            {
                await taskFactory();
                return true;
            });
        }
    }
}
