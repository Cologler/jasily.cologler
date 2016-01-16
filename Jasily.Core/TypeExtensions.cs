using System.Reflection;

namespace System
{
    public static class TypeExtensions
    {
        public static object CreateInstance(this Type type) => Activator.CreateInstance(type);

        public static object CreateInstance(this Type type, params object[] args) => Activator.CreateInstance(type, args);

        public static T CreateInstance<T>(this Type type) => (T)Activator.CreateInstance(type);

        public static T CreateInstance<T>(this Type type, params object[] args) => (T)Activator.CreateInstance(type, args);

        public static object GetDefaultValue(this Type type) => type.GetTypeInfo().IsValueType ? Activator.CreateInstance(type) : null;
    }
}