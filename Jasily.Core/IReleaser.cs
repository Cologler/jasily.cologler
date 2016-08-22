namespace System
{
    public interface IReleaser : IDisposable
    {
        bool IsAcquired { get; }
    }
}