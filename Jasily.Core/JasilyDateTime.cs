namespace System
{
    public static class JasilyDateTime
    {
        public static readonly DateTime DateTime1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

        public static TimeSpan ToUnixTimeSpan(this DateTime dt)
        {
            return dt - DateTime1970;
        }
    }
}