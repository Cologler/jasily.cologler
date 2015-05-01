using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Sockets
{
    public static class JasilySocket
    {
        /// <summary>
        /// Exception info see Socket.SendAsync()
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<SocketError> TrySendAsync(this Socket socket, SocketAsyncEventArgs args)
        {
            var tcs = new TaskCompletionSource<SocketError>();

            EventHandler<SocketAsyncEventArgs> onComplete = null;
            onComplete = (s, e) =>
            {
                args.Completed -= onComplete;
                tcs.SetResult(args.SocketError);
            };
            args.Completed += onComplete;

            if (!socket.SendAsync(args))
                return args.SocketError;
            else
                return await tcs.Task;
        }
    }
}
