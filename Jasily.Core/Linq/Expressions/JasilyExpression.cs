
namespace System.Linq.Expressions
{
    public static class JasilyExpression
    {
        /// <summary>
        /// parse path
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertySelector"></param>
        /// <returns></returns>
        public static string ParsePathFromPropertySelector<T>(this Expression<Func<T, object>> propertySelector)
        {
            if (propertySelector.Body.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException("propertySelector only can select property from current type.");

            return ParsePathFromMemberAccessExpression(propertySelector.Body);
        }

        /// <summary>
        /// parse path
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static string ParsePathFromMemberAccessExpression(Expression expression)
        {
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                var member = expression as MemberExpression;
                var baseMember = ParsePathFromMemberAccessExpression(member.Expression);
                return String.Concat(baseMember == null ? null : String.Concat(baseMember, "."), member.Member.Name);
            }

            return null;
        }
    }
}
