using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace System.ComponentModel
{
    public static class JasilyINotifyPropertyChanged
    {
        /// <summary>
        /// if e != null, call e() with propertyName
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="propertyName"></param>
        public static void Fire(this PropertyChangedEventHandler e, object sender, string propertyName)
        {
            if (e != null)
                e(sender, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// if e != null, call e() with mulit propertyNames
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void Fire(this PropertyChangedEventHandler e, object sender, params string[] propertyNames)
        {
            if (e != null)
            {
                foreach (var propertyName in propertyNames)
                    e(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
        /// <summary>
        /// if e != null, call e() with mulit propertyNames
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="propertyNames"></param>
        public static void Fire(this PropertyChangedEventHandler e, object sender, IEnumerable<string> propertyNames)
        {
            if (e != null)
            {
                foreach (var propertyName in propertyNames)
                    e(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
