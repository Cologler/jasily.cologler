using System;
using System.Collections.Generic;
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
            this.Result = result;
        }

        public WebResult(WebException e)
            : base(e)
        {
        }

        public T Result { get; private set; }

        public byte[] OriginBody { get; private set; }
    }
}
