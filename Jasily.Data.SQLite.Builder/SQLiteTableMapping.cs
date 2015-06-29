using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Jasily.Data.SQLite.Builder.Attributes;

namespace Jasily.Data.SQLite.Builder
{
    public class SQLiteTableMapping
    {
        private readonly Dictionary<string, SQLiteColumnMapping> columns;

        internal SQLiteTableMapping(Type entityType, SQLiteTableAttribute attr)
        {
            this.columns = new Dictionary<string, SQLiteColumnMapping>();

            this.EntityType = entityType;
            this.TableAttribute = attr;
            this.TableName = attr.TableName ?? this.EntityType.Name;
        }

        public Type EntityType { get; private set; }

        public SQLiteTableAttribute TableAttribute { get; private set; }

        public string TableName { get; private set; }

        internal void MapColumns()
        {
            foreach (var property in this.EntityType.GetRuntimeProperties())
            {
                var attr = property.GetCustomAttribute<SQLiteFieldAttribute>();
                if (attr != null)
                {
                    if (!property.CanRead)
                        throw new NotSupportedException(String.Format("property {0} must can read.", property.Name));
                    if (!property.CanWrite)
                        throw new NotSupportedException(String.Format("property {0} must can write.", property.Name));

                    var map = new SQLiteColumnMapping(property, attr);

                    if (this.columns.ContainsKey(map.ColumnName))
                        throw new Exception(String.Format("name of property {0} [{1}] already exists.", property.Name, map.ColumnName));

                    map.BuildMetaData();
                    this.columns.Add(map.ColumnName, map);
                }
            }

            foreach (var field in this.EntityType.GetRuntimeFields())
            {
                var attr = field.GetCustomAttribute<SQLiteFieldAttribute>();
                if (attr != null)
                {
                    if (field.IsStatic)
                        throw new NotSupportedException(String.Format("field {0} must be instance field.", field.Name));
                    if (field.IsLiteral)
                        throw new NotSupportedException(String.Format("field {0} can not be literal.", field.Name));
                    if (field.IsInitOnly)
                        throw new NotSupportedException(String.Format("field {0} can not init only.", field.Name));

                    var map = new SQLiteColumnMapping(field, attr);

                    if (this.columns.ContainsKey(map.ColumnName))
                        throw new Exception(String.Format("name of field {0} [{1}] already exists.", field.Name, map.ColumnName));

                    map.BuildMetaData();
                    this.columns.Add(map.ColumnName, map);
                }
            }

            if (this.columns.Count == 0)
            {
                throw new NotSupportedException("entity must contain one column.");
            }
        }

        public string BuildFieldsDefinitions()
        {
            //var fieldsDef = this.columns.Values.Select(z =>
            //    String.Format("{0} {1}{2}{3}",
            //        z.ColumnName,
            //        z.FieldTypeNameName,
            //        z.IsPrimaryKey ? " PRIMARY KEY" : "",
            //        z.IsAutoIncrement ? " AUTOINCREMENT" : ""));

            return String.Join(", ", "");
        }

        public IEnumerable<SQLiteColumnMapping> GetColumns()
        {
            return this.columns.Values.ToArray();
        }

        public SQLiteColumnMapping this[string columnName]
        {
            get { return this.columns.GetValueOrDefault(columnName); }
        }
    }

    internal sealed class SQLiteTableMapping<T> : SQLiteTableMapping
        where T : new()
    {
        internal SQLiteTableMapping(SQLiteTableAttribute attr)
            : base(typeof(T), attr)
        {
        }
    }
}