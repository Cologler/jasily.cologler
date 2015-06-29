using System;
using System.Collections.Generic;

namespace Jasily.Data.SQLite.Builder
{
    public class CreateTableBuilder
    {
        public bool IsTempTable { get; set; }

        public bool IsNotExists { get; set; }

        /// <summary>
        /// can keep empty
        /// </summary>
        public string DatabaseName { get; set; }

        public bool IsWithoutRowid { get; set; }

        public string Build<T>()
            where T : new ()
        {
            var map = SQLiteMappingManager.GetMapping<T>();

            var sqlpart = new List<string>();

            sqlpart.Add("CREATE");

            if (this.IsTempTable)
                sqlpart.Add("TEMP");

            sqlpart.Add("TABLE");

            if (this.IsNotExists)
                sqlpart.Add("IF NOT EXISTS");

            if (!String.IsNullOrWhiteSpace(this.DatabaseName))
                sqlpart.Add(this.DatabaseName + ".");

            sqlpart.Add(map.TableName);

            sqlpart.Add("(");

            foreach (var column in map.GetColumns())
            {
                sqlpart.Add(column.ColumnName);
                sqlpart.Add(column.ColumnTypeName);

                if (column.PrimaryKeyAttribute != null)
                {
                    sqlpart.Add("PRIMARY KEY");

                    if (column.PrimaryKeyAttribute.Order.HasValue)
                        sqlpart.Add(column.PrimaryKeyAttribute.Order.Value.GetString());

                    if (column.PrimaryKeyAttribute.IsAutoIncrement)
                        sqlpart.Add("AUTOINCREMENT");
                }
            }

            sqlpart.Add(")");

            if (this.IsWithoutRowid)
                sqlpart.Add("WITHOUT ROWID  ");

            sqlpart.Add(";");

            return String.Join(" ", sqlpart);
        }
    }
}