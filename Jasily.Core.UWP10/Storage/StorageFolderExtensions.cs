using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Windows.Storage
{
    public static class StorageFolderExtensions
    {
        [Conditional("DEBUG")]
        public static async void DebugDirectory(this IStorageFolder folder) => await DebugDirectory(folder, 0);

        private static async Task DebugDirectory(this IStorageFolder folder, int deep)
        {
            foreach (var item in await folder.GetItemsAsync())
            {
                var subFolder = item as IStorageFolder;
                if (subFolder != null)
                {
                    Debug.WriteLine($"{' '.Repeat(deep)}[FOLDER] {subFolder.Name}");
                    await DebugDirectory(subFolder, deep + 1);
                }
                else
                {
                    Debug.WriteLine($"{' '.Repeat(deep)}[FILE] {item.Name}");
                }
            }
        }
    }
}