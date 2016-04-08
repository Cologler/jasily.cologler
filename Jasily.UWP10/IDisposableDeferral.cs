using System;

namespace Jasily
{
    public interface IDisposableDeferral<out T> : IDisposable
    {
        T Deferral { get; }
    }
}