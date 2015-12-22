using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

// ReSharper disable InconsistentNaming

namespace Jasily.Data
{
    public class JasilyExtensionName
    {
        public static JasilyExtensionName Instance { get; }

        static JasilyExtensionName()
        {
            Instance = new JasilyExtensionName();
        }

        private JasilyExtensionName()
        {
        }

        private Dictionary<string, ExtensionName> mapedName;
        private Dictionary<int, Dictionary<string, ExtensionName>> mapedType;

        private void Map()
        {
            var builder = ExtensionName.Build().ToArray();
            foreach (var extensionName in builder)
            {
                this.mapedName.Add(extensionName.Name, extensionName);

                foreach (var t in extensionName.Types)
                {
                    Dictionary<string, ExtensionName> map;
                    if (!this.mapedType.TryGetValue((int)t, out map))
                    {
                        map = new Dictionary<string, ExtensionName>();
                        this.mapedType.Add((int)t, map);
                    }

                    try
                    {
                        map.Add(extensionName.Name, extensionName);
                    }
                    catch (ArgumentException)
                    {
                        throw new Exception($"{nameof(ExtensionNames)}.{extensionName.EnumName} contain same FileTypeAttribute: {nameof(ExtensionNames)}.{t}");
                    }
                }
            }
        }

        public bool IsFileType(FileType type, [NotNull] string extensionName)
        {
            if (extensionName == null) throw new ArgumentNullException(nameof(extensionName));
            Dictionary<string, ExtensionName> map;
            if (!this.mapedType.TryGetValue((int)type, out map)) return false;
            return map.ContainsKey(extensionName.TrimStart('.').ToLower());
        }
    }

    public sealed class ExtensionName
    {
        private readonly string originName;

        public string Name => this.originName ?? this.EnumName;

        public string EnumName { get; }

        private ExtensionName(string enumName, string originName)
        {
            this.EnumName = enumName.ToLower();
            this.originName = originName?.ToLower();
        }

        public IEnumerable<FileType> Types { get; private set; }

        public static IEnumerable<ExtensionName> Build()
        {
            var values = (ExtensionNames[])Enum.GetValues(typeof(ExtensionNames));
            if (values.Length == 0) yield break;
            foreach (var f in typeof(ExtensionNames).GetRuntimeFields())
            {
                if (f.Name == "value__") continue; // 枚举保留名称

                var types = f.GetCustomAttributes<FileTypeAttribute>().ToArray();
                if (types.Length == 0) throw new Exception($"missing FileTypeAttribute on {nameof(ExtensionNames)}.{f.Name}");

                var ext = new ExtensionName(f.Name, f.GetCustomAttribute<OriginNameAttribute>()?.OriginName)
                {
                    Types = types.Select(z => z.Type).ToArray()
                };

                yield return ext;
            }
        }
    }

    public enum ExtensionNames
    {
        [FileType(FileType.Document)]
        DOC,
        [FileType(FileType.Document)]
        DOCX,
        [FileType(FileType.Document)]
        XLS,
        [FileType(FileType.Document)]
        XLSX,
        [FileType(FileType.Document)]
        PPT,
        [FileType(FileType.Document)]
        PPTX,
        [FileType(FileType.Document)]
        PDF,
        [FileType(FileType.Document)]
        [FileType(FileType.SourceCode)]
        XML,
        [FileType(FileType.Document)]
        [FileType(FileType.SourceCode)]
        JSON,

        [FileType(FileType.Video)]
        MP4,
        [FileType(FileType.Video)]
        MKV,
        [FileType(FileType.Video)]
        RM,
        [FileType(FileType.Video)]
        RMVB,
        [FileType(FileType.Video)]
        AVI,
        [FileType(FileType.Video)]
        TS,
        [FileType(FileType.Video)]
        WMV,
        [FileType(FileType.Video)]
        MOV,

        [FileType(FileType.Music)]
        MP3,
        [FileType(FileType.Music)]
        [OpenSource("https://xiph.org/flac/")]
        FLAC,
        [FileType(FileType.Music)]
        APE,
        [FileType(FileType.Music)]
        WMA,

        [FileType(FileType.Picture)]
        JPG,
        [FileType(FileType.Picture)]
        JPEG,
        [FileType(FileType.Picture)]
        PNG,
        [FileType(FileType.Picture)]
        GIF,

        [FileType(FileType.CompressedPackage)]
        [OriginName("7z")]
        [OpenSource("http://www.7-zip.org/")]
        _7Z,
    }

    public enum FileType
    {
        Document,
        Video,
        Music,
        Picture,
        CompressedPackage,
        SourceCode,
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class FileTypeAttribute : Attribute
    {
        public FileType Type { get; }

        public FileTypeAttribute(FileType type)
        {
            this.Type = type;
        }
    }

    public sealed class OriginNameAttribute : Attribute
    {
        public string OriginName { get; }

        public OriginNameAttribute(string originName)
        {
            this.OriginName = originName;
        }
    }

    public abstract class FileTypeInfoAttribute : Attribute
    {
    }

    [Conditional("ATTR_OPEN_SOURCE")]
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class OpenSourceAttribute : FileTypeInfoAttribute
    {
        public string Url { get; }

        public OpenSourceAttribute(string url)
        {
            this.Url = url;
        }
    }
}
