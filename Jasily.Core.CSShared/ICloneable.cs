using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public interface ICloneable<out T>
#if DESKTOP
        : ICloneable
#endif
    {
        new T Clone();
    }
}
