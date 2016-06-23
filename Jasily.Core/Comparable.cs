namespace System
{
    public static class Comparable
    {
        public static T Max<T>(this T first, T second) where T : IComparable<T> => first.CompareTo(second) < 0 ? second : first;

        public static T Min<T>(this T first, T second) where T : IComparable<T> => first.CompareTo(second) < 0 ? first : second;
    }
}