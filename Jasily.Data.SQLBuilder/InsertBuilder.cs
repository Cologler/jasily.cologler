using System;
using System.Collections.Generic;
using System.Linq;
using Jasily.Data.SQLBuilder.Enums;

namespace Jasily.Data.SQLBuilder
{
    public class InsertBuilder<T> : SQLBuilder<T>
        where T : new()
    {
        public InsertBuilder()
        {
            this.InsertMode = InsertMode.Insert;
        }

        public InsertMode InsertMode { get; set; }

        public string Build()
        {
            var map = this.Mapping;

            var sql = new List<string>();

            sql.Add(this.InsertMode.GetString());

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

        public IList<IList<object>> GetParameters(IEnumerable<T> objs)
        {
            return objs.Select(this.GetParameters).ToList();
        }

        private IList<object> GetParameters(T obj)
        {
            return this.GetColumns(this.Mapping).Select(column => column.GetValue(obj)).ToList();
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