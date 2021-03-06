﻿using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace System.Net.Sockets
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
            _socket = socket.ThrowIfNull("socket");

            if (_socket.ProtocolType != ProtocolType.Tcp)
            {
                throw new ArgumentException("socket protocol type must be tcp.");
            }

            if (!_socket.Connected)
            {
                throw new IOException("socket is not connected");
            }

            _readable = access.HasFlag(FileAccess.Read);
            _writeable = access.HasFlag(FileAccess.Write);
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

            if (!this.CanRead)
            {
                throw new InvalidOperationException("stream can not read.");
            }

            using (var waiter = new ManualResetEventSlim())
            {
                using (var args = new SocketAsyncEventArgs())
                {
                    var readCount = 0;

                    EventHandler<SocketAsyncEventArgs> com = null;
                    com = new EventHandler<SocketAsyncEventArgs>((ss, ee) =>
                    {
                        args.Completed -= com;
                        waiter.Set();
                    });
                    args.Completed += com;

                    args.SetBuffer(buffer, offset, size);

                    if (_socket.ReceiveAsync(args))
                        waiter.Wait();

                    if (args.SocketError != SocketError.Success)
                    {
                        throw new SocketException((int)args.SocketError);
                    }

                    return readCount;
                }
            }
        }

        public override void Write(byte[] buffer, int offset, int size)
        {
            ValidatePas(buffer, offset, size);

            if (!this.CanWrite)
            {
                throw new InvalidOperationException("stream can not write.");
            }

            using (var waiter = new ManualResetEventSlim())
            {
                using (var args = new SocketAsyncEventArgs())
                {
                    EventHandler<SocketAsyncEventArgs> com = null;
                    com = new EventHandler<SocketAsyncEventArgs>((ss, ee) =>
                    {
                        args.Completed -= com;
                        waiter.Set();
                    });
                    args.Completed += com;

                    args.SetBuffer(buffer, offset, size);

                    if (_socket.SendAsync(args))
                        waiter.Wait();

                    if (args.SocketError != SocketError.Success)
                    {
                        throw new SocketException((int)args.SocketError);
                    }
                }
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