namespace System.Security.Cryptography
{
    public static class JasilyICryptoTransform
    {
        /// <summary>
        /// transform whole inputBuffer.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="inputBuffer"></param>
        /// <returns></returns>
        public static byte[] TransformFinalBlock(this ICryptoTransform transform, byte[] inputBuffer)
        {
            return transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
        }
    }
}