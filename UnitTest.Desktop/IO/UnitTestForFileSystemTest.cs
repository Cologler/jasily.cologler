using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTest.Desktop.IO
{
    [TestClass]
    public class UnitTestForFileSystemTest
    {
        [TestMethod]
        public void TestForIsMatch()
        {
            var notExists = @"D:\a";
            var file1 = @"d:\laiwei7@163.com.cer";
            var file2 = @"D:\wp.iflyzunhong.cn.cer";
            var folder1 = @"D:\AppData";
            var folder2 = @"D:\BaiduYunDownload";

            Assert.IsTrue(File.Exists(file1), "{0} was not a file, please use another path.", file1);
            Assert.IsTrue(File.Exists(file2), "{0} was not a file, please use another path.", file2);
            Assert.IsTrue(Directory.Exists(folder1), "{0} was not a folder, please use another path.", folder1);
            Assert.IsTrue(Directory.Exists(folder2), "{0} was not a folder, please use another path.", folder2);

            var files = new string[] { file1, file2 };
            var folders = new string[] { folder1, folder2 };
            var filesAndFolders1 = new string[] { file1, folder1 };
            var filesAndFolders2 = new string[] { file1, file2, folder1, folder2 };

            Assert.IsTrue(FileSystemTestType.Any.IsMatch(notExists));
            Assert.IsTrue(FileSystemTestType.Any.IsMatch(file1));
            Assert.IsTrue(FileSystemTestType.Any.IsMatch(file2));
            Assert.IsTrue(FileSystemTestType.Any.IsMatch(folder1));
            Assert.IsTrue(FileSystemTestType.Any.IsMatch(folder2));
            Assert.IsTrue(FileSystemTestType.Any.IsMatch(files));
            Assert.IsTrue(FileSystemTestType.Any.IsMatch(folders));
            Assert.IsTrue(FileSystemTestType.Any.IsMatch(filesAndFolders1));
            Assert.IsTrue(FileSystemTestType.Any.IsMatch(filesAndFolders2));

            Assert.IsTrue(!FileSystemTestType.Directory.IsMatch(notExists));
            Assert.IsTrue(!FileSystemTestType.Directory.IsMatch(file1));
            Assert.IsTrue(!FileSystemTestType.Directory.IsMatch(file2));
            Assert.IsTrue(FileSystemTestType.Directory.IsMatch(folder1));
            Assert.IsTrue(FileSystemTestType.Directory.IsMatch(folder2));
            Assert.IsTrue(!FileSystemTestType.Directory.IsMatch(files));
            Assert.IsTrue(!FileSystemTestType.Directory.IsMatch(folders));
            Assert.IsTrue(!FileSystemTestType.Directory.IsMatch(filesAndFolders1));
            Assert.IsTrue(!FileSystemTestType.Directory.IsMatch(filesAndFolders2));

            Assert.IsTrue(!FileSystemTestType.File.IsMatch(notExists));
            Assert.IsTrue(FileSystemTestType.File.IsMatch(file1));
            Assert.IsTrue(FileSystemTestType.File.IsMatch(file2));
            Assert.IsTrue(!FileSystemTestType.File.IsMatch(folder1));
            Assert.IsTrue(!FileSystemTestType.File.IsMatch(folder2));
            Assert.IsTrue(!FileSystemTestType.File.IsMatch(files));
            Assert.IsTrue(!FileSystemTestType.File.IsMatch(folders));
            Assert.IsTrue(!FileSystemTestType.File.IsMatch(filesAndFolders1));
            Assert.IsTrue(!FileSystemTestType.File.IsMatch(filesAndFolders2));

            Assert.IsTrue(!FileSystemTestType.Multiple.IsMatch(notExists));
            Assert.IsTrue(FileSystemTestType.Multiple.IsMatch(file1));
            Assert.IsTrue(FileSystemTestType.Multiple.IsMatch(file2));
            Assert.IsTrue(FileSystemTestType.Multiple.IsMatch(folder1));
            Assert.IsTrue(FileSystemTestType.Multiple.IsMatch(folder2));
            Assert.IsTrue(FileSystemTestType.Multiple.IsMatch(files));
            Assert.IsTrue(FileSystemTestType.Multiple.IsMatch(folders));
            Assert.IsTrue(FileSystemTestType.Multiple.IsMatch(filesAndFolders1));
            Assert.IsTrue(FileSystemTestType.Multiple.IsMatch(filesAndFolders2));

            Assert.IsTrue(!FileSystemTestType.MultipleDirectory.IsMatch(notExists));
            Assert.IsTrue(!FileSystemTestType.MultipleDirectory.IsMatch(file1));
            Assert.IsTrue(!FileSystemTestType.MultipleDirectory.IsMatch(file2));
            Assert.IsTrue(FileSystemTestType.MultipleDirectory.IsMatch(folder1));
            Assert.IsTrue(FileSystemTestType.MultipleDirectory.IsMatch(folder2));
            Assert.IsTrue(!FileSystemTestType.MultipleDirectory.IsMatch(files));
            Assert.IsTrue(FileSystemTestType.MultipleDirectory.IsMatch(folders));
            Assert.IsTrue(!FileSystemTestType.MultipleDirectory.IsMatch(filesAndFolders1));
            Assert.IsTrue(!FileSystemTestType.MultipleDirectory.IsMatch(filesAndFolders2));

            Assert.IsTrue(!FileSystemTestType.MultipleFile.IsMatch(notExists));
            Assert.IsTrue(FileSystemTestType.MultipleFile.IsMatch(file1));
            Assert.IsTrue(FileSystemTestType.MultipleFile.IsMatch(file2));
            Assert.IsTrue(!FileSystemTestType.MultipleFile.IsMatch(folder1));
            Assert.IsTrue(!FileSystemTestType.MultipleFile.IsMatch(folder2));
            Assert.IsTrue(FileSystemTestType.MultipleFile.IsMatch(files));
            Assert.IsTrue(!FileSystemTestType.MultipleFile.IsMatch(folders));
            Assert.IsTrue(!FileSystemTestType.MultipleFile.IsMatch(filesAndFolders1));
            Assert.IsTrue(!FileSystemTestType.MultipleFile.IsMatch(filesAndFolders2));

            Assert.IsTrue(!FileSystemTestType.None.IsMatch(notExists));
            Assert.IsTrue(!FileSystemTestType.None.IsMatch(file1));
            Assert.IsTrue(!FileSystemTestType.None.IsMatch(file2));
            Assert.IsTrue(!FileSystemTestType.None.IsMatch(folder1));
            Assert.IsTrue(!FileSystemTestType.None.IsMatch(folder2));
            Assert.IsTrue(!FileSystemTestType.None.IsMatch(files));
            Assert.IsTrue(!FileSystemTestType.None.IsMatch(folders));
            Assert.IsTrue(!FileSystemTestType.None.IsMatch(filesAndFolders1));
            Assert.IsTrue(!FileSystemTestType.None.IsMatch(filesAndFolders2));

            Assert.IsTrue(!FileSystemTestType.Single.IsMatch(notExists));
            Assert.IsTrue(FileSystemTestType.Single.IsMatch(file1));
            Assert.IsTrue(FileSystemTestType.Single.IsMatch(file2));
            Assert.IsTrue(FileSystemTestType.Single.IsMatch(folder1));
            Assert.IsTrue(FileSystemTestType.Single.IsMatch(folder2));
            Assert.IsTrue(!FileSystemTestType.Single.IsMatch(files));
            Assert.IsTrue(!FileSystemTestType.Single.IsMatch(folders));
            Assert.IsTrue(!FileSystemTestType.Single.IsMatch(filesAndFolders1));
            Assert.IsTrue(!FileSystemTestType.Single.IsMatch(filesAndFolders2));

            Assert.IsTrue(!(FileSystemTestType.File | FileSystemTestType.Directory | FileSystemTestType.MultipleDirectory).IsMatch(notExists));
            Assert.IsTrue((FileSystemTestType.File | FileSystemTestType.Directory | FileSystemTestType.MultipleDirectory).IsMatch(file1));
            Assert.IsTrue((FileSystemTestType.File | FileSystemTestType.Directory | FileSystemTestType.MultipleDirectory).IsMatch(file2));
            Assert.IsTrue((FileSystemTestType.File | FileSystemTestType.Directory | FileSystemTestType.MultipleDirectory).IsMatch(folder1));
            Assert.IsTrue((FileSystemTestType.File | FileSystemTestType.Directory | FileSystemTestType.MultipleDirectory).IsMatch(folder2));
            Assert.IsTrue(!(FileSystemTestType.File | FileSystemTestType.Directory | FileSystemTestType.MultipleDirectory).IsMatch(files));
            Assert.IsTrue((FileSystemTestType.File | FileSystemTestType.Directory | FileSystemTestType.MultipleDirectory).IsMatch(folders));
            Assert.IsTrue(!(FileSystemTestType.File | FileSystemTestType.Directory | FileSystemTestType.MultipleDirectory).IsMatch(filesAndFolders1));
            Assert.IsTrue(!(FileSystemTestType.File | FileSystemTestType.Directory | FileSystemTestType.MultipleDirectory).IsMatch(filesAndFolders2));
        }
    }
}
