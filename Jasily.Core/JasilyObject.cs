﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class JasilyObject
    {
        /// <summary>
        /// if both were not null, using Equals() to check if they equals.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool NormalEquals<T>(this T obj, T other)
        {
            return !((obj == null && other == null) || (obj != null && obj.Equals(other)));
        }
    }
}