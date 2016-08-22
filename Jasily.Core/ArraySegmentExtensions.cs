namespace System
{
    public static class ArraySegmentExtensions
    {
        public static bool CheckValid<T>(this ArraySegment<T> segment) => segment.Array != null;

        public static ArraySegment<T> ThrowIfInvalid<T>(this ArraySegment<T> segment, string paramName)
        {
            if (!segment.CheckValid())
            {
                throw new ArgumentException("ArraySegment is invalid", paramName ?? nameof(segment));
            }
            return segment;
        }

        public static T Index<T>(this ArraySegment<T> segment, int index)
        {
            segment.ThrowIfInvalid(nameof(segment));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (index >= segment.Count) throw new IndexOutOfRangeException();
            return segment.Array[segment.Offset + index];
        }
    }
}