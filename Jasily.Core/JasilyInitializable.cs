namespace System
{
    public static class JasilyInitializable
    {
        public static T InitializeInstance<T>(this T obj)
            where T : IInitializable<T>
        {
            return obj.InitializeInstance(obj);
        }
    }
}