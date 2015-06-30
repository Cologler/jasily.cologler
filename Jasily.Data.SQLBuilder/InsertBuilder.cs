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

        public string Build<T>(T obj, out IList<object> parameters)
            where T : new()
        {
            parameters = new List<object>();

            var map = DbMappingManager.GetMapping<T>();

            var sql = new List<string>();

            sql.Add(this.InsertMode.GetString());

            sql.Add("INTO");

            this.TryGetDatabaseName(sql);

            sql.Add(map.TableName);

            sql.Add("(");

            var columnNames = new List<string>();
            foreach (var column in map.GetColumns()
                .Where(column => column.PrimaryKeyAttribute == null || !column.PrimaryKeyAttribute.IsAutoIncrement))
            {
                columnNames.Add(column.ColumnName);
                parameters.Add(column.GetValue(obj));
            }

            sql.Add(String.Join(", ", columnNames));
            sql.Add(")");

            sql.Add("VALUES");
            sql.Add("(");
            sql.Add(String.Join(", ", Enumerable.Repeat("?", columnNames.Count)));
            sql.Add(")");

            return String.Concat(String.Join(" ", sql), ";");
        }
    }
}