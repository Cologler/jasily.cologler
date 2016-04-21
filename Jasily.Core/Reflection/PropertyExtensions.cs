using System.Linq.Expressions;
using JetBrains.Annotations;

namespace System.Reflection
{
    public static class PropertyExtensions
    {
        public static Getter<TObject, TProperty> CompileGetter<TObject, TProperty>([NotNull] this PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (typeof(TObject) != property.DeclaringType) throw new InvalidOperationException();
            if (typeof(TProperty) != property.PropertyType) throw new InvalidOperationException();

            var argObj = Expression.Parameter(typeof(TObject));
            var accessor = Expression.Property(argObj, property);
            return new Getter<TObject, TProperty>(Expression.Lambda<Func<TObject, TProperty>>(accessor, argObj).Compile());
        }

        public static Getter<object, object> CompileGetter([NotNull] this PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var obj = Expression.Parameter(typeof(object));
            var accessor = Expression.MakeMemberAccess(Expression.Convert(obj, property.DeclaringType), property);
            return new Getter<object, object>(
                Expression.Lambda<Func<object, object>>(Expression.Convert(accessor, typeof(object)), obj
            ).Compile());
        }

        public static Setter<TObject, TProperty> CompileSetter<TObject, TProperty>([NotNull] this PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (typeof(TObject) != property.DeclaringType) throw new InvalidOperationException();
            if (typeof(TProperty) != property.PropertyType) throw new InvalidOperationException();

            var argObj = Expression.Parameter(typeof(TObject));
            var argInput = Expression.Parameter(typeof(TProperty));
            var accessor = Expression.Property(argObj, property);
            var assign = Expression.Assign(accessor, argInput);
            return new Setter<TObject, TProperty>(Expression.Lambda<Action<TObject, TProperty>>(assign, argObj, argInput).Compile());
        }

        public static Setter<object, object> CompileSetter([NotNull] this PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var obj = Expression.Parameter(typeof(object));
            var value = Expression.Parameter(typeof(object));
            var accessor = Expression.Property(Expression.Convert(obj, property.DeclaringType), property);
            var assign = Expression.Assign(accessor, Expression.Convert(value, property.PropertyType));
            return new Setter<object, object>(Expression.Lambda<Action<object, object>>(assign, obj, value).Compile());
        }
    }
}