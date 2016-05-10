using System;

namespace Jasily.Threading
{
    public interface ISemaphore
    {
        int CurrentCount { get; }

        int Release(int count = 1);

        Releaser<int> Acquire(int count = 1);
    }
}