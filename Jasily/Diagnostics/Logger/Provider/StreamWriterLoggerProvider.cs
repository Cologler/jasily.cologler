using System;
using System.IO;
using JetBrains.Annotations;

namespace Jasily.Diagnostics.Logger.Provider
{
    public class StreamWriterLoggerProvider : TextWriterLoggerProvider
    {
        private readonly StreamWriter writer;

        public StreamWriterLoggerProvider([NotNull] string id, [NotNull] StreamWriter writer)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            this.Id = id;
            this.writer = writer;
        }

        public override string Id { get; }

        protected override TextWriter GetTextWriter() => this.writer;
    }
}