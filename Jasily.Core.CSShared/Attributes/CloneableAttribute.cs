using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace System.Attributes
{
    /// <summary>
    /// 设置指定的属性或字段需要被克隆
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class)]
    public class CloneableAttribute : Attribute
    {
        /// <summary>
        /// return dest.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Clone<T>(T source)
        {
            return (T)ObjectClone(source);
        }

        private static object ObjectClone(object source)
        {
            if (source == null) return null;

            var type = source.GetType();

            if (type.IsPrimitive ||
                type == typeof(string) ||
                type == typeof(decimal)) return source;

            if (type.GetCustomAttribute<CloneableAttribute>() != null)
            {
                var dest = FormatterServices.GetUninitializedObject(type);

                foreach (var field in
                    from fieldInfo in type.GetRuntimeFields()
                    where fieldInfo.GetCustomAttribute<CloneableAttribute>() != null
                    select fieldInfo)
                {
                    if (field.IsStatic || field.IsInitOnly || field.IsLiteral)
                    {
                        throw new InvalidOperationException();
                    }

                    var value = field.GetValue(source);
                    field.SetValue(dest, ObjectClone(value));
                }

                foreach (var property in
                    from propertyInfo in type.GetRuntimeProperties()
                    where propertyInfo.GetCustomAttribute<CloneableAttribute>() != null
                    select propertyInfo)
                {
                    if (property.CanRead && property.CanWrite)
                    {
                        var value = property.GetValue(source);
                        property.SetValue(dest, ObjectClone(value));
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }

                return dest;
            }

            var clone = source as ICloneable;
            if (clone != null) return clone.Clone();

            return source;
        }
    }
}