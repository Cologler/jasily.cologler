using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Performance
{
    /// <summary>
    /// ref from http://www.cnblogs.com/jeffreyzhao/archive/2009/03/10/codetimer.html
    /// </summary>
    public sealed class CodeTimer : IDisposable
    {
        private bool IsCachedStatus;
        private ProcessPriorityClass CachedProcessPriorityClass;
        private ThreadPriority CachedThreadPriority;

        public CodeTimer()
        {
            
        }

        public void Initialize()
        {
            this.CachedProcessPriorityClass = Process.GetCurrentProcess().PriorityClass;
            this.CachedThreadPriority = Thread.CurrentThread.Priority;
            
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            this.IsCachedStatus = true;

            this.Time(1, () => { });
        }

        public CodeTimerResult Time(int iteration, Action action)
        {
            // 1.
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            var gcCounts = new int[GC.MaxGeneration + 1];
            for (var i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }

            // 2.
            var watch = new Stopwatch();
            watch.Start();
            var cycleCount = GetCycleCount();
            for (var i = 0; i < iteration; i++) action();
            var cpuCycles = GetCycleCount() - cycleCount;
            watch.Stop();

            var gens = new [] { 0, 0, 0 };

            for (var i = 0; i < 3; i++)
            {
                if (i <= GC.MaxGeneration)
                {
                    gens[i] = GC.CollectionCount(i) - gcCounts[i];
                }
            }

            return new CodeTimerResult(watch.ElapsedMilliseconds, cpuCycles, gens[0], gens[1], gens[2]);
        }

        private static ulong GetCycleCount()
        {
            ulong cycleCount = 0;
            QueryThreadCycleTime(GetCurrentThread(), ref cycleCount);
            return cycleCount;
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThread();

        public void Dispose()
        {
            if (this.IsCachedStatus)
            {
                Process.GetCurrentProcess().PriorityClass = this.CachedProcessPriorityClass;
                Thread.CurrentThread.Priority = this.CachedThreadPriority;
                this.IsCachedStatus = false;
            }
        }
    }
}
