using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Magnet
{
    interface IMagnetElement
    {
        string NodeName { get; }

        string NodeValue { get; }

        string AsMagnetElement();
    }
}
