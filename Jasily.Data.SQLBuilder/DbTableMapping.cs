using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jasily.Data.SQLBuilder.Attributes;
using Jasily.Data.SQLBuilder.DbProvider;

namespace Jasily.Data.SQLBuilder
{
    public class DbTableMapping<TDbProvider>
        where TDbProvider : IDbProvider
    {
        private readonly Dictionary<string, DbColumnMapping<IDbProvider>> columns;

        internal DbTableMapping(Type entityType, DbTableAttribute attr)
        {
            this.columns = new Dictionary<string, DbColumnMapping<IDbProvider>>();

            this.EntityType = entityType;
            this.TableAttribute = attr;
            this.TableName = attr.TableName ?? this.EntityType.Name;
        }

        public Type EntityType { get; private set; }

        public DbTableAttribute TableAttribute { get; private set; }

        public string TableName { get; private set; }

        public DbColumnMapping<IDbProvider> PrimaryKeyColums { get; private set; }

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

                    var map = new DbColumnMapping<IDbProvider>(property, attr);

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

                    var map = new DbColumnMapping<IDbProvider>(field, attr);

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

        public IEnumerable<DbColumnMapping<IDbProvider>> GetColumns()
        {
            return this.columns.Values.ToArray();
        }

        public DbColumnMapping<IDbProvider> this[string columnName]
        {
            get { return this.columns.GetValueOrDefault(columnName); }
        }
    }

    internal sealed class DbTableMapping<TDbProvider, TEntity> : DbTableMapping<TDbProvider>
        where TDbProvider : IDbProvider
        where TEntity : new()
    {
        internal DbTableMapping(DbTableAttribute attr)
            : base(typeof(TEntity), attr)
        {
        }
    }
}