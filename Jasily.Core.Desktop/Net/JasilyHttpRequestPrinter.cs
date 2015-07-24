using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Net
{
    public class JasilyHttpRequestPrinter
    {
        public Action<string> LineWriter { get; set; }
#if DEBUG
            = WriteToDebug;
#endif

        private static void WriteToDebug(string line)
        {
            Debug.WriteLine(line);
        }

        public void Print(HttpListenerRequest request)
        {
            var writer = this.LineWriter;
            if (writer == null) return;

            writer($"{request.HttpMethod} {request.RawUrl} HTTP/{request.ProtocolVersion}");
            foreach (var key in request.Headers.AllKeys)
                writer($"[Header] {key} = {request.Headers[key]}");
            writer("");
        }
    }
}
