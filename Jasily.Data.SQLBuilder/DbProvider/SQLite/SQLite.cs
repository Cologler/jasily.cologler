using System;
using System.Collections.Generic;
using Jasily.Data.SQLBuilder.Enums;

namespace Jasily.Data.SQLBuilder.DbProvider.SQLite
{
    public class SQLite : IDbProvider
    {
        public string GetDbTypeName<T>()
        {
            return this.GetDbTypeName(typeof(T));
        }

        public string GetDbTypeName(Type type)
        {
            switch (type.FullName)
            {
                case "System.Boolean":
                case "System.Int64":
                case "System.Int32":
                    return "INTEGER";

                case "System.String":
                    return "TEXT";
            }

            throw new NotSupportedException(String.Format("type {0} can not convert to SQLite type.", type.Name));
        }

        public object ConvertValue(object value, Type type)
        {
            if (value == null)
                return null;

            var valueType = value.GetType();

            if (valueType == type)
                return value;

            switch (valueType.FullName)
            {
                case "System.DBNull":
                    return null;

                case "System.Int64":
                    var int64 = (long) value;
                    switch (type.FullName)
                    {
                        case "System.Boolean":
                            switch (int64)
                            {
                                case 0:
                                    return false;
                                case 1:
                                    return true;
                                default:
                                    throw new ArgumentOutOfRangeException("value", "boolean value only can be 0 or 1.");
                            }

                        case "System.Int32":
                            if (int64 <= Int32.MaxValue && int64 >= Int32.MinValue)
                                return Convert.ToInt32(int64);
                            else
                                throw new ArgumentOutOfRangeException("value");
                    }
                    break;

            }

            throw new NotSupportedException();
        }

        public IEnumerable<string> BuildColumnDefintions(DbColumnMapping<IDbProvider> column)
        {
            yield return column.ColumnName;
            yield return column.ColumnTypeName;

            if (column.PrimaryKeyAttribute != null)
            {
                yield return "PRIMARY KEY";

                if (column.PrimaryKeyAttribute.Order.HasValue)
                    yield return this.GetString(column.PrimaryKeyAttribute.Order.Value);

                if (column.PrimaryKeyAttribute.IsAutoIncrement)
                    yield return "AUTOINCREMENT";
            }
        }

        public string GetString(OrderMode order)
        {
            switch (order)
            {
                case OrderMode.Asc:
                    return "ASC";
                case OrderMode.Desc:
                    return "DESC";
                default:
                    throw new ArgumentOutOfRangeException("order", order, null);
            }
        }

        public string GetString(InsertMode insert)
        {
            switch (insert)
            {
                case InsertMode.Insert:
                    return "INSERT";
                case InsertMode.Replace:
                    return "REPLACE";
                case InsertMode.InsertOrReplace:
                    return "INSERT OR REPLACE";
                case InsertMode.InsertOrRollback:
                    return "INSERT OR ROLLBACK";
                case InsertMode.InsetOrAbort:
                    return "INSERT OR ABORT";
                case InsertMode.InsertOrFail:
                    return "INSERT OR FAIL";
                case InsertMode.InsertOrIgnore:
                    return "INSERT OR IGNORE";
                default:
                    throw new ArgumentOutOfRangeException("insert", insert, null);
            }
        }
    }
}
