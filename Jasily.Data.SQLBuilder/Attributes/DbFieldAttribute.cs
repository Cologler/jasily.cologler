using System;
using Jasily.Data.SQLBuilder.Enums;

namespace Jasily.Data.SQLBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class DbFieldAttribute : Attribute
    {
        public DbFieldAttribute(string fieldName = null)
        {
            this.FieldName = fieldName;
        }

        public string FieldName { get; set; }

        public SQLiteFieldFlags FieldFlags { get; set; }
    }
}