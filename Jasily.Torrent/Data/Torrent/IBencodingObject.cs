using System.Collections.Generic;

namespace System.Data.Torrent
{
    public interface IBencodingObject
    {
        BencodingObjectType Type { get; }

        object Value { get; }

        List<byte> WriteOriginBytes(List<byte> bytes);
    }

    public interface IBencodingObject<T> : IBencodingObject
    {
        T Value { get; }
    }

    public interface IBencodingDictionary :
        IBencodingObject<Dictionary<string, IBencodingObject>>,
        IReadOnlyDictionary<string, IBencodingObject>
    {

    }
}
