namespace System
{
    public static class DateTimeExtensions
    {
        public static readonly DateTime DateTime1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static TimeSpan ToUnixTimeSpan(this DateTime dt)
        {
            return dt - DateTime1970;
        }
    }
}