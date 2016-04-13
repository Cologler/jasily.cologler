using System.Threading;

namespace System
{
    public static class Singleton
    {
        public static T Instance<T>() where T : class, new()
        {
            if (Shared<T>.instance == null)
            {
                Interlocked.CompareExchange(ref Shared<T>.instance, new T(), null);
            }
            return Shared<T>.instance;
        }

        public static T ThreadStaticInstance<T>() where T : class, new()
            => ThreadStatic<T>.instance ?? (ThreadStatic<T>.instance = new T());

        private static class Shared<T>
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