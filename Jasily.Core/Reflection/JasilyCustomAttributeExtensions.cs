namespace System.Reflection
{
    public static class JasilyCustomAttributeExtensions
    {
        public static bool HasCustomAttribute<T>(this Module element) where T : Attribute
            => element.GetCustomAttribute<T>() != null;

        public static bool HasCustomAttribute<T>(this ParameterInfo element) where T : Attribute
            => element.GetCustomAttribute<T>() != null;

        public static bool HasCustomAttribute<T>(this MemberInfo element) where T : Attribute
            => element.GetCustomAttribute<T>() != null;

        public static bool HasCustomAttribute<T>(this Assembly element) where T : Attribute
            => element.GetCustomAttribute<T>() != null;

        public static bool HasCustomAttribute<T>(this ParameterInfo element, bool inherit) where T : Attribute
            => element.GetCustomAttribute<T>() != null;

        public static bool HasCustomAttribute<T>(this MemberInfo element, bool inherit) where T : Attribute
            => element.GetCustomAttribute<T>() != null;
    }
}