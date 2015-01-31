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
            return type.HasFlag(FileSystemTestType.Multiple) && items.All(z => type.IsMatch(z));
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
