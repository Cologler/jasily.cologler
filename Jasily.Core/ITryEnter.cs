namespace System
{
    public interface ITryEnter : IDisposable
    {
        bool IsEntered { get; }
    }
}