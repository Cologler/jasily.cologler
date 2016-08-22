using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily.Threading.Tasks
{
    public sealed class UniqueTask<T>
    {
        private object syncRoot = new object();
        private bool isCompleted;
        private T result;
        private Func<Task<T>> target;
        private Task<T> task;

        public UniqueTask([NotNull] Func<Task<T>> target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            this.target = target;
        }

        public async Task<T> Run()
        {
            if (this.isCompleted) return this.result;

            if (this.task == null)
            {
                lock (this.syncRoot)
                {
                    if (this.isCompleted) return this.result;
                    if (this.task == null)
                    {
                        this.task = this.target().StartIfAllowed();
                    }
                }
            }

            this.result = await this.task;

            lock (this.syncRoot)
            {
                this.isCompleted = true;
            }

            // remove
            this.task = null;
            this.syncRoot = null;
            this.target = null;

            return this.result;
        }
    }

    public sealed class UniqueTask
    {
        private bool isCompleted;
        private Func<Task> target;
        private Task task;
        private object syncRoot = new object();

        public UniqueTask([NotNull] Func<Task> target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            this.target = target;
        }

        public async Task Run()
        {
            if (this.isCompleted) return;

            if (this.task == null)
            {
                lock (this.syncRoot)
                {
                    if (this.isCompleted) return;
                    if (this.task == null)
                    {
                        this.task = this.target().StartIfAllowed();
                    }
                }
            }

            await this.task;

            lock (this.syncRoot)
            {
                this.isCompleted = true;
            }

            // remove
            this.task = null;
            this.syncRoot = null;
            this.target = null;
        }
    }
}