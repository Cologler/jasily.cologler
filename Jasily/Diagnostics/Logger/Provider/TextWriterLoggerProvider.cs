using System.IO;
using System.Threading.Tasks;

namespace Jasily.Diagnostics.Logger.Provider
{
    public abstract class TextWriterLoggerProvider : LoggerProvider
    {
        protected abstract TextWriter GetTextWriter();

        public override void Write(string message)
            => this.GetTextWriter().Write(message);

        public override void WriteLine(string message)
            => this.GetTextWriter().WriteLine(message);

        public override async Task WriteAsync(string message)
            => await this.GetTextWriter().WriteAsync(message);

        public override async Task WriteLineAsync(string message)
            => await this.GetTextWriter().WriteLineAsync(message);
    }
}