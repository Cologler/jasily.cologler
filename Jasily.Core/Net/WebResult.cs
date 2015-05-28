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
        /// <summary>
        /// maybe null if not contain response.
        /// </summary>
        public WebResponse Response { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="System.ArgumentNullException">if response is null.</exception>
        /// <param name="response"></param>
        public WebResult(WebResponse response)
        {
            if (response == null) throw new ArgumentNullException("response");
            
            Type = WebResultType.Succeed;
            Response = response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <exception cref="System.ArgumentNullException">if response or e is null.</exception>
        /// <param name="e"></param>
        public WebResult(WebResponse response, WebException e)
        {
            if (response == null) throw new ArgumentNullException("response");
            if (e == null) throw new ArgumentNullException("e");

            Type = WebResultType.WebException;
            Response = response;
            WebException = e;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="System.ArgumentNullException">if e is null.</exception>
        /// <param name="e"></param>
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
        public WebResult(WebResponse response, T result)
            : base(response)
        {
            Debug.Assert(result != null, "if result is null, you should use WebResult, not WebResult<T>.");

            this.Result = result;
        }

        public WebResult(WebResponse response, WebException e)
            : base(response, e)
        {
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
    }
}
