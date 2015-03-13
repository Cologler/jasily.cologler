
namespace System.Data.Torrent
{
    public enum BencodingObjectType : byte
    {
        /// <summary>
        /// 'd'
        /// </summary>
        Dictionary = 100,

        /// <summary>
        /// 'i'
        /// </summary>
        Digit = 105,

        /// <summary>
        /// 'l'
        /// </summary>
        List = 108,

        String
    }
}
