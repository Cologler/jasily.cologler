using System.Diagnostics.Contracts;

namespace System
{
    public interface IBuilder<T>
    {
        T Build();
    }
}