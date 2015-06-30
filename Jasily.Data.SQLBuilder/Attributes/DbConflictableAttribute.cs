using System;
using Jasily.Data.SQLBuilder.Enums;

namespace Jasily.Data.SQLBuilder.Attributes
{
    public class DbConflictableAttribute : Attribute
    {
        public SQLiteConflictMode Conflict { get; set; }
    }
}