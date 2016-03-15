using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Jasily.Diagnostics.AttributeTest
{
    public static class AttributeTestor
    {
        [Conditional("DEBUG")]
        public static void Test(this IJasilyTestable obj)
        {
            Test((object)obj);
        }

        [Conditional("DEBUG")]
        public static void Test(object obj, bool canBeNull = false)
        {
            if (obj == null)
            {
                Debug.Assert(canBeNull);
                return;
            }

            var type = obj.GetType();
            foreach (var property in type.GetRuntimeProperties()
                .Where(z => z.CanRead))
            {
                foreach (var attr in property.GetCustomAttributes<TestAttribute>())
                {
                    Debug.Assert(attr.Test(property.GetValue(obj)));
                }
            }

            foreach (var field in type.GetRuntimeFields())
            {
                foreach (var attr in field.GetCustomAttributes<TestAttribute>())
                {
                    Debug.Assert(attr.Test(field.GetValue(obj)));
                }
            }
        }
    }
}
