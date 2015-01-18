using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Torrent
{
    public sealed class Bencoding
    {
        public Bencoding(Stream torrentStream)
        {
            using (var reader = new BinaryReader(torrentStream))
            {
                Content = new BencodingObject(reader);
            }
        }

        public BencodingObject Content { get; private set; }
    }
}
