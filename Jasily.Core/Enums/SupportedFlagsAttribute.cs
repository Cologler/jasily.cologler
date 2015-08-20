using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Enums
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SupportedFlagsAttribute : Attribute
    {
        public SupportedFlagsAttribute(int flags)
        {
            this.SupportedFlags = flags;
        }

        public int SupportedFlags { get; }

        public bool IsSupport(int flag)
        {
            return (this.SupportedFlags & flag) == flag;
        }
    }
}
