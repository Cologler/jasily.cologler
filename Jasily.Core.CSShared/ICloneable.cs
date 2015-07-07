namespace System
{
#if !DESKTOP
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
