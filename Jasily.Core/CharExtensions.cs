namespace System
{
    public static class CharExtensions
    {
        public static bool IsEnglishChar(this char ch) => ('a' <= ch && ch <= 'z') || ('A' <= ch && ch <= 'Z');
    }
}