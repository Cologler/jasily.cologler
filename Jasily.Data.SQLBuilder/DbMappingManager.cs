﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Jasily.Data.SQLBuilder.Attributes;

namespace Jasily.Data.SQLBuilder
{
    public static class DbMappingManager
    {
        private static readonly Dictionary<Type, Lazy<DbTableMapping>> mapped = new Dictionary<Type, Lazy<DbTableMapping>>();

        public static DbTableMapping GetMapping<T>()
            where T : new()
        {
            var type = typeof(T);

            var m = mapped.GetValueOrDefault(type);

            if (m != null)
                return m.Value;

            lock (mapped)
            {
                m = mapped.GetValueOrSetDefault(type,
                    new Func<Lazy<DbTableMapping>>(
                        () => new Lazy<DbTableMapping>(
                            new Func<DbTableMapping>(BuildMapping<T>),
                            LazyThreadSafetyMode.ExecutionAndPublication)));
            }

            return m.Value;
        }

        private static DbTableMapping BuildMapping<T>()
            where T : new()
        {
            var attr = typeof(T).GetTypeInfo().GetCustomAttribute<DbTableAttribute>();
            if (attr == null)
                throw new ArgumentException(String.Format("type {0} must contain SQLiteTableAttribute", typeof(T).Name));
            var mapping = new DbTableMapping<T>(attr);
            mapping.MapColumns();
            return mapping;
        }
    }
}