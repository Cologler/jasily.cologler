using System;
using System.Reflection;
using Jasily.Data.SQLBuilder.Attributes;

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
            if (this.Property != null)
            {
                this.Property.SetValue(obj, TryConvertValue(value, this.ColumnType));
            }
            else
            {
                this.Field.SetValue(obj, TryConvertValue(value, this.ColumnType));
            }
        }

        private static object TryConvertValue(object value, Type type)
        {
            if (value == null)
                return null;

            var valueType = value.GetType();

            if (valueType == type)
                return value;

            switch (valueType.FullName)
            {
                case "System.DBNull":
                    return null;

                case "System.Int64":
                    var Int64 = (long)value;
                    if (type == typeof(int))
                    {
                        if (Int64 <= Int32.MaxValue && Int64 >= Int32.MinValue)
                            return Convert.ToInt32(Int64);
                    }
                    break;

            }

            throw new NotSupportedException();
        }

        public object GetValue(object obj)
        {
            return this.Property != null ? this.Property.GetValue(obj) : this.Field.GetValue(obj);
        }

        internal void BuildMetaData()
        {
            this.ColumnTypeName = GetSQLiteTypeName(this.ColumnType);
        }

        private static string GetSQLiteTypeName(Type type)
        {
            if (type == typeof(string))
            {
                return "TEXT";
            }
            else if (type == typeof(int) || type == typeof(long) || type == typeof(bool))
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