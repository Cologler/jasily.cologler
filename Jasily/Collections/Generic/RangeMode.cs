using System;

namespace Jasily.Collections.Generic
{
    [Flags]
    public enum RangeMode : byte
    {
        None = 0,

        IncludeMin = 1,

        IncludeMax = 2,
    }
}