using System;
using System.Collections.Generic;
using Jasily.Data.SQLBuilder.Enums;

namespace Jasily.Data.SQLBuilder.DbProvider
{
    public interface IDbProvider
    {
        string GetDbTypeName<T>();
        string GetDbTypeName(Type type);

        object ConvertValue(object value, Type type);

        IEnumerable<string> BuildColumnDefintions(DbColumnMapping<IDbProvider> column);

        string GetString(OrderMode order);
        string GetString(InsertMode insert);
    }
}