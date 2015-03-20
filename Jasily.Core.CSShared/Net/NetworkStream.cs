using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace System.Net
{
#if WINDOWS_PHONE_80
    /// <summary>
    /// alse see: http://www.mgenware.com/blog/?p=448, modify by myself
    /// </summary>
    public class NetworkStream : Stream
    {
        Socket _socket;
        bool _writeable;
        bool _readable;

        public NetworkStream(Socket socket)
            : this(socket, FileAccess.ReadWrite)
        { }

        public NetworkStream(Socket socket, FileAccess access)
        {
            if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }
            if (!socket.Connected)
            {
                throw new IOException("Socket is not connected");
            }

            _socket = socket;
            _readable = (access & FileAccess.Read) == FileAccess.Read;
            _writeable = (access & FileAccess.Write) == FileAccess.Write;
        }

        public override bool CanRead
        {
            get { return _readable; }
        }

        public override bool CanWrite
        {
            get { return _writeable; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override int Read(byte[] buffer, int offset, int size)
        {
            ValidatePas(buffer, offset, size);
            if (!_readable)
            {
                throw new InvalidOperationException();
            }

            var args = new SocketAsyncEventArgs();
            var eventWaitHandle = new AutoResetEvent(false);
            var socketError = SocketError.Success;
            var readCount = 0;

            args.SetBuffer(buffer, offset, size);
            args.Completed += (ss, ee) =>
                {
                    socketError = ee.SocketError;
                    readCount = ee.BytesTransferred;
                    eventWaitHandle.Set();
                };

            _socket.ReceiveAsync(args);
            eventWaitHandle.WaitOne();

            eventWaitHandle.Close();
            if (socketError != SocketError.Success)
            {
                throw new SocketException((int)socketError);
            }

            return readCount;
        }

        public override void Write(byte[] buffer, int offset, int size)
        {
            ValidatePas(buffer, offset, size);
            if (!_writeable)
            {
                throw new InvalidOperationException();
            }

            var args = new SocketAsyncEventArgs();
            var eventWaitHandle = new AutoResetEvent(false);
            var socketError = SocketError.Success;

            args.SetBuffer(buffer, offset, size);
            args.Completed += (ss, ee) =>
            {
                socketError = ee.SocketError;
                eventWaitHandle.Set();
            };

            _socket.SendAsync(args);
            eventWaitHandle.WaitOne();

            eventWaitHandle.Close();
            if (socketError != SocketError.Success)
            {
                throw new SocketException((int)socketError);
            }
        }

        void ValidatePas(byte[] buffer, int offset, int size)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (offset < 0 || offset > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (size < 0 || size > buffer.Length - offset)
            {
                throw new ArgumentOutOfRangeException("size");
            }
        }

        public override void Flush()
        { }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
    }
#endif
}