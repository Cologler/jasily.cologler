namespace System.Threading
{
    public static class ReaderWriterLockSlimExtensions
    {
        public static IDisposable StartRead(this ReaderWriterLockSlim @lock)
        {
            @lock.EnterReadLock();
            return new ReadMode(@lock);
        }

        public static IDisposable StartWrite(this ReaderWriterLockSlim @lock)
        {
            @lock.EnterWriteLock();
            return new WriteMode(@lock);
        }

        public static ITryEnter TryStartRead(this ReaderWriterLockSlim @lock, int millisecondsTimeout)
            => @lock.TryEnterReadLock(millisecondsTimeout) ? new ReadMode(@lock) : new ReadMode(null);

        public static ITryEnter TryStartRead(this ReaderWriterLockSlim @lock, TimeSpan timeout)
            => @lock.TryEnterReadLock(timeout) ? new ReadMode(@lock) : new ReadMode(null);

        public static ITryEnter TryStartWrite(this ReaderWriterLockSlim @lock, int millisecondsTimeout)
            => @lock.TryEnterWriteLock(millisecondsTimeout) ? new WriteMode(@lock) : new WriteMode(null);

        public static ITryEnter TryStartWrite(this ReaderWriterLockSlim @lock, TimeSpan timeout)
            => @lock.TryEnterWriteLock(timeout) ? new WriteMode(@lock) : new WriteMode(null);

        private class ReadMode : ITryEnter
        {
            private readonly ReaderWriterLockSlim @lock;

            public ReadMode(ReaderWriterLockSlim @lock)
            {
                this.@lock = @lock;
            }

            public void Dispose() => this.@lock?.ExitReadLock();

            public bool IsEntered => this.@lock != null;
        }

        private class WriteMode : ITryEnter
        {
            private readonly ReaderWriterLockSlim @lock;

            public WriteMode(ReaderWriterLockSlim @lock)
            {
                this.@lock = @lock;
            }

            public void Dispose() => this.@lock?.ExitWriteLock();

            public bool IsEntered => this.@lock != null;
        }
    }
}