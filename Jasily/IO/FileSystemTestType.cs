using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
    [Flags]
    public enum FileSystemTestType
    {
        None = 0,

        File = 1,

        Directory = 2,

        Multiple = 4
    }
}
