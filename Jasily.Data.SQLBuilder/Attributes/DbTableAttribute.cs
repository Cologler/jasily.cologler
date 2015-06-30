using System;

namespace Jasily.Data.SQLBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableAttribute : Attribute
    {
        public DbTableAttribute(string tableName = null)
        {
            this.TableName = tableName;
        }

        public string TableName { get; private set; }
    }
}