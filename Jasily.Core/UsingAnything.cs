using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public struct UsingAnything : IDisposable
    {
        readonly Action StartAction;
        readonly Action EndAction;

        public UsingAnything(Action start, Action end)
        {
            this.StartAction = start;
            this.EndAction = end;
        }

        public void Start()
        {
            if (this.StartAction != null)
            {
                this.StartAction();
            }
        }

        public void Dispose()
        {
            if (this.EndAction != null)
            {
                this.EndAction();
            }
        }
    }
}
