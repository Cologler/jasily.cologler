using System;
using System.Collections.Generic;
using System.Linq;
using Jasily.Data.SQLBuilder.Enums;

namespace Jasily.Data.SQLBuilder
{
    public class InsertBuilder : SQLBuilder
    {
        public InsertBuilder()
        {
            this.InsertMode = InsertMode.Insert;
        }

        public InsertMode InsertMode { get; set; }

        public string Build<T>()
            where T : new()
        {
            var map = DbMappingManager.GetMapping<T>();

            var sql = new List<string>();

            sql.Add(this.InsertMode.GetString());

            sql.Add("INTO");

            this.TryGetDatabaseName(sql);

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

        public IList<IList<object>> GetParameters<T>(IEnumerable<T> objs)
            where T : new()
        {
            var map = DbMappingManager.GetMapping<T>();
            return objs.Select(o => this.GetParameters(map, o)).ToList();
        }

        private IList<object> GetParameters<T>(DbTableMapping mapping, T obj)
            where T : new()
        {
            return this.GetColumns(mapping).Select(column => column.GetValue(obj)).ToList();
        }

        private IEnumerable<DbColumnMapping> GetColumns(DbTableMapping mapping)
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