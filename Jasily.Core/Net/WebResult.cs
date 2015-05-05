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
            IsSuccess = true;
        }

        public WebResult(WebException e)
        {
            IsSuccess = false;
            Exception = e;
        }

        public bool IsSuccess { get; private set; }

        public WebException Exception { get; private set; }
    }

    public class WebResult<T> : WebResult
    {
        public WebResult(T result)
            : base()
        {
            Result = result;
        }

        public WebResult(WebException e)
            : base(e)
        {
        }

        public T Result { get; private set; }
    }
}
