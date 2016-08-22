using JetBrains.Annotations;

namespace System
{
    public struct Releaser : IReleaser
    {
        public event TypedEventHandler<Releaser, object> ReleaseRaised;
        private readonly object state;

        private void Release() => this.ReleaseRaised?.Invoke(this, this.state);

        public Releaser(bool isAcquired, object state = null)
        {
            ReleaseRaised = null;
            this.IsAcquired = isAcquired;
            this.state = state;
        }

        public bool IsAcquired { get; }

        #region Implementation of IDisposable

        /// <summary>执行与释放或重置非托管资源相关的应用程序定义的任务。</summary>
        public void Dispose() => this.Release();

        #endregion
    }

    public struct Releaser<T> : IReleaser
    {
        public event TypedEventHandler<Releaser<T>, T> ReleaseRaised;
        private readonly T state;

        private void Release() => this.ReleaseRaised?.Invoke(this, this.state);

        public Releaser(bool isAcquired, T state = default(T))
        {
            ReleaseRaised = null;
            this.IsAcquired = isAcquired;
            this.state = state;
        }

        public bool IsAcquired { get; }

        public Releaser<T> AcquiredCallback([NotNull] Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (this.IsAcquired) action();
            return this;
        }

        #region Implementation of IDisposable

        /// <summary>执行与释放或重置非托管资源相关的应用程序定义的任务。</summary>
        public void Dispose() => this.Release();

        #endregion
    }
}