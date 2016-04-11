using JetBrains.Annotations;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Jasily.Threading.Tasks
{
    /// <summary>
    /// if you try create a ( async void BeginXXX() => await XXXAsync(); ) just use this class!
    /// </summary>
    public static class JasilyTask
    {
        private static async Task RunTaskFactory([NotNull] Func<Task> taskFactory)
        {
            Debug.WriteLine("JasilyTask: run at" + DateTime.Now);
            Debug.Assert(taskFactory != null);

            var t = taskFactory();
            if (t.Status == TaskStatus.Created) t.Start();
            await t;
        }

        public static async void Begin([NotNull] Func<Task> taskFactory)
        {
            if (taskFactory == null) throw new ArgumentNullException(nameof(taskFactory));

            await RunTaskFactory(taskFactory);
        }

        /// <summary>
        /// exec task same time
        /// </summary>
        /// <param name="taskFactorys"></param>
        public static void Begin([NotNull] params Func<Task>[] taskFactorys)
        {
            if (taskFactorys == null || taskFactorys.Any(z => z == null))
                throw new ArgumentNullException(nameof(taskFactorys));

            foreach (var t in taskFactorys) Begin(t);
        }

        /// <summary>
        /// orderly exec task one by one
        /// </summary>
        /// <param name="taskFactorys"></param>
        public static async void BeginQueue([NotNull] params Func<Task>[] taskFactorys)
        {
            if (taskFactorys == null || taskFactorys.Any(z => z == null))
                throw new ArgumentNullException(nameof(taskFactorys));

            foreach (var t in taskFactorys) await RunTaskFactory(t);
        }

        /// <summary>
        /// orderly exec task one by one
        /// </summary>
        /// <param name="taskFactorys"></param>
        /// <returns></returns>
        public static async Task StartQueue([NotNull] params Func<Task>[] taskFactorys)
        {
            if (taskFactorys == null || taskFactorys.Any(z => z == null))
                throw new ArgumentNullException(nameof(taskFactorys));

            foreach (var t in taskFactorys) await RunTaskFactory(t);
        }
    }
}