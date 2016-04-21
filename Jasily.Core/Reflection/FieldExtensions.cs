using System.Linq.Expressions;
using JetBrains.Annotations;

namespace System.Reflection
{
    public static class FieldExtensions
    {
        #region FieldInfo

        public static Getter<TObject, TField> CompileGetter<TObject, TField>([NotNull] this FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            if (typeof(TObject) != field.DeclaringType) throw new InvalidOperationException();
            if (typeof(TField) != field.FieldType) throw new InvalidOperationException();

            var obj = Expression.Parameter(typeof(TObject));
            var accessor = Expression.Field(obj, field);
            return new Getter<TObject, TField>(Expression.Lambda<Func<TObject, TField>>(accessor, obj).Compile());
        }

        public static Getter<object, object> CompileGetter([NotNull] this FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            var obj = Expression.Parameter(typeof(object));
            var accessor = Expression.MakeMemberAccess(Expression.Convert(obj, field.DeclaringType), field);
            return new Getter<object, object>(
                Expression.Lambda<Func<object, object>>(Expression.Convert(accessor, typeof(object)), obj
            ).Compile());
        }

        public static Setter<TObject, TField> CompileSetter<TObject, TField>([NotNull] this FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            if (typeof(TObject) != field.DeclaringType) throw new InvalidOperationException();
            if (typeof(TField) != field.FieldType) throw new InvalidOperationException();

            var obj = Expression.Parameter(typeof(TObject));
            var value = Expression.Parameter(typeof(TField));
            var accessor = Expression.Field(obj, field);
            var assign = Expression.Assign(accessor, value);
            return new Setter<TObject, TField>(Expression.Lambda<Action<TObject, TField>>(assign, obj, value).Compile());
        }

        public static Setter<object, object> CompileSetter([NotNull] this FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            var obj = Expression.Parameter(typeof(object));
            var value = Expression.Parameter(typeof(object));
            var accessor = Expression.Field(Expression.Convert(obj, field.DeclaringType), field);
            var assign = Expression.Assign(accessor, Expression.Convert(value, field.FieldType));
            return new Setter<object, object>(Expression.Lambda<Action<object, object>>(assign, obj, value).Compile());
        }

        #endregion
    }
}