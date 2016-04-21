using System.Linq;
using JetBrains.Annotations;

namespace System.Reflection
{
    public static class TypeExtensions
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
                    type.GenericTypeArguments.Select(GetCSharpName).JoinWith(", "))
                : type.Name;
        }

        public static Getter<TObject, TMember> CompileGetter<TObject, TMember>([NotNull] this Type type, string memberName)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var field = type.GetRuntimeField(memberName);
            if (field != null) return field.CompileGetter<TObject, TMember>();
            var property = type.GetRuntimeProperty(memberName);
            return property?.CompileGetter<TObject, TMember>();
        }

        public static Getter<object, object> CompileGetter([NotNull] this Type type, string memberName)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var field = type.GetRuntimeField(memberName);
            if (field != null) return field.CompileGetter();
            var property = type.GetRuntimeProperty(memberName);
            return property?.CompileGetter();
        }

        public static Setter<TObject, TMember> CompileSetter<TObject, TMember>([NotNull] this Type type, string memberName)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var field = type.GetRuntimeField(memberName);
            if (field != null) return field.CompileSetter<TObject, TMember>();
            var property = type.GetRuntimeProperty(memberName);
            return property?.CompileSetter<TObject, TMember>();
        }

        public static Setter<object, object> CompileSetter([NotNull] this Type type, string memberName)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var field = type.GetRuntimeField(memberName);
            if (field != null) return field.CompileSetter();
            var property = type.GetRuntimeProperty(memberName);
            return property?.CompileSetter();
        }
    }
}