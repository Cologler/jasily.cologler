using System;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.Data.SQLBuilder
{
    public class CreateTableBuilder : SQLBuilder
    {
        public bool IsTempTable { get; set; }

        public bool IsNotExists { get; set; }

        public bool IsWithoutRowid { get; set; }

        public string Build<T>()
            where T : new()
        {
            var map = DbMappingManager.GetMapping<T>();
            
            var sql = new List<string>();

            sql.Add("CREATE");

            if (this.IsTempTable)
                sql.Add("TEMP");

            sql.Add("TABLE");

            if (this.IsNotExists)
                sql.Add("IF NOT EXISTS");

            this.TryGetDatabaseName(sql);

            sql.Add(map.TableName);

            sql.Add("(");

            sql.Add(String.Join(", ", map.GetColumns().Select(column => String.Join(" ", this.BuildColumnDefintions(column)))));

            sql.Add(")");

            if (this.IsWithoutRowid)
                sql.Add("WITHOUT ROWID");

            sql.Add(";");

            return String.Join(" ", sql);
        }

        private IEnumerable<string> BuildColumnDefintions(DbColumnMapping column)
        {
            yield return column.ColumnName;
            yield return column.ColumnTypeName;

            if (column.PrimaryKeyAttribute != null)
            {
                yield return "PRIMARY KEY";

                if (column.PrimaryKeyAttribute.Order.HasValue)
                    yield return column.PrimaryKeyAttribute.Order.Value.GetString();

                if (column.PrimaryKeyAttribute.IsAutoIncrement)
                    yield return "AUTOINCREMENT";
            }
        }
    }
}