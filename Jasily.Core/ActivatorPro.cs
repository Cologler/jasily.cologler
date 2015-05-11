using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class ActivatorPro
    {
        public static T CreateInstance<T>(this Type type)
        {
            return (T)Activator.CreateInstance(type);
        }

        public static T CreateInstance<T>(this Type type, params object[] args)
        {
            return (T)Activator.CreateInstance(type, args);
        }

        public static T CreateInstance<T>(params object[] args)
        {
            return (T)Activator.CreateInstance(typeof(T), args);
        }
    }
}
