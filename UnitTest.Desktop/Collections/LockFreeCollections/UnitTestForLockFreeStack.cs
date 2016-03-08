using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jasily.Collections.LockFreeCollections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Desktop.Collections.LockFreeCollections
{
    [TestClass]
    public class UnitTestForLockFreeStack
    {
        [TestMethod]
        public void TestMethod()
        {
            var queue = new LockFreeStack<int>();
            int value;
            Assert.AreEqual(false, queue.TryPop(out value));
            queue.Push(5);
            Assert.AreEqual(true, queue.TryPop(out value));
            Assert.AreEqual(5, value);
            Assert.AreEqual(false, queue.TryPop(out value));
            queue.Push(9);
            queue.Push(10);
            Assert.AreEqual(true, queue.TryPop(out value));
            Assert.AreEqual(true, queue.TryPop(out value));
            Assert.AreEqual(false, queue.TryPop(out value));
        }

        [TestMethod]
        public void TestMethodWithThread()
        {
            var queue = new LockFreeStack<int>();
            var count = 100000;
            int value;
            var setter = new Action(() =>
            {
                foreach (var v in Enumerable.Range(0, count))
                {
                    queue.Push(v);
                }
            });

            var total = 0;
            var getter = new Action(() =>
            {
                while (queue.MayHasNext)
                {
                    if (queue.TryPop(out value)) Interlocked.Increment(ref total);
                }
            });

            var getters = Task.WhenAll(Task.Run(getter), Task.Run(getter), Task.Run(getter), Task.Run(getter))
                .ContinueWith(z => Assert.AreEqual(count * 4, total));
            var setters = Task.WhenAll(Task.Run(setter), Task.Run(setter), Task.Run(setter), Task.Run(setter))
                .ContinueWith(z => queue.Dispose());

            Task.WaitAll(setters, getters);

            queue = new LockFreeStack<int>();
            setters = Task.WhenAll(Task.Run(setter), Task.Run(setter)).ContinueWith(z => queue.Dispose());
            getters = Task.WhenAll(Task.Run(getter), Task.Run(getter)).ContinueWith(z => Assert.AreEqual(count * 6, total));
            Task.WaitAll(setters, getters);
        }

        [TestMethod]
        public void TestMethodWithEnumerator()
        {
            var queue = new LockFreeStack<int>();
            var count = 100000;
            var setter = new Action(() =>
            {
                foreach (var v in Enumerable.Range(0, count))
                {
                    queue.Push(v);
                }
            });

            var total = 0;
            var getter = new Action(() =>
            {
                foreach (var i in queue)
                {
                    Interlocked.Increment(ref total);
                }
            });

            var getters = Task.WhenAll(Task.Run(getter), Task.Run(getter), Task.Run(getter), Task.Run(getter))
                .ContinueWith(z => Assert.AreEqual(count * 4, total));
            var setters = Task.WhenAll(Task.Run(setter), Task.Run(setter), Task.Run(setter), Task.Run(setter))
                .ContinueWith(z => queue.Dispose());

            Task.WaitAll(setters, getters);

            queue = new LockFreeStack<int>();
            setters = Task.WhenAll(Task.Run(setter), Task.Run(setter)).ContinueWith(z => queue.Dispose());
            getters = Task.WhenAll(Task.Run(getter), Task.Run(getter)).ContinueWith(z => Assert.AreEqual(count * 6, total));
            Task.WaitAll(setters, getters);
        }

        [TestMethod]
        public void TestMethodWithTimeout()
        {
            var queue = new LockFreeStack<int>();
            int value;

            var now = DateTime.UtcNow;
            Assert.AreEqual(false, queue.TryPop(out value, 1000));
            Debug.WriteLine((DateTime.UtcNow - now).TotalMilliseconds);

            Task.Run(async () =>
            {
                await Task.Delay(5000);
                queue.Push(100);
            });
            now = DateTime.UtcNow;
            Assert.AreEqual(true, queue.TryPop(out value, 10000));
            Debug.WriteLine((DateTime.UtcNow - now).TotalMilliseconds);

            Task.Run(async () =>
            {
                await Task.Delay(5000);
                queue.Push(100);
            });
            now = DateTime.UtcNow;
            Assert.AreEqual(false, queue.TryPop(out value, 2000));
            Debug.WriteLine((DateTime.UtcNow - now).TotalMilliseconds);
            Assert.AreEqual(true, queue.TryPop(out value, 4000));
            Debug.WriteLine((DateTime.UtcNow - now).TotalMilliseconds);
        }
    }
}