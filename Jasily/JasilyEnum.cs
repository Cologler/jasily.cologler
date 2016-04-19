using System;

namespace Jasily
{
    public class JasilyEnum
    {
        public static T[] GetValues<T>() => Enum.GetValues(typeof(T)) as T[];
    }
}