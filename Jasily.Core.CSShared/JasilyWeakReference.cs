
namespace System
{
    public static class JasilyWeakReference
    {
        public static T GetTarget<T>(this WeakReference<T> weakRef)
            where T : class
        {
            T r;
            if (weakRef.TryGetTarget(out r))
                return r;
            else
                return null;
        }
    }
}
