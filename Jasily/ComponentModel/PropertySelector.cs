using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Jasily.ComponentModel
{
    public static class PropertySelector
    {
        public static PropertySelector<TProperty> SelectProperty<T, TProperty>(this T obj, [NotNull] Expression<Func<T, TProperty>> selectExpression)
            => PropertySelector<T>.Start(selectExpression);

        public static PropertySelector<TProperty> SelectProperty<T, TProperty>([NotNull] Expression<Func<T, TProperty>> selectExpression)
            => PropertySelector<T>.Start(selectExpression);
    }

    public class PropertySelector<T>
    {
        private readonly string name;

        private PropertySelector(string name)
        {
            this.name = name;
        }

        public static PropertySelector<TProperty> Start<TProperty>([NotNull] Expression<Func<T, TProperty>> selectExpression)
        {
            if (selectExpression == null) throw new ArgumentNullException(nameof(selectExpression));

            return new PropertySelector<T>(null).Select(selectExpression);
        }

        public static PropertySelector<T> Start() => Start(z => z);

        public PropertySelector<TProperty> Select<TProperty>([NotNull] Expression<Func<T, TProperty>> selectExpression)
        {
            if (selectExpression == null) throw new ArgumentNullException(nameof(selectExpression));
            var expression = selectExpression.Body;
            var name = this.Select(expression);
            return new PropertySelector<TProperty>(this.name == null ? name : this.name + "." + name);
        }

        public PropertySelector<TProperty> SelectMany<TProperty>([NotNull] Expression<Func<T, IEnumerable<TProperty>>> selectExpression)
        {
            if (selectExpression == null) throw new ArgumentNullException(nameof(selectExpression));
            var expression = selectExpression.Body;
            var name = this.Select(expression);
            return new PropertySelector<TProperty>(Concat(this.name, name));
        }

        private string Select([NotNull] Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Parameter:
                    return null;

                case ExpressionType.TypeAs:
                case ExpressionType.Convert:
                    return this.Select((UnaryExpression)expression);

                case ExpressionType.ArrayLength:
                    return Concat(this.Select((UnaryExpression)expression), "Length");

                case ExpressionType.MemberAccess:
                    return this.Select((MemberExpression)expression);

                default:
                    throw new NotSupportedException();
            }
        }

        private static string Concat(string left, string right)
        {
            return left == null ? right : (right == null ? left : left + "." + right);
        }

        private string Select([NotNull] UnaryExpression expression) => this.Select(expression.Operand);

        private string Select([NotNull] MemberExpression expression)
        {
            var parentName = this.Select(expression.Expression);
            var memberName = expression.Member.Name;
            return Concat(parentName, memberName);
        }

        public static implicit operator string(PropertySelector<T> selector)
            => selector.ToString();

        public override string ToString() => this.name ?? string.Empty;
    }
}