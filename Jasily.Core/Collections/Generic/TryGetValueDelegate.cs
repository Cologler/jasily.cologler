namespace System.Collections.Generic
{
    public delegate bool TryGetValueDelegate<in TKey, TValue>(TKey key, out TValue value);
}