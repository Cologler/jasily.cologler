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

        public TorrentInfo(string filepath)
        {
            using (var stream = File.OpenRead(filepath))
                Init(stream);
        }

        public TorrentInfo(Stream torrentStream)
        {
            Init(torrentStream);
        }

        private void Init(Stream torrentStream)
        {
            try
            {
                InnerDictionary = (IBencodingDictionary)Bencoding.Parse(torrentStream);
            }
            catch (Exception) { throw new ArgumentException("非法的 torrent 文件"); }

            var bytes = InnerDictionary.Value["info"].OriginBytes();
            InfoHash = SHA1.Create().ComputeHashString(bytes);
        }

        public string InfoHash { get; private set; }
    }
}
