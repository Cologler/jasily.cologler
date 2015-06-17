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
            this.Initialize();
        }

        [Conditional("DEBUG")]
        private void Initialize()
        {
            this.Watcher = new Stopwatch();
        }

        [Conditional("DEBUG")]
        public void Start()
        {
            this.Watcher.Start();
        }

        [Conditional("DEBUG")]
        public void Next(string desc)
        {
            var time = this.Last = this.Watcher.ElapsedMilliseconds - this.Last;
            this.NextEvent.Fire(this, new JasilyPerformanceTestHitEventArgs(desc, time));
            this.Watcher.Restart();
        }

        public struct JasilyPerformanceTestHitEventArgs
        {
            string _description;
            long _milliseconds;

            public JasilyPerformanceTestHitEventArgs(string desc, long milliseconds)
            {
                this._description = desc;
                this._milliseconds = milliseconds;
            }

            public long Milliseconds
            {
                get { return this._milliseconds; }
            }

            public string Description
            {
                get { return this._description; }
            }
        }
    }
}
