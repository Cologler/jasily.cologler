using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// allow class copy member from other instance
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICopy<in T>
    {
        void Copy(T source);
    }
}
