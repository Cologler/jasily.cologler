using System;
using Jasily.Data.SQLBuilder.Enums;

namespace Jasily.Data.SQLBuilder
{
    public static class GetSQLiteStringHelper
    {
        public static string GetString(this OrderMode order)
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

        public static string GetString(this InsertMode insert)
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