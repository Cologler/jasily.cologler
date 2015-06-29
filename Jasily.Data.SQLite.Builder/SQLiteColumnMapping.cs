using System;
using System.Reflection;
using System.Text;
using Jasily.Data.SQLite.Builder.Attributes;

namespace Jasily.Data.SQLite.Builder
{
    internal class SQLiteColumnMapping
    {
        public string ColumnName { get; private set; }

        public Type ColumnType { get; private set; }

        public string ColumnTypeName { get; private set; }

        public PropertyInfo Property { get; set; }

        public FieldInfo Field { get; set; }

        public SQLiteFieldAttribute FieldAttribute { get; private set; }

        public SQLiteFieldPrimaryKeyAttribute PrimaryKeyAttribute { get; private set; }

        internal SQLiteColumnMapping(FieldInfo field, SQLiteFieldAttribute attr)
        {
            this.FieldAttribute = attr;
            this.Field = field;
            this.ColumnType = field.FieldType;
            this.ColumnName = attr.FieldName ?? field.Name;
            this.PrimaryKeyAttribute = field.GetCustomAttribute<SQLiteFieldPrimaryKeyAttribute>();
        }

        internal SQLiteColumnMapping(PropertyInfo property, SQLiteFieldAttribute attr)
        {
            this.FieldAttribute = attr;
            this.Property = property;
            this.ColumnType = property.PropertyType;
            this.ColumnName = attr.FieldName ?? property.Name;
            this.PrimaryKeyAttribute = property.GetCustomAttribute<SQLiteFieldPrimaryKeyAttribute>();
        }

        public void SetValue(object obj, object value)
        {
            if (this.Property != null)
            {
                this.Property.SetValue(obj, value);
            }
            else
            {
                this.Field.SetValue(obj, value);
            }
        }

        public object GetValue(object obj)
        {
            return this.Property != null ? this.Property.GetValue(obj) : this.Field.GetValue(obj);
        }

        public void BuildMetaData()
        {
            this.ColumnTypeName = GetSQLiteTypeName(this.ColumnType);
        }

        private static string GetSQLiteTypeName(Type type)
        {
            if (type == typeof(string))
            {
                return "TEXT";
            }
            else if (type == typeof(int) || type == typeof(bool))
            {
                return "INTEGER";
            }
            else
            {
                throw new NotSupportedException(String.Format("type {0} was not a SQLite type.", type.Name));
            }
        }
    }
}