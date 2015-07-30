using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using static System.Diagnostics.Debug;

namespace System.Diagnostics
{
    public static class AttributeTestor
    {
        [Conditional("DEBUG")]
        public static void Test(this IJasilyTestable obj)
        {
            Assert(obj != null);

            Test((object)obj);
        }

        [Conditional("DEBUG")]
        public static void Test(object obj)
        {
            Assert(obj != null);

            var type = obj.GetType();
            foreach (var property in type.GetRuntimeProperties())
            {
                Test<CanBeNullAttribute>(obj, property);
                Test<DateTimeKindAttribute>(obj, property);
            }

            foreach (var field in type.GetRuntimeFields())
            {
                Test<CanBeNullAttribute>(obj, field);
                Test<DateTimeKindAttribute>(obj, field);
            }
        }

        [Conditional("DEBUG")]
        private static void Test<T>(object obj, PropertyInfo info)
            where T : TestAttribute
        {
            var attr = info.GetCustomAttribute<T>();
            if (attr != null && info.CanRead)
            {
                Assert(attr.Test(info.GetValue(obj)));
            }
        }

        [Conditional("DEBUG")]
        private static void Test<T>(object obj, FieldInfo info)
            where T : TestAttribute
        {
            var attr = info.GetCustomAttribute<T>();
            if (attr != null)
            {
                if (info.IsStatic)
                {
                    if (Debugger.IsAttached) Debugger.Break();
                }
                else
                {
                    Assert(attr.Test(info.GetValue(obj)));
                }
            }
        }
    }
}
