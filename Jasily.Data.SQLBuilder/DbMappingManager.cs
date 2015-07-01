using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Jasily.Data.SQLBuilder.Attributes;
using Jasily.Data.SQLBuilder.DbProvider;

namespace Jasily.Data.SQLBuilder
{
    public static class DbMappingManager<TDbProvider>
        where TDbProvider : IDbProvider, new()
    {
        private static readonly Dictionary<Type, Lazy<DbTableMapping<TDbProvider>>> mapped =
            new Dictionary<Type, Lazy<DbTableMapping<TDbProvider>>>();

        public static DbTableMapping <TDbProvider> GetMapping<T>()
            where T : new()
        {
            var type = typeof(T);

            var m = mapped.GetValueOrDefault(type);

            if (m != null)
                return m.Value;

            lock (mapped)
            {
                m = mapped.GetValueOrSetDefault(type,
                    new Func<Lazy<DbTableMapping<TDbProvider>>>(
                        () => new Lazy<DbTableMapping<TDbProvider>>(
                            new Func<DbTableMapping<TDbProvider>>(BuildMapping<T>),
                            LazyThreadSafetyMode.ExecutionAndPublication)));
            }

            return m.Value;
        }

        private static DbTableMapping<TDbProvider> BuildMapping<T>()
            where T : new()
        {
            var attr = typeof(T).GetTypeInfo().GetCustomAttribute<DbTableAttribute>();
            if (attr == null)
                throw new ArgumentException(String.Format("type {0} must contain SQLiteTableAttribute", typeof(T).Name));
            var mapping = new DbTableMapping<TDbProvider, T>(attr);
            mapping.MapColumns();
            return mapping;
        }
    }
}