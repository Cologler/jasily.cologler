using System;

namespace Jasily.Data.SQLite.Builder.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SQLiteFieldPrimaryKeyAttribute : SQLiteConflictableAttribute
    {
        public OrderMode? Order { get; set; }

        public bool IsAutoIncrement { get; set; }
    }
}