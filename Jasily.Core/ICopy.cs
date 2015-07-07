namespace System
{
    /// <summary>
    /// allow class copy member from other instance
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICopy<in T>
    {
        void Copy(T source);
    }
}
