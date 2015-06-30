using System;
using System.Collections.Generic;

namespace Jasily.Data.SQLBuilder
{
    public abstract class SQLBuilder
    {
        /// <summary>
        /// can keep empty
        /// </summary>
        public string DatabaseName { get; set; }

        protected void TryGetDatabaseName(List<string> sql)
        {
            if (!String.IsNullOrWhiteSpace(this.DatabaseName))
            {
                sql.Add(this.DatabaseName);
            }
        }
    }
}