using System;
using System.Threading.Tasks;
using Jasily.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily
{
    public sealed class AsyncLazy<T>
    {
        private readonly UniqueTask<T> task;

        public AsyncLazy([NotNull] Func<Task<T>> initFunc)
        {
            if (initFunc == null) throw new ArgumentNullException(nameof(initFunc));
            this.task = new UniqueTask<T>(initFunc);
        }

        public async Task<T> GetValueAsync() => await this.task.Run();
    }
}