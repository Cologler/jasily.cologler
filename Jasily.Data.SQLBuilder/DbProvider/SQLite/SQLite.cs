using System;

namespace Jasily.Data.SQLBuilder.DbProvider.SQLite
{
    public class SQLite
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
                    var int64 = (long)value;
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
    }
}
