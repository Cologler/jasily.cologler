using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class EnumExtensions
    {
        /// <summary>
        /// only use to split flag enum!!!
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="@enum"></param>
        /// <param name="canBeZero"></param>
        /// <returns></returns>
        public static IEnumerable<TEnum> SplitFlags<TEnum>(this TEnum @enum, bool canBeZero)
            where TEnum : struct
        {
            var value = (int)(dynamic)@enum;
            var enumArray = (TEnum[])Enum.GetValues(typeof(TEnum));
            if (value == 0)
            {
                if (canBeZero) yield return (TEnum)(dynamic)0;
            }
            foreach (var e in enumArray)
            {
                var ei = (int)(dynamic)e;
                if (ei != 0 && (value & ei) == ei)
                    yield return e;
            }
        }


    }
}
