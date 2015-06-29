using System;

namespace Jasily.Data.SQLite.Builder
{
    public enum OrderMode
    {
        Asc,

        Desc
    }

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
    }
}