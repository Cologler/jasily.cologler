using System;
using System.Collections.Generic;
using System.Linq;
using Jasily.Data.SQLBuilder.DbProvider;
using Jasily.Data.SQLBuilder.DbProvider.SQLite;

namespace Jasily.Data.SQLBuilder
{
    public abstract class SQLBuilder
    {
        public IDbProvider DbProvider { get; private set; }

        public SQLBuilder(IDbProvider dbProvider)
        {
            this.DbProvider = dbProvider;
        }

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

        public static CreateTableBuilder<TDbProvider> CreateTable<TDbProvider>()
            where TDbProvider : IDbProvider, new()
        {
            return new CreateTableBuilder<TDbProvider>(new TDbProvider());
        }

        public static InsertBuilder<TDbProvider, TEntity> Insert<TDbProvider, TEntity>()
            where TDbProvider : IDbProvider, new()
            where TEntity : new()
        {
            return new InsertBuilder<TDbProvider, TEntity>(new TDbProvider());
        }
    }

    public abstract class SQLBuilder<TDbProvider, TEntity> : SQLBuilder
        where TDbProvider : IDbProvider, new ()
        where TEntity : new ()
    {

        protected SQLBuilder(IDbProvider dbProvider)
            : base(dbProvider)
        {
            this.Mapping = DbMappingManager<TDbProvider>.GetMapping<TEntity>();
        }

        protected DbTableMapping<TDbProvider> Mapping { get; private set; }

        protected IEnumerable<string> TableNamePart()
        {
            yield return this.Mapping.TableName;
        }
    }
}