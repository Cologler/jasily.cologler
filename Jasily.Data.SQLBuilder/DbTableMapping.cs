using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jasily.Data.SQLBuilder.Attributes;

namespace Jasily.Data.SQLBuilder
{
    public class DbTableMapping
    {
        private readonly Dictionary<string, DbColumnMapping> columns;

        internal DbTableMapping(Type entityType, DbTableAttribute attr)
        {
            this.columns = new Dictionary<string, DbColumnMapping>();

            this.EntityType = entityType;
            this.TableAttribute = attr;
            this.TableName = attr.TableName ?? this.EntityType.Name;
        }

        public Type EntityType { get; private set; }

        public DbTableAttribute TableAttribute { get; private set; }

        public string TableName { get; private set; }

        public DbColumnMapping PrimaryKeyColums { get; private set; }

        internal void MapColumns()
        {
            foreach (var property in this.EntityType.GetRuntimeProperties())
            {
                var attr = property.GetCustomAttribute<DbFieldAttribute>();
                if (attr != null)
                {
                    if (!property.CanRead)
                        throw new NotSupportedException(String.Format("property {0} must can read.", property.Name));
                    if (!property.CanWrite)
                        throw new NotSupportedException(String.Format("property {0} must can write.", property.Name));

                    var map = new DbColumnMapping(property, attr);

                    if (this.columns.ContainsKey(map.ColumnName))
                        throw new Exception(String.Format("name of property {0} [{1}] already exists.", property.Name, map.ColumnName));

                    map.BuildMetaData();
                    this.columns.Add(map.ColumnName, map);
                }
            }

            foreach (var field in this.EntityType.GetRuntimeFields())
            {
                var attr = field.GetCustomAttribute<DbFieldAttribute>();
                if (attr != null)
                {
                    if (field.IsStatic)
                        throw new NotSupportedException(String.Format("field {0} must be instance field.", field.Name));
                    if (field.IsLiteral)
                        throw new NotSupportedException(String.Format("field {0} can not be literal.", field.Name));
                    if (field.IsInitOnly)
                        throw new NotSupportedException(String.Format("field {0} can not init only.", field.Name));

                    var map = new DbColumnMapping(field, attr);

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

            if (this.columns.Values.Count(z => z.PrimaryKeyAttribute != null) > 1)
            {
                throw new NotSupportedException("entity not support primary key more then one.");
            }
            else
            {
                this.PrimaryKeyColums = this.columns.Values.FirstOrDefault(z => z.PrimaryKeyAttribute != null);
            }
        }

        public IEnumerable<DbColumnMapping> GetColumns()
        {
            return this.columns.Values.ToArray();
        }

        public DbColumnMapping this[string columnName]
        {
            get { return this.columns.GetValueOrDefault(columnName); }
        }
    }

    internal sealed class SQLiteTableMapping<T> : DbTableMapping
        where T : new()
    {
        internal SQLiteTableMapping(DbTableAttribute attr)
            : base(typeof(T), attr)
        {
        }
    }
}