using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Performance;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTest.Desktop.Performance
{
    [TestClass]
    public class UnitTestForCodeTimer
    {
        [TestMethod]
        public void TestForCodeTimer()
        {
            var CachedProcessPriorityClass = Process.GetCurrentProcess().PriorityClass;
            var CachedThreadPriority = Thread.CurrentThread.Priority;

            using (var timer = new CodeTimer())
            {
                timer.Initialize();
                var result = timer.Time(1, () => { });
                Assert.Equals(result.CPUCycles, 0);
            }

            Assert.Equals(CachedProcessPriorityClass, Process.GetCurrentProcess().PriorityClass);
            Assert.Equals(CachedThreadPriority, Thread.CurrentThread.Priority);
        }
    }
}
