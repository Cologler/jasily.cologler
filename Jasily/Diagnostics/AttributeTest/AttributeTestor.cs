using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Jasily.Diagnostics.AttributeTest
{
    public static class AttributeTestor
    {
        private static List<Type> FieldTestAttributes;
        private static List<Type> PropertyTestAttributes;

        static AttributeTestor()
        {
            //Initialize();
        }

        [Conditional("DEBUG")]
        public static void Initialize()
        {
            FieldTestAttributes = new List<Type>();
            PropertyTestAttributes = new List<Type>();

            foreach (var typeInfo in typeof(AttributeTestor).GetTypeInfo().Assembly.DefinedTypes)
            {
                if (typeInfo.IsSubclassOf(typeof(TestAttribute)))
                {
                    var attr = typeInfo.GetCustomAttribute<AttributeUsageAttribute>();
                    if (attr == null) throw new NotImplementedException();
                    if ((attr.ValidOn & AttributeTargets.Field) == AttributeTargets.Field)
                        FieldTestAttributes.Add(typeInfo.AsType());
                    if ((attr.ValidOn & AttributeTargets.Property) == AttributeTargets.Property)
                        PropertyTestAttributes.Add(typeInfo.AsType());
                }
            }
        }

        [Conditional("DEBUG")]
        public static void Test(this IJasilyTestable obj)
        {
            Debug.Assert(obj != null);

            Test((object)obj);
        }

        [Conditional("DEBUG")]
        public static void Test(object obj)
        {
            Debug.Assert(obj != null);

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
                Debug.Assert(attr.Test(info.GetValue(obj)));
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
                    Debug.Assert(attr.Test(info.GetValue(obj)));
                }
            }
        }
    }
}
