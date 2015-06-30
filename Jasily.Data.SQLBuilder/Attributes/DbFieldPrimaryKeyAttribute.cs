using System;
using Jasily.Data.SQLBuilder.Enums;

namespace Jasily.Data.SQLBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class DbFieldPrimaryKeyAttribute : DbConflictableAttribute
    {
        public OrderMode? Order { get; set; }

        public bool IsAutoIncrement { get; set; }
    }
}