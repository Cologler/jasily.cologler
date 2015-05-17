using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace System.Diagnostics
{
    public static class DebugPro
    {
        [Conditional("DEBUG")]
        public static void AssertType<T>(this object obj)
        {
            var type = obj.GetType();
            Debug.Assert(obj is T, String.Format("assert object type false. {0} not {1}", type.FullName, typeof(T).FullName));
        }

        [Conditional("DEBUG")]
        public static void AssertType<T>(this Type type)
        {
            Debug.Assert(type == typeof(T), String.Format("assert object type false. {0} not {1}", type.FullName, typeof(T).FullName));
        }
    }
}
