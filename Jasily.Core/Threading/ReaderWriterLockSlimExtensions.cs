using System.Diagnostics;
using JetBrains.Annotations;

namespace System.Threading
{
    public static class ReaderWriterLockSlimExtensions
    {
        public static Releaser<ReaderWriterLockSlim> StartRead([NotNull] this ReaderWriterLockSlim locker)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            locker.EnterReadLock();
            return Read(locker);
        }

        public static Releaser<ReaderWriterLockSlim> StartWrite([NotNull] this ReaderWriterLockSlim locker)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            locker.EnterWriteLock();
            return Write(locker);
        }

        public static Releaser<ReaderWriterLockSlim> TryStartRead([NotNull] this ReaderWriterLockSlim locker, int millisecondsTimeout)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            return locker.TryEnterReadLock(millisecondsTimeout) ? Read(locker) : new Releaser<ReaderWriterLockSlim>();
        }

        public static Releaser<ReaderWriterLockSlim> TryStartRead([NotNull] this ReaderWriterLockSlim locker, TimeSpan timeout)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            return locker.TryEnterReadLock(timeout) ? Read(locker) : new Releaser<ReaderWriterLockSlim>();
        }

        public static Releaser<ReaderWriterLockSlim> TryStartWrite([NotNull] this ReaderWriterLockSlim locker, int millisecondsTimeout)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            return locker.TryEnterWriteLock(millisecondsTimeout) ? Write(locker) : new Releaser<ReaderWriterLockSlim>();
        }

        public static Releaser<ReaderWriterLockSlim> TryStartWrite([NotNull] this ReaderWriterLockSlim locker, TimeSpan timeout)
        {
            if (locker == null) throw new ArgumentNullException(nameof(locker));
            return locker.TryEnterWriteLock(timeout) ? Write(locker) : new Releaser<ReaderWriterLockSlim>();
        }

        private static Releaser<ReaderWriterLockSlim> Read(ReaderWriterLockSlim locker)
        {
            Debug.Assert(locker != null);
            var releaser = new Releaser<ReaderWriterLockSlim>(true, locker);
            releaser.ReleaseRaised += (s, e) => e.ExitReadLock();
            return releaser;
        }

        private static Releaser<ReaderWriterLockSlim> UpgradeableRead(ReaderWriterLockSlim locker)
        {
            Debug.Assert(locker != null);
            var releaser = new Releaser<ReaderWriterLockSlim>(true, locker);
            releaser.ReleaseRaised += (s, e) => e.ExitUpgradeableReadLock();
            return releaser;
        }

        private static Releaser<ReaderWriterLockSlim> Write(ReaderWriterLockSlim locker)
        {
            Debug.Assert(locker != null);
            var releaser = new Releaser<ReaderWriterLockSlim>(true, locker);
            releaser.ReleaseRaised += (s, e) => e.ExitWriteLock();
            return releaser;
        }
    }
}