using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
