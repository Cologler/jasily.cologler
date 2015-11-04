
namespace System
{
    public static class WeakReferenceExtensions
    {
        /// <summary>
        /// get target (return null if get failed) .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="weakRef"></param>
        /// <returns></returns>
        public static T GetTargetOrNull<T>(this WeakReference<T> weakRef)
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
