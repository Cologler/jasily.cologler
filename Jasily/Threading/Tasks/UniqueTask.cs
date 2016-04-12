using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace Jasily.Threading.Tasks
{
    public sealed class UniqueTask<T>
    {
        private readonly Func<Task<T>> taskFunc;
        private Task<T> task;
        private readonly object syncRoot = new object();

        public UniqueTask([NotNull] Func<Task<T>> taskFunc)
        {
            if (taskFunc == null) throw new ArgumentNullException(nameof(taskFunc));

            this.taskFunc = taskFunc;
        }

        public async Task<T> Run()
        {
            if (this.task == null)
            {
                lock (this.syncRoot)
                {
                    if (this.task == null)
                    {
                        this.task = JasilyTask.Run(this.taskFunc);
                    }
                }
            }

            return await this.task;
        }
    }

    public sealed class UniqueTask
    {
        private readonly Func<Task> taskFunc;
        private Task task;
        private readonly object syncRoot = new object();

        public UniqueTask([NotNull] Func<Task> taskFunc)
        {
            if (taskFunc == null) throw new ArgumentNullException(nameof(taskFunc));

            this.taskFunc = taskFunc;
        }

        public async Task Run()
        {
            if (this.task == null)
            {
                lock (this.syncRoot)
                {
                    if (this.task == null)
                    {
                        this.task = JasilyTask.Run(this.taskFunc);
                    }
                }
            }

            await this.task;
        }
    }
}