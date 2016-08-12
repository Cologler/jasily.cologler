using System;

namespace Jasily.Diagnostics
{
    [Flags]
    public enum JasilyLoggerMode : byte
    {
        // x.......0
        NotLog = 0,
        // x.......1
        Log = 1,
        // x......01
        Debug = 2,
        // x......11
        Release = 3,
        // x.....1.1
        Track = 5,
        // x....11.1
        RealTimeTrack = 13
    }
}