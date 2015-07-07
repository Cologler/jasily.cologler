namespace System.IO
{
    public static class JasilyStringReader
    {
        /// <summary>
        /// read next char if not end.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static char? ReadChar(this StringReader reader)
        {
            var n = reader.Read();

            if (n == -1)
                return null;
            else
                return (char)n;
        }
    }
}
