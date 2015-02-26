using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Torrent
{
    public sealed class TorrentInfo
    {
        IBencodingDictionary InnerDictionary;

        string _infoHash;

        private TorrentInfo()
        {
        }

        private TorrentInfo Load(Stream torrentStream)
        {
            try
            {
                InnerDictionary = (IBencodingDictionary)Bencoding.Parse(torrentStream);
            }
            catch (Exception) { throw new ArgumentException("error torrent file"); }

            return this;
        }

        public string InfoHash
        {
            get
            { 
                if (_infoHash == null)
                    _infoHash = SHA1.Create().ComputeHashString(InnerDictionary.Value["info"].OriginBytes());

                return _infoHash;
            }
        }

        public string CreateMagnetLink()
        {
            return "magnet:?xt=urn:btih:" + InfoHash;
        }

        public static TorrentInfo From(string filepath)
        {
            using (var stream = File.OpenRead(filepath))
                return From(stream);
        }

        public static TorrentInfo From(Stream torrentStream)
        {
            var info = new TorrentInfo();
            return info.Load(torrentStream);
        }
    }
}
