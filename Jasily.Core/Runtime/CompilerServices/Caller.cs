namespace System.Runtime.CompilerServices
{
    public static class Caller
    {
        public static string GetFilePath([CallerFilePath] string path = "") => path;
        public static int GetLineNumber([CallerLineNumber] int line = 0) => line;
        public static string GetMemberName([CallerMemberName] string member = "") => member;

        public static string GetFilePath(this object obj, [CallerFilePath] string path = "") => path;
        public static int GetLineNumber(this object obj, [CallerLineNumber] int line = 0) => line;
        public static string GetMemberName(this object obj, [CallerMemberName] string member = "") => member;
    }
}