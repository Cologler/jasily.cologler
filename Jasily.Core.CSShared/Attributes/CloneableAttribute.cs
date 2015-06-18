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
            foreach (var fieldInfo in 
                from fieldInfo in fields.Where(z => !z.IsStatic && !z.IsInitOnly && !z.IsLiteral)
                where fieldInfo.GetCustomAttribute<CloneableAttribute>() != null
                select fieldInfo)
            {
#if DESKTOP
                var value = fieldInfo.GetValue(source);
                var clone = value as ICloneable;
                fieldInfo.SetValue(dest, clone != null ? clone.Clone() : value);
#else
                fieldInfo.SetValue(dest, fieldInfo.GetValue(source));
#endif
            }

            var properties = type.GetRuntimeProperties();
            foreach (var propertyInfo in
                from propertyInfo in properties.Where(z => z.CanRead && z.CanWrite)
                where propertyInfo.GetCustomAttribute<CloneableAttribute>() != null
                select propertyInfo)
            {
#if DESKTOP
                var value = propertyInfo.GetValue(source);
                var clone = value as ICloneable;
                propertyInfo.SetValue(dest, clone != null ? clone.Clone() : value);
#else
                propertyInfo.SetValue(dest, propertyInfo.GetValue(source));
#endif
            }

            return dest;
        }
    }
}