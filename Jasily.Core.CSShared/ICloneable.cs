using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
#if !DESKTOP
    public interface ICloneable
    {
        object Clone();
    }
#endif

    public interface ICloneable<out T>
        : ICloneable
    {
        new T Clone();
    }
}
