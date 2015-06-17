namespace System
{
    public static class UfDateTimeConverter
    {
        public static readonly DateTime DateTime1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime UnixMillisecondsToDateTime(long milliseconds)
        {
            return DateTime1970.AddMilliseconds(milliseconds);
        }

        public static TimeSpan UnixMillisecondsToTimeSpan(long milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }
    }
}