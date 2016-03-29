namespace System
{
    public static class CharExtensions
    {
        public static bool IsEnglishChar(this char ch)
        {
            return ('a' <= ch && ch <= 'z') || ('A' <= ch && ch <= 'Z');
        }
    }
}