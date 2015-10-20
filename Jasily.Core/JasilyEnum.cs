namespace System
{
    public static class JasilyEnum
    {
        public static T[] GetValues<T>() => Enum.GetValues(typeof(T)) as T[];
    }
}