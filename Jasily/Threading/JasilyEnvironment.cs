using System;
using System.Threading;

namespace Jasily.Threading
{
    /// <summary>
    /// this is not thread safe
    /// </summary>
    public abstract class JasilyEnvironment
    {
        public int Count { get; }

        public abstract int Current { get; }

        protected JasilyEnvironment(int count)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            this.Count = count;
        }

        public Result Enter()
        {
            if (this.OnGet())
            {
                var r = new Result(true);
                r.Exited += this.R_Exited;
                return r;
            }
            return new Result(false);
        }

        private void R_Exited(object sender, EventArgs e) => this.OnPut();

        protected abstract bool OnGet();

        protected abstract void OnPut();

        public struct Result : IDisposable
        {
            public event EventHandler Exited;

            public Result(bool get)
            {
                // ReSharper disable once ArrangeThisQualifier
                Exited = null;
                this.IsGet = get;
            }

            public bool IsGet { get; }

            public void Dispose() => this.Exited?.Invoke(this);
        }

        /// <summary>
        /// general use in UI thread.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static JasilyEnvironment CreateThreadNotSafe(int count = 1)
            => new ThreadNotSafeEnvironment(count);

        public static JasilyEnvironment CreateThreadSafe(int count = 1)
            => new ThreadSafeEnvironment(count);

        private class ThreadNotSafeEnvironment : JasilyEnvironment
        {
            private int current;

            public ThreadNotSafeEnvironment(int count)
                : base(count)
            {
            }

            public override int Current => this.current;

            protected override bool OnGet()
            {
                if (this.current >= this.Count) return false;
                this.current++;
                return true;
            }

            protected override void OnPut() => this.current--;
        }

        private class ThreadSafeEnvironment : JasilyEnvironment
        {
            private int current;

            public ThreadSafeEnvironment(int count)
                : base(count)
            {
            }

            public override int Current => this.current;

            protected override bool OnGet()
            {
                if (this.current >= this.Count) return false;
                if (Interlocked.Increment(ref this.current) > this.Count)
                {
                    Interlocked.Decrement(ref this.current);
                    return false;
                }
                return true;
            }

            protected override void OnPut() => Interlocked.Decrement(ref this.current);
        }
    }
}