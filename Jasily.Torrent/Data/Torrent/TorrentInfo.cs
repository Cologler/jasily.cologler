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

        bool isMFile;
        string _infoHash;
        List<TorrentFileInfo> _files;

        private TorrentInfo()
        {
            _files = new List<TorrentFileInfo>();
        }

        private TorrentInfo Load(Stream torrentStream)
        {
            try
            {
                InnerDictionary = (IBencodingDictionary)Bencoding.Parse(torrentStream);

                var info = InnerDictionary.Value["info"] as IBencodingDictionary;

                isMFile = true;

                if (info.ContainsKey("files"))
                {
                    var files = info.Value["files"].Value as IBencodingObject[];

                    _files.AddRange(
                        files.Cast<IBencodingDictionary>()
                            .Select(z => new TorrentFileInfo(
                                (z.Value["path"].Value as IBencodingObject[])
                                .Cast<IBencodingObject<string>>().Select(x => x.Value).ToArray(),
                                (long)z.Value["length"].Value)));
                }
                else
                {
                    _files.Add(new TorrentFileInfo(new string[] { (string)info.Value["name"].Value }, (long)info.Value["length"].Value));
                }
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

        public TorrentFileInfo[] Files
        {
            get { return _files.ToArray(); }
        }

        public long TotalSize { get { return Files.Sum(z => z.Length); } }

        public string CreateMagnetLink()
        {
            return "magnet:?xt=urn:btih:" + InfoHash;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <exception cref="System.ArgumentException">此文件为非法种子</exception>
        /// <returns></returns>
        public static TorrentInfo From(string filepath)
        {
            using (var stream = File.OpenRead(filepath))
                return From(stream);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="torrentStream"></param>
        /// <exception cref="System.ArgumentException">此文件为非法种子</exception>
        /// <returns></returns>
        public static TorrentInfo From(Stream torrentStream)
        {
            var info = new TorrentInfo();
            return info.Load(torrentStream);
        }
    }
}
