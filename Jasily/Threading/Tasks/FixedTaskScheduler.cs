using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jasily.Threading.Tasks
{
    public class FixedTaskScheduler : TaskScheduler
    {
        private int taskCount;
        private readonly Queue<Task> tasks = new Queue<Task>();
        private readonly TaskFactory taskFactory;

        public FixedTaskScheduler(int maxThread)
        {
            if (maxThread < 1) throw new ArgumentOutOfRangeException(nameof(maxThread));
            this.MaximumConcurrencyLevel = maxThread;
            this.taskFactory = new TaskFactory(TaskScheduler.Default);
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            lock (this.tasks)
            {
                return this.tasks.ToArray();
            }
        }

        protected override void QueueTask(Task task)
        {
            bool needNew;

            lock (this.tasks)
            {
                this.tasks.Enqueue(task);
                needNew = this.taskCount < this.MaximumConcurrencyLevel;
                if (needNew) this.taskCount++;
            }
            
            if (needNew)
            {
                this.StartNewTask();
            }
        }

        protected override bool TryDequeue(Task task)
        {
            lock (this.tasks)
            {
                var array = this.tasks.ToArray();
                if (array.FirstOrDefault(z => z == task) != null)
                {
                    this.tasks.Clear();
                    foreach (var t in array.Where(z => z != task))
                    {
                        this.tasks.Enqueue(t);
                    }
                }
            }

            return false;
        }

        private void StartNewTask()
        {
            this.taskFactory.StartNew(() =>
            {
                while (true)
                {
                    Task task;
                    lock (this.tasks)
                    {
                        if (this.tasks.Count == 0)
                        {
                            this.taskCount--;
                            return;
                        }
                        task = this.tasks.Dequeue();
                    }
                    this.TryExecuteTask(task);
                }
            });
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (taskWasPreviouslyQueued && !this.TryDequeue(task)) return false;
            return this.TryExecuteTask(task);
        }

        public override int MaximumConcurrencyLevel { get; }
    }
}