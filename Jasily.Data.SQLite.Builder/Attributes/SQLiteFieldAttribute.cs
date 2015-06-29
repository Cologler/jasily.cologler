using System;

namespace Jasily.Data.SQLite.Builder.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SQLiteFieldAttribute : Attribute
    {
        public SQLiteFieldAttribute(string fieldName = null)
        {
            this.FieldName = fieldName;
        }

        public string FieldName { get; private set; }

        public SQLiteFieldFlags FieldFlags { get; set; }
    }
}