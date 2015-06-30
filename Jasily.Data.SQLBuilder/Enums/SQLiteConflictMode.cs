namespace Jasily.Data.SQLBuilder.Enums
{
    public enum SQLiteConflictMode
    {
        None,

        Rollback,

        Abort,

        Fail,

        Ignore,

        Replace
    }
}