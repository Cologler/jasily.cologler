namespace System.Reflection
{
    public interface ISetter : IAccessor
    {
        void Set(object instance, object value);

        object this[object obj] { set; }
    }
}