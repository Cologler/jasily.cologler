namespace System.Collections.Generic
{
    public interface ITryGetValue<TKey, TValue>
    {
        bool TryGetValue(TKey key, out TValue value);
    }
}