using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jasily
{
    public static class JasilyDayOfWeekExtensions
    {
        public static JasilyDayOfWeek ToJasilyDayOfWeek(this DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday: return JasilyDayOfWeek.Sunday;
                case DayOfWeek.Monday: return JasilyDayOfWeek.Monday;
                case DayOfWeek.Tuesday: return JasilyDayOfWeek.Tuesday;
                case DayOfWeek.Wednesday: return JasilyDayOfWeek.Wednesday;
                case DayOfWeek.Thursday: return JasilyDayOfWeek.Thursday;
                case DayOfWeek.Friday: return JasilyDayOfWeek.Friday;
                case DayOfWeek.Saturday: return JasilyDayOfWeek.Saturday;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static JasilyDayOfWeek ToJasilyDayOfWeek([NotNull] this DayOfWeek[] dayOfWeeks)
        {
            if (dayOfWeeks == null) throw new ArgumentNullException(nameof(dayOfWeeks));

            return dayOfWeeks.Length == 0
                ? JasilyDayOfWeek.None
                : dayOfWeeks.Aggregate(JasilyDayOfWeek.None, (current, dayOfWeek) => current.Or(dayOfWeek));
        }

        public static JasilyDayOfWeek Or(this JasilyDayOfWeek dayOfWeek, JasilyDayOfWeek other)
            => dayOfWeek | other;

        public static JasilyDayOfWeek Or(this JasilyDayOfWeek dayOfWeek, DayOfWeek other)
            => dayOfWeek.Or(other.ToJasilyDayOfWeek());

        public static JasilyDayOfWeek Or(this DayOfWeek dayOfWeek, DayOfWeek other)
            => dayOfWeek.ToJasilyDayOfWeek().Or(other.ToJasilyDayOfWeek());

        public static DayOfWeek[] ToDayOfWeek(this JasilyDayOfWeek dayOfWeek)
        {
            var list = new List<DayOfWeek>(7);
            if ((dayOfWeek & JasilyDayOfWeek.Sunday) == JasilyDayOfWeek.Sunday) list.Add(DayOfWeek.Sunday);
            if ((dayOfWeek & JasilyDayOfWeek.Monday) == JasilyDayOfWeek.Monday) list.Add(DayOfWeek.Monday);
            if ((dayOfWeek & JasilyDayOfWeek.Tuesday) == JasilyDayOfWeek.Tuesday) list.Add(DayOfWeek.Tuesday);
            if ((dayOfWeek & JasilyDayOfWeek.Wednesday) == JasilyDayOfWeek.Wednesday) list.Add(DayOfWeek.Wednesday);
            if ((dayOfWeek & JasilyDayOfWeek.Thursday) == JasilyDayOfWeek.Thursday) list.Add(DayOfWeek.Thursday);
            if ((dayOfWeek & JasilyDayOfWeek.Friday) == JasilyDayOfWeek.Friday) list.Add(DayOfWeek.Friday);
            if ((dayOfWeek & JasilyDayOfWeek.Saturday) == JasilyDayOfWeek.Saturday) list.Add(DayOfWeek.Saturday);
            return list.ToArray();
        }
    }
}