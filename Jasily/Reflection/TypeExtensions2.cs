using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.Reflection
{
    public static class TypeExtensions2
    {
        public static IEnumerable<TBase> GetImplementedObjects<TBase>([NotNull] this IEnumerable<Type> types, Predicate<Type> predicate = null)
            where TBase : class
        {
            if (types == null) throw new ArgumentNullException(nameof(types));
            var baseType = typeof(TBase).GetTypeInfo();
            return from type in types.Select(z => new { Type = z, TypeInfo = z.GetTypeInfo() })
                   where !type.TypeInfo.IsAbstract
                   where predicate?.Invoke(type.Type) ?? true
                   where baseType.IsAssignableFrom(type.TypeInfo)
                   select (TBase)JasilyActivator.CreateInstance(type.Type);
        }
    }
}