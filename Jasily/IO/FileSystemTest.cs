using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
    public static class FileSystemTest
    {
        public static bool IsMatch(this FileSystemTestType type, string item)
        {
            return (type.HasFlag(FileSystemTestType.File) && File.Exists(item)) ||
                   (type.HasFlag(FileSystemTestType.Directory) && Directory.Exists(item));
        }

        public static bool IsMatch(this FileSystemTestType type, IEnumerable<string> items)
        {
            if (items.Count() <= 1)
                return items.All(z => type.IsMatch(z));

            switch (type)
            {
                case FileSystemTestType.None:
                case FileSystemTestType.File:
                case FileSystemTestType.Directory:
                case FileSystemTestType.Single:
                    return false;

                case FileSystemTestType.MultipleFile:
                    return items.All(z => File.Exists(z));

                case FileSystemTestType.MultipleDirectory:
                    return items.All(z => Directory.Exists(z));

                case FileSystemTestType.Multiple:
                    return items.All(z => File.Exists(z) || Directory.Exists(z));

                default:
                    return false;
            }
        }

        public static IEnumerable<string> Filter(this FileSystemTestType type, IEnumerable<string> items)
        {
            if (type.HasFlag(FileSystemTestType.Multiple))
                return items.Where(z => type.IsMatch(z));
            else
                return new string[] { };
        }
    }
}
