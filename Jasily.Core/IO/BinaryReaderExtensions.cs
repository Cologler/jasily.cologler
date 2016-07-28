using JetBrains.Annotations;

namespace System.IO
{
    public static class BinaryReaderExtensions
    {
        public static byte[] ReadBytesOrThrow([NotNull] this BinaryReader reader, int count)
        {
            var buffer = reader.ReadBytes(count);
            if (buffer.Length != count) throw new EndOfStreamException();
            return buffer;
        }
    }
}