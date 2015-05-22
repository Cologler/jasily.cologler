using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Net
{
    public class WebResult
    {
        public WebResult()
        {
            Type = WebResultType.Succeed;
        }

        public WebResult(WebException e)
        {
            if (e == null) throw new ArgumentNullException("e");
            Type = WebResultType.WebException;
            WebException = e;
        }

        public bool IsSuccess
        {
            get { return this.Type == WebResultType.Succeed; }
        }

        public WebResultType Type { get; protected set; }

        /// <summary>
        /// return null if this.Type != WebResultType.WebException
        /// </summary>
        public WebException WebException { get; private set; }
    }

    public class WebResult<T> : WebResult
    {
        public WebResult(T result)
            : base()
        {
            Debug.Assert(result != null, "if result is null, you should use WebResult, not WebResult<T>");

            this.Result = result;
        }

        public WebResult(WebException e)
            : base(e)
        {
        }

        public T Result { get; private set; }

        /// <summary>
        /// throw System.Net.WebException if this.WebException not null.
        /// </summary>
        /// <exception cref="System.Net.WebException"></exception>
        /// <returns></returns>
        public T GetResultOrThrow()
        {
            if (this.WebException != null)
                throw this.WebException;
            return Result;
        }

        public byte[] OriginBody { get; private set; }
    }
}
