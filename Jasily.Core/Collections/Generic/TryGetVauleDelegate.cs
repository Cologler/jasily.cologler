namespace System.Collections.Generic
{
    public delegate bool TryGetVauleDelegate<in TKey, TValue>(TKey key, out TValue value);
}