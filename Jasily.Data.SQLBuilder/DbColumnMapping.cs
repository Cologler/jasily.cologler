using System;
using System.Reflection;
using Jasily.Data.SQLBuilder.Attributes;
using Jasily.Data.SQLBuilder.DbProvider.SQLite;

namespace Jasily.Data.SQLBuilder
{
    public class DbColumnMapping
    {
        public string ColumnName { get; private set; }

        public Type ColumnType { get; private set; }

        public string ColumnTypeName { get; private set; }

        public PropertyInfo Property { get; set; }

        public FieldInfo Field { get; set; }

        public DbFieldAttribute FieldAttribute { get; private set; }

        public DbFieldPrimaryKeyAttribute PrimaryKeyAttribute { get; private set; }

        internal DbColumnMapping(FieldInfo field, DbFieldAttribute attr)
        {
            this.FieldAttribute = attr;
            this.Field = field;
            this.ColumnType = field.FieldType;
            this.ColumnName = attr.FieldName ?? field.Name;
            this.PrimaryKeyAttribute = field.GetCustomAttribute<DbFieldPrimaryKeyAttribute>();
        }

        internal DbColumnMapping(PropertyInfo property, DbFieldAttribute attr)
        {
            this.FieldAttribute = attr;
            this.Property = property;
            this.ColumnType = property.PropertyType;
            this.ColumnName = attr.FieldName ?? property.Name;
            this.PrimaryKeyAttribute = property.GetCustomAttribute<DbFieldPrimaryKeyAttribute>();
        }

        public void SetValue(object obj, object value)
        {
            var sqlite = new SQLite();

            if (this.Property != null)
            {
                this.Property.SetValue(obj, sqlite.ConvertValue(value, this.ColumnType));
            }
            else
            {
                this.Field.SetValue(obj, sqlite.ConvertValue(value, this.ColumnType));
            }
        }

        public object GetValue(object obj)
        {
            return this.Property != null ? this.Property.GetValue(obj) : this.Field.GetValue(obj);
        }

        internal void BuildMetaData()
        {
            this.ColumnTypeName = new SQLite().GetDbTypeName(this.ColumnType);
        }
    }
}