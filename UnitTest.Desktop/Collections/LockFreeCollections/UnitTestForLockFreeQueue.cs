using Jasily.Collections.LockFreeCollections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Performance;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTest.Desktop.Collections.LockFreeCollections
{
    [TestClass]
    public class UnitTestForLockFreeQueue
    {
        [TestMethod]
        public void TestMethod()
        {
            var queue = new LockFreeQueue<int>();
            int value;
            Assert.AreEqual(false, queue.TryDequeue(out value));
            queue.Enqueue(5);
            Assert.AreEqual(true, queue.TryDequeue(out value));
            Assert.AreEqual(5, value);
            Assert.AreEqual(false, queue.TryDequeue(out value));
            queue.Enqueue(9);
            queue.Enqueue(10);
            Assert.AreEqual(true, queue.TryDequeue(out value));
            Assert.AreEqual(true, queue.TryDequeue(out value));
            Assert.AreEqual(false, queue.TryDequeue(out value));
        }

        [TestMethod]
        public void TestMethodWithThread()
        {
            var queue = new LockFreeQueue<int>();
            var count = 100000;
            int value;
            var setter = new Action(() =>
            {
                foreach (var v in Enumerable.Range(0, count))
                {
                    queue.Enqueue(v);
                }
            });

            var total = 0;
            var getter = new Action(() =>
            {
                while (queue.MayHasNext)
                {
                    if (queue.TryDequeue(out value)) Interlocked.Increment(ref total);
                }
            });

            var getters = Task.WhenAll(Task.Run(getter), Task.Run(getter), Task.Run(getter), Task.Run(getter))
                .ContinueWith(z => Assert.AreEqual(count * 4, total));
            var setters = Task.WhenAll(Task.Run(setter), Task.Run(setter), Task.Run(setter), Task.Run(setter))
                .ContinueWith(z => queue.Dispose());

            Task.WaitAll(setters, getters);

            queue = new LockFreeQueue<int>();
            setters = Task.WhenAll(Task.Run(setter), Task.Run(setter)).ContinueWith(z => queue.Dispose());
            getters = Task.WhenAll(Task.Run(getter), Task.Run(getter)).ContinueWith(z => Assert.AreEqual(count * 6, total));
            Task.WaitAll(setters, getters);
        }

        [TestMethod]
        public void TestMethodWithEnumerator()
        {
            var queue = new LockFreeQueue<int>();
            var count = 100000;
            var setter = new Action(() =>
            {
                foreach (var v in Enumerable.Range(0, count))
                {
                    queue.Enqueue(v);
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

            queue = new LockFreeQueue<int>();
            setters = Task.WhenAll(Task.Run(setter), Task.Run(setter)).ContinueWith(z => queue.Dispose());
            getters = Task.WhenAll(Task.Run(getter), Task.Run(getter)).ContinueWith(z => Assert.AreEqual(count * 6, total));
            Task.WaitAll(setters, getters);
        }

        [TestMethod]
        public void TestMethodWithTimeout()
        {
            var queue = new LockFreeQueue<int>();
            int value;

            var now = DateTime.UtcNow;
            Assert.AreEqual(false, queue.TryDequeue(out value, 1000));
            Debug.WriteLine((DateTime.UtcNow - now).TotalMilliseconds);

            Task.Run(async () =>
            {
                await Task.Delay(5000);
                queue.Enqueue(100);
            });
            now = DateTime.UtcNow;
            Assert.AreEqual(true, queue.TryDequeue(out value, 10000));
            Debug.WriteLine((DateTime.UtcNow - now).TotalMilliseconds);

            Task.Run(async () =>
            {
                await Task.Delay(5000);
                queue.Enqueue(100);
            });
            now = DateTime.UtcNow;
            Assert.AreEqual(false, queue.TryDequeue(out value, 2000));
            Debug.WriteLine((DateTime.UtcNow - now).TotalMilliseconds);
            Assert.AreEqual(true, queue.TryDequeue(out value, 4000));
            Debug.WriteLine((DateTime.UtcNow - now).TotalMilliseconds);
        }

        [TestMethod]
        public void Performance()
        {
            // OMG, it was too many slow !!!

            var count = 100000;

            using (var timer = new CodeTimer())
            {
                timer.Initialize();
                Debug.WriteLine("single thread:");
                Debug.WriteLine("LockFreeQueue: " + timer.Time(100, () =>
                {
                    var queue = new LockFreeQueue<int>();
                    foreach (var v in Enumerable.Range(0, count))
                    {
                        queue.Enqueue(v);
                    }
                }).ToString());
                Debug.WriteLine("Queue: " + timer.Time(100, () =>
                {
                    var queue = new Queue<int>();
                    foreach (var v in Enumerable.Range(0, count))
                    {
                        queue.Enqueue(v);
                    }
                }).ToString());
                Debug.WriteLine("ConcurrentQueue: " + timer.Time(100, () =>
                {
                    var queue = new ConcurrentQueue<int>();
                    foreach (var v in Enumerable.Range(0, count))
                    {
                        queue.Enqueue(v);
                    }
                }).ToString());

                Debug.WriteLine("");
                Debug.WriteLine("mulit thread:");
                Debug.WriteLine("LockFreeQueue: " + timer.Time(100, () =>
                {
                    var queue = new LockFreeQueue<int>();
                    var setter = new Action(() =>
                    {
                        foreach (var v in Enumerable.Range(0, count))
                        {
                            queue.Enqueue(v);
                        }
                    });
                    Task.WaitAll(Task.Run(setter), Task.Run(setter), Task.Run(setter), Task.Run(setter));
                }).ToString());
                Debug.WriteLine("ConcurrentQueue: " + timer.Time(100, () =>
                {
                    var queue = new ConcurrentQueue<int>();
                    var setter = new Action(() =>
                    {
                        foreach (var v in Enumerable.Range(0, count))
                        {
                            queue.Enqueue(v);
                        }
                    });
                    Task.WaitAll(Task.Run(setter), Task.Run(setter), Task.Run(setter), Task.Run(setter));
                }).ToString());

                Debug.WriteLine("");
                Debug.WriteLine("mulit thread read | write:");
                Debug.WriteLine("LockFreeQueue: " + timer.Time(100, () =>
                {
                    var queue = new LockFreeQueue<int>();
                    var setter = new Action(() =>
                    {
                        foreach (var v in Enumerable.Range(0, count))
                        {
                            queue.Enqueue(v);
                        }
                    });
                    var getter = new Action(() =>
                    {
                        foreach (var item in queue)
                        {

                        }
                    });
                    Task.WhenAll(Task.Run(setter), Task.Run(setter), Task.Run(setter), Task.Run(setter))
                        .ContinueWith(z => queue.Dispose())
                        .Wait();
                    Task.WaitAll(Task.Run(getter), Task.Run(getter), Task.Run(getter), Task.Run(getter));
                }).ToString());
                Debug.WriteLine("ConcurrentQueue: " + timer.Time(100, () =>
                {
                    var queue = new ConcurrentQueue<int>();
                    var setter = new Action(() =>
                    {
                        foreach (var v in Enumerable.Range(0, count))
                        {
                            queue.Enqueue(v);
                        }
                    });
                    var getter = new Action(() =>
                    {
                        foreach (var item in queue)
                        {

                        }
                    });
                    Task.WaitAll(Task.Run(setter), Task.Run(setter), Task.Run(setter), Task.Run(setter));
                    Task.WaitAll(Task.Run(getter), Task.Run(getter), Task.Run(getter), Task.Run(getter));
                }).ToString());

                Debug.WriteLine("");
                Debug.WriteLine("mulit thread read & write:");
                Debug.WriteLine("LockFreeQueue: " + timer.Time(100, () =>
                {
                    var queue = new LockFreeQueue<int>();
                    var setter = new Action(() =>
                    {
                        foreach (var v in Enumerable.Range(0, count))
                        {
                            queue.Enqueue(v);
                        }
                    });
                    var getter = new Action(() =>
                    {
                        foreach (var item in queue)
                        {

                        }
                    });
                    Task.WaitAll(Task.WhenAll(Task.Run(setter), Task.Run(setter), Task.Run(setter), Task.Run(setter))
                        .ContinueWith(z => queue.Dispose()),
                        Task.WhenAll(Task.Run(getter), Task.Run(getter), Task.Run(getter), Task.Run(getter))
                    );
                }).ToString());
                Debug.WriteLine("ConcurrentQueue: " + timer.Time(100, () =>
                {
                    var queue = new ConcurrentQueue<int>();
                    var setter = new Action(() =>
                    {
                        foreach (var v in Enumerable.Range(0, count))
                        {
                            queue.Enqueue(v);
                        }
                    });
                    var getter = new Action(() =>
                    {
                        foreach (var item in queue)
                        {

                        }
                    });
                    Task.WaitAll(
                        Task.WhenAll(Task.Run(setter), Task.Run(setter), Task.Run(setter), Task.Run(setter)),
                        Task.WhenAll(Task.Run(getter), Task.Run(getter), Task.Run(getter), Task.Run(getter))
                    );
                }).ToString());
            }
        }
    }
}