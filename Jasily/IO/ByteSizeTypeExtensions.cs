namespace Jasily.IO
{
    public static class ByteSizeTypeExtensions
    {
        public static ByteSize GetByteSize(this long length)
        {

            double l = length;
            int level = 0;
            while (l > 1024)
            {
                level++;
                l /= 1024;
            }
            return new ByteSize((ByteSizeType)level, l, length);
        }
    }
}