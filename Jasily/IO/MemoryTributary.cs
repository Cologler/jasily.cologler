using System;
using System.Collections.Generic;
using System.IO;

namespace Jasily.IO
{
    /// <summary>
    /// source: http://memorytributary.codeplex.com/
    /// MemoryTributary is a re-implementation of MemoryStream that uses a dynamic list of byte arrays as a backing store, instead of a single byte array, the allocation
    /// of which will fail for relatively small streams as it requires contiguous memory.
    /// </summary>
    public class MemoryTributary : Stream
    {
        #region Constructors

        public MemoryTributary()
        {
            this.Position = 0;
        }

        public MemoryTributary(byte[] source)
        {
            this.Write(source, 0, source.Length);
            this.Position = 0;
        }

        public MemoryTributary(int length)
        {
            this.SetLength(length);
            this.Position = length;
            var d = this.block;   //access block to prompt the allocation of memory
            this.Position = 0;
        }

        #endregion

        #region Status Properties

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        #endregion

        #region Public Properties

        public override long Length
        {
            get { return this.length; }
        }

        public override long Position { get; set; }

        #endregion

        #region Members

        protected long length = 0;

        protected long blockSize = 65536;

        protected List<byte[]> blocks = new List<byte[]>();

        #endregion

        #region Internal Properties

        /* Use these properties to gain access to the appropriate block of memory for the current Position */

        /// <summary>
        /// The block of memory currently addressed by Position
        /// </summary>
        protected byte[] block
        {
            get
            {
                while (this.blocks.Count <= this.blockId)
                    this.blocks.Add(new byte[this.blockSize]);
                return this.blocks[(int) this.blockId];
            }
        }
        /// <summary>
        /// The id of the block currently addressed by Position
        /// </summary>
        protected long blockId
        {
            get { return this.Position / this.blockSize; }
        }
        /// <summary>
        /// The offset of the byte currently addressed by Position, into the block that contains it
        /// </summary>
        protected long blockOffset
        {
            get { return this.Position % this.blockSize; }
        }

        #endregion

        #region Public Stream Methods

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var lcount = (long)count;

            if (lcount < 0)
            {
                throw new ArgumentOutOfRangeException("count", lcount, "Number of bytes to copy cannot be negative.");
            }

            var remaining = (this.length - this.Position);
            if (lcount > remaining)
                lcount = remaining;

            if (buffer == null)
            {
                throw new ArgumentNullException("buffer", "Buffer cannot be null.");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", offset, "Destination offset cannot be negative.");
            }

            var read = 0;
            long copysize = 0;
            do
            {
                copysize = Math.Min(lcount, (this.blockSize - this.blockOffset));
                Buffer.BlockCopy(this.block, (int) this.blockOffset, buffer, offset, (int)copysize);
                lcount -= copysize;
                offset += (int)copysize;

                read += (int)copysize;
                this.Position += copysize;

            } while (lcount > 0);

            return read;

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
                    this.Position = this.Length - offset;
                    break;
            }
            return this.Position;
        }

        public override void SetLength(long value)
        {
            this.length = value;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var initialPosition = this.Position;
            int copysize;
            try
            {
                do
                {
                    copysize = Math.Min(count, (int)(this.blockSize - this.blockOffset));

                    this.EnsureCapacity(this.Position + copysize);

                    Buffer.BlockCopy(buffer, (int)offset, this.block, (int) this.blockOffset, copysize);
                    count -= copysize;
                    offset += copysize;

                    this.Position += copysize;

                } while (count > 0);
            }
            catch (Exception e)
            {
                this.Position = initialPosition;
                throw e;
            }
        }

        public override int ReadByte()
        {
            if (this.Position >= this.length)
                return -1;

            var b = this.block[this.blockOffset];
            this.Position++;

            return b;
        }

        public override void WriteByte(byte value)
        {
            this.EnsureCapacity(this.Position + 1);
            this.block[this.blockOffset] = value;
            this.Position++;
        }

        protected void EnsureCapacity(long intended_length)
        {
            if (intended_length > this.length)
                this.length = (intended_length);
        }

        #endregion

        #region IDispose

        /* http://msdn.microsoft.com/en-us/library/fs2xkftw.aspx */
        protected override void Dispose(bool disposing)
        {
            /* We do not currently use unmanaged resources */
            base.Dispose(disposing);
        }

        #endregion

        #region Public Additional Helper Methods

        /// <summary>
        /// Returns the entire content of the stream as a byte array. This is not safe because the call to new byte[] may 
        /// fail if the stream is large enough. Where possible use methods which operate on streams directly instead.
        /// </summary>
        /// <returns>A byte[] containing the current data in the stream</returns>
        public byte[] ToArray()
        {
            var firstposition = this.Position;
            this.Position = 0;
            var destination = new byte[this.Length];
            this.Read(destination, 0, (int) this.Length);
            this.Position = firstposition;
            return destination;
        }

        /// <summary>
        /// Reads length bytes from source into the this instance at the current position.
        /// </summary>
        /// <param name="source">The stream containing the data to copy</param>
        /// <param name="length">The number of bytes to copy</param>
        public void ReadFrom(Stream source, long length)
        {
            var buffer = new byte[4096];
            int read;
            do
            {
                read = source.Read(buffer, 0, (int)Math.Min(4096, length));
                length -= read;
                this.Write(buffer, 0, read);

            } while (length > 0);
        }

        /// <summary>
        /// Writes the entire stream into destination, regardless of Position, which remains unchanged.
        /// </summary>
        /// <param name="destination">The stream to write the content of this stream to</param>
        public void WriteTo(Stream destination)
        {
            var initialpos = this.Position;
            this.Position = 0;
            this.CopyTo(destination);
            this.Position = initialpos;
        }

        #endregion
    }
}

