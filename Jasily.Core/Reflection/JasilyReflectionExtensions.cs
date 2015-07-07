using System.Linq;

namespace System.Reflection
{
    public static class JasilyReflectionExtensions
    {
        /// <summary>
        /// get getter from field or property
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Func<object, object> GetGetter(this Type type, string name)
        {
            var f = type.GetRuntimeField(name);
            if (f != null) return f.GetValue;
            var p = type.GetRuntimeProperty(name);
            if (p != null) return p.GetValue;
            return null;
        }

        /// <summary>
        /// get setter from field or property
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Action<object, object> GetSetter(this Type type, string name)
        {
            var f = type.GetRuntimeField(name);
            if (f != null) return f.SetValue;
            var p = type.GetRuntimeProperty(name);
            if (p != null) return p.SetValue;
            return null;
        }

        public static string GetCSharpName(this Type type)
        {
            return type.IsConstructedGenericType
                ? String.Format("{0}<{1}>",
                    type.Name.Substring(0, type.Name.IndexOf("`", StringComparison.Ordinal)),
                    type.GenericTypeArguments.Select(GetCSharpName).AsLines(", "))
                : type.Name;
        }
    }
}
