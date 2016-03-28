namespace System.Collections.Generic
{
    public static class ArrayExtensions
    {
        public static int IndexOf<T>(this T[] array, T value) => Array.IndexOf(array, value);
    }
}