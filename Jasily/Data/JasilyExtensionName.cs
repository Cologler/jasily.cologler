using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace System.Data
{
    public static class JasilyExtensionName
    {
        public enum Video
        {
            MP4,
            MKV,
            RM, RMVB,
            AVI,
            TS,
            WMV,
            MOV
        }

        public enum Music
        {
            MP3,
            FLAC, APE,
            WMA
        }

        public enum Picture
        {
            JPG, JPEG, PNG, GIF
        }

        public static bool IsVideo(string extensionName)
        {
            if (extensionName == null)
                throw new ArgumentNullException();

            if (extensionName.IsNullOrWhiteSpace())
                return false;

            return IsType<Video>(extensionName);
        }

        public static bool IsMusic(string extensionName)
        {
            if (extensionName == null)
                throw new ArgumentNullException();

            if (extensionName.IsNullOrWhiteSpace())
                return false;

            return IsType<Music>(extensionName);
        }

        public static bool IsPicture(string extensionName)
        {
            if (extensionName == null)
                throw new ArgumentNullException();

            if (extensionName.IsNullOrWhiteSpace())
                return false;

            return IsType<Picture>(extensionName);
        }

        private static bool IsType<T>(string extensionName)
        {
            extensionName = extensionName.TrimStart('.').ToLower();
            return Enum.GetValues(typeof(T)).OfType<T>()
                .Select(z => z.ToString().ToLower())
                .Contains(extensionName);
        }
    }
}
