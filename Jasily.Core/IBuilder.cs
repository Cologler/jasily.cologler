using System.Diagnostics.Contracts;

namespace System
{
    public interface IBuilder<out T>
    {
        T Build();
    }
}