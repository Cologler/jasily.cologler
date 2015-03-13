using System.Collections.Generic;
using System.Linq;

namespace System.IO
{
    public sealed class MergeReadonlyStream : Stream
    {
        private Stream[] InnerStreams;

        public MergeReadonlyStream(IEnumerable<Stream> source)
        {
            InnerStreams = source.ToArray();

            if (InnerStreams.Any(z => z == null))
                throw new ArgumentNullException("some stream in source was null.");
        }

        public int StreamCount
        {
            get { return InnerStreams.Length; }
        }

        public override bool CanRead
        {
            get { return InnerStreams.All(z => z.CanRead); }
        }
        public override bool CanSeek
        {
            get { return InnerStreams.All(z => z.CanSeek); }
        }
        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            foreach (var stream in InnerStreams)
                stream.Flush();
        }

        public override long Length
        {
            get { return InnerStreams.Sum(z => z.Length); }
        }

        public override long Position
        {
            get { return InnerStreams.Sum(z => z.Position); }
            set
            {
                foreach (var i in InnerStreams)
                {
                    if (value <= 0)
                        i.Position = 0;
                    else
                    {
                        i.Position = Math.Min(i.Length, value);
                        value = value - i.Length;
                    }
                }
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int total = 0;

            foreach (var i in InnerStreams)
            {
                total += i.Read(buffer, offset + total, count - total);

                if (total >= count)
                    break;
            }

            return total;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    this.Position = offset;
                    break;

                case SeekOrigin.Current:
                    this.Position += offset;
                    break;

                case SeekOrigin.End:
                    this.Position = this.Length + offset;
                    break;
            }

            return this.Position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public static Stream Create(params Stream[] streams)
        {
            return Create(streams.ToList());
        }
        public static Stream Create(IEnumerable<Stream> streams)
        {
            var all = streams.ToList();

            if (all.Count <= 0)
                throw new ArgumentOutOfRangeException("count must big than 0");

            if (all.Count == 1)
                return all[0];

            return new MergeReadonlyStream(streams);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {

            }

            foreach (var i in InnerStreams)
                i.Dispose();
        }
    }
}
