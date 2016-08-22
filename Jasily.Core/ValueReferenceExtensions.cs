namespace System
{
    public static class ValueReferenceExtensions
    {
        public static ValueReference<T> Reference<T>(T value) where T : struct => value;
    }
}