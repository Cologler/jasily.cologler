using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Attributes
{
    /// <summary>
    /// 设置指定的属性或字段需要被浅表克隆
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class CloneableAttribute : Attribute
    {
        /// <summary>
        /// return dest.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static T Clone<T>(T source, T dest)
        {
            var type = typeof (T);

            var fields = type.GetRuntimeFields();
            foreach (var fieldInfo in fields.Where(z => !z.IsStatic))
            {
                var attr = fieldInfo.GetCustomAttribute<CloneableAttribute>();
                if (attr != null)
                {
                    fieldInfo.SetValue(dest, fieldInfo.GetValue(source));
                }
            }

            var properties = type.GetRuntimeProperties();
            foreach (var propertyInfo in properties.Where(z => z.CanRead && z.CanWrite))
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