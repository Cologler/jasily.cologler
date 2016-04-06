using JetBrains.Annotations;

namespace System.Collections.Generic
{
    public static class ItemExtensions
    {
        public static T BeAdd<T>(this T obj, [NotNull] ICollection<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            collection.Add(obj);
            return obj;
        }
    }
}