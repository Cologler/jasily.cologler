namespace System.Reflection
{
    public interface IGetter : IAccessor
    {
        object Get(object instance);

        object this[object instance] { get; }
    }
}