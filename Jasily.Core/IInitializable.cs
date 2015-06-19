namespace System
{
    public interface IInitializable<T>
        where T : IInitializable<T>
    {
        T InitializeInstance(T obj);
    }
}