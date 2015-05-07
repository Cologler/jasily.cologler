using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Shell
{
    public interface ISingleInstanceApp
    {
        bool SignalExternalCommandLineArgs(string[] args);
    } 
}
