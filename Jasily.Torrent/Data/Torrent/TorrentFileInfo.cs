using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Torrent
{
    public struct TorrentFileInfo
    {
        string filePath;
        string fileName;
        long fileSize;

        public TorrentFileInfo(string[] filePaths, long fileSize)
        {
            this.filePath = String.Join("\\", filePaths);
            this.fileName = filePaths.Last();
            this.fileSize = fileSize;
        }

        public string FilePath { get { return filePath; } }

        public string FileName { get { return fileName; } }

        public long Length { get { return fileSize; } }
    }
}
