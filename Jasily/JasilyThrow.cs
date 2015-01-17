using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class JasilyThrow
    {
        public static T ThrowIfNull<T>(this T obj, string paramName)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName);
            return obj;
        }
    }
}
