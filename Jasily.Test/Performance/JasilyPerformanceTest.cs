using System.Diagnostics;

namespace System.Test.Performance
{
    public sealed class JasilyPerformanceTest
    {
        Stopwatch Watcher;
        int ListenId = 184;

        public event EventHandler<JasilyPerformanceTestHitEventArgs> NextEvent;

        JasilyPerformanceTest(int listenId)
        {
            ListenId = listenId;
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
        public void Next(int id, string desc)
        {
            if (id != ListenId) return;
            var time = Watcher.ElapsedMilliseconds;
            NextEvent.Fire(this, new JasilyPerformanceTestHitEventArgs(id, desc, time));
            Watcher.Restart();
        }

        public sealed class JasilyPerformanceTestHitEventArgs
        {
            public JasilyPerformanceTestHitEventArgs(int id, string desc, long milliseconds)
            {
                Id = id;
                Description = desc;
                Milliseconds = milliseconds;
            }

            public long Milliseconds { get; private set; }

            public string Description { get; private set; }

            public int Id { get; private set; }
        }
    }
}
