
namespace System.Linq.Expressions
{
    public static class JasilyExpression
    {
        private static readonly Guid PropertySelectorErrorGuid = Guid.NewGuid();

        /// <summary>
        /// parse path
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertySelector"></param>
        /// <exception cref="System.NotSupportedException">propertySelector only can select property from current type</exception>
        /// <returns></returns>
        public static string PropertySelector<T>(this Expression<Func<T, object>> propertySelector)
        {
            try
            {
                return InnerPropertyPathSelector(propertySelector.Body);
            }
            catch (JasilyException e)
            {
                if (e.Id == PropertySelectorErrorGuid)
                    throw new NotSupportedException("propertySelector only can select property from current type.");
                else
                    throw;
            }
            catch
            {
                throw;
            }
        }

        private static string InnerPropertyPathSelector(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Parameter:
                    return null;

                case ExpressionType.TypeAs:
                case ExpressionType.Convert:
                    return InnerPropertyPathSelector((expression as UnaryExpression).Operand);

                case ExpressionType.ArrayLength:
                    return string.Concat(InnerPropertyPathSelector((expression as UnaryExpression).Operand), ".Length");

                case ExpressionType.MemberAccess:
                    var member = expression as MemberExpression;
                    var baseMember = InnerPropertyPathSelector(member.Expression);
                    return String.Concat(baseMember == null ? null : String.Concat(baseMember, "."), member.Member.Name);

                default:
                    throw new JasilyException(PropertySelectorErrorGuid);
            }
        }
    }
}
