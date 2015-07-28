using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Threading.Tasks
{
    /// <summary>
    /// use @id to sure a task was single instanse once time.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JasilySingleTaskManager<T>
    {
        public static JasilySingleTaskManager<T> Default { get; } = new JasilySingleTaskManager<T>();

        private readonly object SyncRoot = new object();
        private readonly Dictionary<string, Task<T>> Pool = new Dictionary<string, Task<T>>();

        public async Task<T> RunAsync(string id, Task<T> task)
        {
            if (task == null)
                throw new ArgumentNullException();

            Task<T> t;
            lock (this.SyncRoot)
            {
                if (!Pool.TryGetValue(id, out t))
                {
                    t = task;
                    Pool.Add(id, t);
                }
            }

            try
            {
                return await t;
            }
            finally
            {
                if (t == task)
                {
                    lock (this.SyncRoot)
                    {
                        Pool.Remove(id);
                    }
                }
            }
        }
        public async Task<T> RunAsync(string id, Func<Task<T>> taskFactory)
        {
            return await this.RunAsync(id, taskFactory());
        }        
        public async Task<T> RunAsync(string id, Func<T> func)
        {
            return await this.RunAsync(id, Task.Run(() => func()));
        }
    }

    public class JasilySingleTaskManager : JasilySingleTaskManager<bool>
    {
        public static new JasilySingleTaskManager Default { get; } = new JasilySingleTaskManager();

        public async Task RunAsync(string id, Task task)
        {
            if (task == null)
                throw new ArgumentNullException();

            await this.RunAsync(id, async () =>
            {
                await task;
                return true;
            });
        }
        public async Task RunAsync(string id, Func<Task> taskFactory)
        {
            await this.RunAsync(id, taskFactory());
        }
        public async Task RunAsync(string id, Action action)
        {
            await this.RunAsync(id, Task.Run(() => action());
        }
    }
}
