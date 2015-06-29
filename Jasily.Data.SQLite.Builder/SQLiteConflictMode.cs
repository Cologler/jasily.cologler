namespace Jasily.Data.SQLite.Builder
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