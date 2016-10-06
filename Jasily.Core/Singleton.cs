namespace System
{
    public static class Singleton
    {
        public static T Instance<T>() where T : class, new()
        {
            if (Shared<T>.instance == null)
            {
                lock (typeof(Shared<T>))
                {
                    if (Shared<T>.instance == null)
                    {
                        Shared<T>.instance = new T();
                    }
                }
            }

            return Shared<T>.instance;
        }

        private static class Shared<T> where T : class
        {
            // ReSharper disable once InconsistentNaming
            public static T instance;
        }

        #region thread static

        public static T ThreadStaticInstance<T>() where T : class, new()
            => ThreadStatic<T>.instance ?? (ThreadStatic<T>.instance = new T());

        private static class ThreadStatic<T> where T : class
        {
            [ThreadStatic]
            // ReSharper disable once InconsistentNaming
            public static T instance;
        }

        #endregion
    }

    public static class Singleton<T>
    {
        public static T Instance;

        [ThreadStatic]
        // ReSharper disable once InconsistentNaming
        public static T ThreadStaticInstance;
    }
}