using System;

namespace Jasily.Data.SQLite.Builder.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SQLiteTableAttribute : Attribute
    {
        public SQLiteTableAttribute(string tableName = null)
        {
            this.TableName = tableName;
        }

        public string TableName { get; private set; }
    }
}