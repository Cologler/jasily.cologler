using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Threading
{
    public sealed class JasiyTaskManager<TKey>
    {
        public static JasiyTaskManager<TKey> Default { get; } = new JasiyTaskManager<TKey>();

        private Dictionary<TKey, Task> TaskPool = new Dictionary<TKey, Task>();

        public async Task RunAsync(TKey id, Func<Task> action)
        {
            var func = new Func<Task<bool>>(async () =>
            {
                await action();
                return false;
            });

            await this.RunAsync(id, func);
        }
        public async Task<TResult> RunAsync<TResult>(TKey id, Func<Task<TResult>> func)
        {
            Task<TResult> task;
            lock (this.TaskPool)
            {
                task = (Task<TResult>)this.TaskPool.GetOrSetValue(id, func);
            }

            try
            {
                return await func();
            }
            finally
            {

            }            
        }

        private class WrapedTask
        {
            public WrapedTask()
            {

            }


        }
    }
}
