namespace System
{
    public static class JasilyChar
    {
        /// <summary>
        /// repeat this like ( string * int ) in python
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="count"></param>
        /// <exception cref="System.ArgumentOutOfRangeException">count &lt; 0</exception>
        /// <returns></returns>
        public static string Repeat(this char ch, int count)
        {
            return new string(ch, count);
        } 
    }
}