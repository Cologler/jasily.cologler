using System.Threading;

namespace System
{
    public class Singleton
    {
        public static T Instance<T>() where T : class, new()
        {
            if (Item<T>.instance == null)
            {
                Interlocked.CompareExchange(ref Item<T>.instance, new T(), null);
            }
            return Item<T>.instance;
        }

        public static T ThreadStaticInstance<T>() where T : class, new()
            => ThreadStatic<T>.instance ?? (ThreadStatic<T>.instance = new T());

        private static class Item<T>
        {
            // ReSharper disable once InconsistentNaming
            public static T instance;
        }

        private static class ThreadStatic<T>
        {
            [ThreadStatic]
            // ReSharper disable once InconsistentNaming
            public static T instance;
        }
    }
}