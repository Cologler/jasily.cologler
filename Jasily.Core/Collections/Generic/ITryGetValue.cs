namespace System.Collections.Generic
{
    public interface ITryGetValue<in TKey, TValue>
    {
        bool TryGetValue(TKey key, out TValue value);
    }
}