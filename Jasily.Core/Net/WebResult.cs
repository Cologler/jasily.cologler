using System.Diagnostics;

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
            if (e == null) throw new ArgumentNullException("e");

            this.Type = WebResultType.WebException;
            this.WebException = e;
            this.Response = e.Response;
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

            return this.Result;
        }
    }
}
