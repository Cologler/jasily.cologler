using JetBrains.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Jasily.Threading.Tasks
{
    public static class JasilyTask
    {
        public static async void Begin([NotNull] params Func<Task>[] tasks)
        {
            if (tasks == null || tasks.Any(z => z == null)) throw new ArgumentNullException(nameof(tasks));

            foreach (var t in tasks) await t();
        }
    }
}