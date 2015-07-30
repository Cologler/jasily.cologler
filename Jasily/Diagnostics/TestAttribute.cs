using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Diagnostics
{
    public abstract class TestAttribute : Attribute
    {
        public abstract bool Test(object obj);
    }
}
