using JetBrains.Annotations;

namespace System
{
    public struct Disposer : IDisposable
    {
        private readonly Action target;

        public Disposer([NotNull] Action target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            this.target = target;
        }

        public void Dispose() => this.target();
    }
}
