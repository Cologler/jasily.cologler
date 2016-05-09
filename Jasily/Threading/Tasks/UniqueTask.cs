using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily.Threading.Tasks
{
    public sealed class UniqueTask<T>
    {
        private bool isCompleted;
        private T result;
        private Func<Task<T>> taskFunc;
        private Task<T> task;
        private object syncRoot = new object();

        public UniqueTask([NotNull] Func<Task<T>> taskFunc)
        {
            if (taskFunc == null) throw new ArgumentNullException(nameof(taskFunc));

            this.taskFunc = taskFunc;
        }

        public async Task<T> Run()
        {
            if (this.isCompleted) return this.result;

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

            this.result = await this.task;
            this.isCompleted = true;

            // remove
            this.task = null;
            this.syncRoot = null;
            this.taskFunc = null;

            return this.result;
        }
    }

    public sealed class UniqueTask
    {
        private bool isCompleted;
        private Func<Task> taskFunc;
        private Task task;
        private object syncRoot = new object();

        public UniqueTask([NotNull] Func<Task> taskFunc)
        {
            if (taskFunc == null) throw new ArgumentNullException(nameof(taskFunc));

            this.taskFunc = taskFunc;
        }

        public async Task Run()
        {
            if (this.isCompleted) return;

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
            this.isCompleted = true;

            // remove
            this.task = null;
            this.syncRoot = null;
            this.taskFunc = null;
        }
    }
}