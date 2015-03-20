using System.Diagnostics;

namespace System.Test.Performance
{
    /// <summary>
    /// alse see: http://blog.zhaojie.me/2009/03/codetimer.html
    /// </summary>
    public sealed class JasilyPerformanceTest
    {
        Stopwatch Watcher;
        long Last;

        public event EventHandler<JasilyPerformanceTestHitEventArgs> NextEvent;

        JasilyPerformanceTest(int listenId)
        {
            Initialize();
        }

        [Conditional("DEBUG")]
        private void Initialize()
        {
            Watcher = new Stopwatch();
        }

        [Conditional("DEBUG")]
        public void Start()
        {
            Watcher.Start();
        }

        [Conditional("DEBUG")]
        public void Next(string desc)
        {
            var time = Last = Watcher.ElapsedMilliseconds - Last;
            NextEvent.Fire(this, new JasilyPerformanceTestHitEventArgs(desc, time));
            Watcher.Restart();
        }

        public struct JasilyPerformanceTestHitEventArgs
        {
            string _description;
            long _milliseconds;

            public JasilyPerformanceTestHitEventArgs(string desc, long milliseconds)
            {
                _description = desc;
                _milliseconds = milliseconds;
            }

            public long Milliseconds
            {
                get { return _milliseconds; }
            }

            public string Description
            {
                get { return _description; }
            }
        }
    }
}
