using System.Collections.Generic;
using System.Reflection;

namespace System.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class CloneableAttribute : Attribute
    {
        public static T CloneWithCloneableAttribute<T>(T source, T dest)
        {
            var type = typeof (T);
            var fields = type.GetRuntimeFields();
            foreach (var fieldInfo in fields)
            {
                var attr = fieldInfo.GetCustomAttribute<CloneableAttribute>();
                if (attr != null)
                {
                    fieldInfo.SetValue(dest, fieldInfo.GetValue(source));
                }
            }
            var properties = type.GetRuntimeProperties();
            foreach (var propertyInfo in properties)
            {
                var attr = propertyInfo.GetCustomAttribute<CloneableAttribute>();
                if (attr != null)
                {
                    propertyInfo.SetValue(dest, propertyInfo.GetValue(source));
                }
            }
            return dest;
        }
    }
}