using System;
using System.Collections.Generic;
using System.Linq;
using Jasily.Data.SQLBuilder.DbProvider;

namespace Jasily.Data.SQLBuilder
{
    public class CreateTableBuilder<TDbProvider> : SQLBuilder
        where TDbProvider : IDbProvider, new ()
    {
        public CreateTableBuilder(IDbProvider dbProvider)
            : base(dbProvider)
        {
            
        }

        public bool IsTempTable { get; set; }

        public bool IsNotExists { get; set; }

        public bool IsWithoutRowid { get; set; }

        public string Build<T>()
            where T : new ()
        {
            var map = DbMappingManager<TDbProvider>.GetMapping<T>();
            
            var sql = new List<string>();

            sql.Add("CREATE");

            if (this.IsTempTable)
                sql.Add("TEMP");

            sql.Add("TABLE");

            if (this.IsNotExists)
                sql.Add("IF NOT EXISTS");

            sql.AddRange(this.DatabaseNamePart());

            sql.Add(map.TableName);

            sql.Add("(");

            sql.Add(String.Join(", ", map.GetColumns().Select(column => String.Join(" ", this.DbProvider.BuildColumnDefintions(column)))));

            sql.Add(")");

            if (this.IsWithoutRowid)
                sql.Add("WITHOUT ROWID");

            sql.Add(";");

            return String.Join(" ", sql);
        }
    }
}