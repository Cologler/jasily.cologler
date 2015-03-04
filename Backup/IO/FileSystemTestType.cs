using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
    [Flags]
    public enum FileSystemTestType : byte
    {
        /// <summary>
        /// 00000000
        /// </summary>
        None = 0,

        /// <summary>
        /// 00000001
        /// </summary>
        File = 1,

        /// <summary>
        /// 00000010
        /// </summary>
        Directory = 2,

        /// <summary>
        /// 00000011
        /// </summary>
        Single = 3,

        /// <summary>
        /// 00000101
        /// </summary>
        MultipleFile = 5,

        /// <summary>
        /// 00001010
        /// </summary>
        MultipleDirectory = 10,

        /// <summary>
        /// 00001111
        /// </summary>
        Multiple = 15
    }
}
