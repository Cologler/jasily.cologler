using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Cache
{
    public struct CacheTimeStamp
    {
        public DateTime DeadTime;

        public CacheTimeStamp(DateTime dead)
        {
            DeadTime = dead.ToUniversalTime();
        }
        public CacheTimeStamp(TimeSpan after)
        {
            DeadTime = DateTime.UtcNow + after;
        }
    }
}
