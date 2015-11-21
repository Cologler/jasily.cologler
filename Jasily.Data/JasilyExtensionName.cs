using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable InconsistentNaming

namespace Jasily.Data
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

        public static bool IsVideo(string extensionName) => IsType<Video>(extensionName);

        public static bool IsMusic(string extensionName) => IsType<Music>(extensionName);

        public static bool IsPicture(string extensionName) => IsType<Picture>(extensionName);

        private static bool IsType<T>(string extensionName) => ExtensionNameMapper<T>.Instance[extensionName];

        private class ExtensionNameMapper<T>
        {
            public static ExtensionNameMapper<T> Instance { get; }

            static ExtensionNameMapper()
            {
                Instance = new ExtensionNameMapper<T>();
            }

            public IReadOnlyList<string> ExtensionNames { get; }

            private ExtensionNameMapper()
            {
                this.ExtensionNames = Enum.GetValues(typeof(T)).OfType<T>().Select(z => z.ToString().ToLower()).ToList();
            }

            /// <summary>
            /// return is extensionName was in this.ExtensionNames
            /// </summary>
            /// <param name="extensionName"></param>
            /// <returns></returns>
            public bool this[string extensionName]
            {
                get
                {
                    if (extensionName == null)
                        throw new ArgumentNullException();

                    if (string.IsNullOrEmpty(extensionName))
                        return false;

                    extensionName = extensionName.TrimStart('.').ToLower();
                    return this.ExtensionNames.Contains(extensionName);
                }
            }
        }
    }
}
