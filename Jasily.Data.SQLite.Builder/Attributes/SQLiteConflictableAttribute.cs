using System;

namespace Jasily.Data.SQLite.Builder.Attributes
{
    public class SQLiteConflictableAttribute : Attribute
    {
        public SQLiteConflictMode Conflict { get; set; }
    }
}