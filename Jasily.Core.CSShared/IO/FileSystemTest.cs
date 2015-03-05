using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
    public static class FileSystemTest
    {
        private readonly static Func<string, bool> DefaultFileExists;
        private readonly static Func<string, bool> DefaultDirectoryExists;

        public static Func<string, bool> FileExistsFunc;
        public static Func<string, bool> DirectoryExistsFunc;

        static FileSystemTest()
        {
            DefaultFileExists = (path) => File.Exists(path);
            DefaultDirectoryExists = (path) => Directory.Exists(path);

            FileExistsFunc = DefaultFileExists;
            DirectoryExistsFunc = DefaultDirectoryExists;
        }

        private static bool IsFileExists(string item)
        {
            return (FileExistsFunc ?? DefaultFileExists)(item);
        }

        private static bool IsDirectoryExists(string item)
        {
            return (DirectoryExistsFunc ?? DefaultDirectoryExists)(item);
        }

        public static bool IsMatch(this FileSystemTestType type, string item)
        {
            return (type.HasFlag(FileSystemTestType.File) && IsFileExists(item)) ||
                   (type.HasFlag(FileSystemTestType.Directory) && IsDirectoryExists(item)) ||
                   type == FileSystemTestType.Any;
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
                    return items.All(z => IsFileExists(z));

                case FileSystemTestType.MultipleDirectory:
                    return items.All(z => IsDirectoryExists(z));

                case FileSystemTestType.Multiple:
                    return items.All(z => IsFileExists(z) || IsDirectoryExists(z));

                case FileSystemTestType.Any:
                    return true;
            }

            if (type.HasFlag(FileSystemTestType.MultipleDirectory) && items.All(z => IsDirectoryExists(z)))
                return true;

            if (type.HasFlag(FileSystemTestType.MultipleFile) && items.All(z => IsFileExists(z)))
                return true;

            return false;
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
