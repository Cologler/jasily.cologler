using System.Linq;
using JetBrains.Annotations;

namespace System.Threading.Tasks
{
    public static class TaskFactory2
    {
        public static void Begin([NotNull] Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            task.StartIfAllowed();
        }

        public static void Begin([NotNull] params Task[] tasks)
        {
            if (tasks == null) throw new ArgumentNullException(nameof(tasks));
            foreach (var t in tasks) Begin(t);
        }

        public static async void BeginQueue([NotNull] params Task[] tasks)
        {
            if (tasks == null) throw new ArgumentNullException(nameof(tasks));
            if (tasks.Any(z => z == null)) throw new ArgumentNullException(nameof(tasks), "some task in tasks is null.");
            foreach (var t in tasks) await t.StartIfAllowed();
        }

        /// <summary>
        /// orderly exec task one by one
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static async Task StartQueue([NotNull] params Task[] tasks)
        {
            if (tasks == null) throw new ArgumentNullException(nameof(tasks));
            if (tasks.Any(z => z == null)) throw new ArgumentNullException(nameof(tasks), "some task in tasks is null.");
            foreach (var t in tasks) await t.StartIfAllowed();
        }
    }
}