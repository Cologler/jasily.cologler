using System.Collections.Generic;
using System.Diagnostics;

namespace System.Net
{
    public class JasilyHttpRequestPrinter
    {
        public Action<string> LineWriter { get; set; }
#if DEBUG
            = WriteToDebug;

        private static void WriteToDebug(string line)
        {
            Debug.WriteLine(line);
        }
#endif

        public string Print(HttpListenerRequest obj)
        {
            var writer = this.LineWriter;
            if (writer == null) return "NULL";

            var text = GetTexts(obj).AsLines();
            writer(text);
            return text;
        }

        static IEnumerable<string> GetTexts(HttpListenerRequest request)
        {
            if (request != null)
            {
                yield return $"{request.HttpMethod} {request.RawUrl} HTTP/{request.ProtocolVersion}";
                foreach (var key in request.Headers.AllKeys)
                    yield return $"{key} = {request.Headers[key]}";
            }
        }
    }
}
