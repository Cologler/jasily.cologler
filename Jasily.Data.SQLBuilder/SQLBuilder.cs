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

        protected IEnumerable<string> DatabaseNamePart()
        {
            if (!String.IsNullOrWhiteSpace(this.DatabaseName))
            {
                yield return this.DatabaseName;
            }
        }
    }

    public abstract class SQLBuilder<T> : SQLBuilder
        where T : new()
    {
        protected SQLBuilder()
        {
            this.Mapping = DbMappingManager.GetMapping<T>();
        }

        protected DbTableMapping Mapping { get; private set; }

        protected IEnumerable<string> TableNamePart()
        {
            yield return this.Mapping.TableName;
        }
    }
}