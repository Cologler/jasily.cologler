using System;
using System.Diagnostics;
using System.Net;

namespace Jasily.Net
{
    public class WebResult : IDisposable
    {
        /// <summary>
        /// maybe null if not contain response.
        /// </summary>
        public WebResponse Response { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="System.ArgumentNullException">if response is null.</exception>
        /// <param name="response"></param>
        public WebResult(WebResponse response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            this.Type = WebResultType.Succeed;
            this.Response = response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="System.ArgumentNullException">if e is null.</exception>
        /// <param name="e"></param>
        public WebResult(WebException e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            this.Type = WebResultType.WebException;
            this.WebException = e;
            this.Response = e.Response;
        }

        public bool IsSuccess => this.Type == WebResultType.Succeed;

        public WebResultType Type { get; protected set; }

        /// <summary>
        /// return null if this.Type != WebResultType.WebException
        /// </summary>
        public WebException WebException { get; }

        public void Dispose()
        {
            this.Response?.Dispose();
        }
    }

    public class WebResult<T> : WebResult
    {
        public WebResult(WebResponse response, T result)
            : base(response)
        {
            Debug.Assert(result != null, "if result is null, you should use WebResult, not WebResult<T>.");

            this.Result = result;
        }

        public WebResult(WebException e)
            : base(e)
        {
        }

        public T Result { get; }

        /// <summary>
        /// throw System.Net.WebException if this.WebException not null.
        /// </summary>
        /// <exception cref="System.Net.WebException"></exception>
        /// <returns></returns>
        public T GetResultOrThrow()
        {
            if (this.WebException != null)
                throw this.WebException;

            return this.Result;
        }

        public WebResult<TOut> Cast<TOut>(Func<T, TOut> selector)
        {
            return this.WebException != null
                ? new WebResult<TOut>(this.WebException)
                : new WebResult<TOut>(this.Response, selector(this.Result));
        }
    }
}
