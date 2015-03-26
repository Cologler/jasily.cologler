using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            CachedProcessPriorityClass = Process.GetCurrentProcess().PriorityClass;
            CachedThreadPriority = Thread.CurrentThread.Priority;
            
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            IsCachedStatus = true;

            Time(1, () => { });
        }

        public CodeTimerResult Time(int iteration, Action action)
        {
            // 1.
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            int[] gcCounts = new int[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }

            // 2.
            Stopwatch watch = new Stopwatch();
            watch.Start();
            ulong cycleCount = GetCycleCount();
            for (int i = 0; i < iteration; i++) action();
            ulong cpuCycles = GetCycleCount() - cycleCount;
            watch.Stop();

            var gens = new [] { 0, 0, 0 };

            for (int i = 0; i < 3; i++)
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
                Process.GetCurrentProcess().PriorityClass = CachedProcessPriorityClass;
                Thread.CurrentThread.Priority = CachedThreadPriority;
                this.IsCachedStatus = false;
            }
        }
    }
}
