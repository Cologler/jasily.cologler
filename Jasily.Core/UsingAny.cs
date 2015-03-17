using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public struct UsingAny : IDisposable
    {
        Action End;

        public UsingAny(Action begin, Action end)
        {
            this.End = end;
            begin();
        }

        public void Dispose()
        {
            this.End();
            this.End = null;
        }
    }
}
