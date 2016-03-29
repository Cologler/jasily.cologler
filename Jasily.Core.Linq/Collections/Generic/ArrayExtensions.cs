namespace System.Collections.Generic
{
    public static class ArrayExtensions
    {
        public static int IndexOf<T>(this T[] array, T value) => Array.IndexOf(array, value);

        public static T[] ToArray<T>(this T[] array)
        {
            var ret = new T[array.Length];
            Array.Copy(array, ret, array.Length);
            return ret;
        }
    }
}