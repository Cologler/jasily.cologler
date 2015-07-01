using System;
using System.Collections.Generic;
using System.Linq;
using Jasily.Data.SQLBuilder.DbProvider;
using Jasily.Data.SQLBuilder.Enums;

namespace Jasily.Data.SQLBuilder
{
    public class InsertBuilder<TDbProvider, TEntity> : SQLBuilder<TDbProvider, TEntity>
        where TDbProvider : IDbProvider, new()
        where TEntity : new()
    {
        public InsertBuilder(IDbProvider dbProvider)
            : base(dbProvider)
        {
            this.InsertMode = InsertMode.Insert;
        }

        public InsertMode InsertMode { get; set; }

        public string Build()
        {
            var map = this.Mapping;

            var sql = new List<string>();

            sql.Add(this.DbProvider.GetString(this.InsertMode));

            sql.Add("INTO");

            sql.AddRange(this.DatabaseNamePart());

            sql.Add(map.TableName);

            sql.Add("(");

            var columnNames = this.GetColumns(map).Select(column => column.ColumnName).ToList();

            sql.Add(String.Join(", ", columnNames));
            sql.Add(")");

            sql.Add("VALUES");
            sql.Add("(");
            sql.Add(String.Join(", ", Enumerable.Repeat("?", columnNames.Count)));
            sql.Add(")");

            return String.Concat(String.Join(" ", sql), ";");
        }

        public IList<IList<object>> GetParameters(IEnumerable<TEntity> objs)
        {
            return objs.Select(this.GetParameters).ToList();
        }

        private IList<object> GetParameters(TEntity obj)
        {
            return this.GetColumns(this.Mapping).Select(column => column.GetValue(obj)).ToList();
        }

        private IEnumerable<DbColumnMapping<IDbProvider>> GetColumns(DbTableMapping<TDbProvider> mapping)
        {
            return mapping.GetColumns()
                .Where(column =>
                    this.InsertMode != InsertMode.Insert ||
                    column.PrimaryKeyAttribute == null ||
                    !column.PrimaryKeyAttribute.IsAutoIncrement)
                .ToArray();
        }
    }
}