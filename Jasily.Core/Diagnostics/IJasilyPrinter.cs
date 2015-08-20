using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Diagnostics
{
    public interface IJasilyPrinter<T>
    {
        /// <summary>
        /// print a object as mulit-line text.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        string Print(T obj);
    }
}
