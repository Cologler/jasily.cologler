namespace System
{
#if !DESKTOP && !COMMON
    public interface ICloneable
    {
        object Clone();
    }
#endif

    public interface ICloneable<out T>
        : ICloneable
    {
        new T Clone();
    }
}
