using Jasily.Threading.Tasks;
using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

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

        public async Task<T> GetValue() => await this.task.Run();
    }
}