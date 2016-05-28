using System.IO;
using System.Text;
using JetBrains.Annotations;

namespace System.Runtime.Serialization.Json
{
    public static class DesktopDataContractJsonSerializerExtensions
    {
        public static void ObjectToJsonFile([NotNull] this object obj, string path, bool overwrite = false)
        {
            var json = obj.ObjectToJson();
            using (var stream = File.Open(path, overwrite ? FileMode.Create : FileMode.CreateNew))
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                writer.Write(json);
            }
        }

        public static T JsonFileToObject<T>([NotNull] this FileInfo file)
        {
            using (var reader = file.OpenText())
            {
                return reader.ReadToEnd().JsonToObject<T>();
            }
        }
    }
}