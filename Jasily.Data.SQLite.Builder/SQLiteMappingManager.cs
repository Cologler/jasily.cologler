using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Jasily.Data.SQLite.Builder.Attributes;

namespace Jasily.Data.SQLite.Builder
{
    internal class SQLiteMappingManager
    {
        private static readonly Dictionary<Type, Lazy<SQLiteTableMapping>> mapped = new Dictionary<Type, Lazy<SQLiteTableMapping>>();

        internal static SQLiteTableMapping GetMapping<T>()
            where T : new()
        {
            var type = typeof(T);

            var m = mapped.GetValueOrDefault(type);

            if (m != null)
                return m.Value;

            lock (mapped)
            {
                m = mapped.GetValueOrSetDefault(type,
                    new Func<Lazy<SQLiteTableMapping>>(
                        () => new Lazy<SQLiteTableMapping>(
                            new Func<SQLiteTableMapping>(BuildMapping<T>),
                            LazyThreadSafetyMode.ExecutionAndPublication)));
            }

            return m.Value;
        }

        private static SQLiteTableMapping BuildMapping<T>()
            where T : new()
        {
            var attr = typeof(T).GetTypeInfo().GetCustomAttribute<SQLiteTableAttribute>();
            if (attr == null)
                throw new ArgumentException(String.Format("type {0} must contain SQLiteTableAttribute", typeof(T).Name));
            var mapping = new SQLiteTableMapping<T>(attr);
            mapping.BuildColumnsMapping();
            return mapping;
        }
    }
}