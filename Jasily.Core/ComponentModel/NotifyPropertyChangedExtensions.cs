using System.Collections.Generic;
using JetBrains.Annotations;

namespace System.ComponentModel
{
    public static class NotifyPropertyChangedExtensions
    {
        public static void Invoke([NotNull] this PropertyChangedEventHandler handler, object sender, string propertyName)
            => handler.Invoke(sender, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// if e != null, call e() with propertyName
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        /// <param name="propertyName"></param>
        public static void Fire([CanBeNull] this PropertyChangedEventHandler handler, object sender, string propertyName)
            => handler?.Invoke(sender, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// if e != null, call e() with mulit propertyNames
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        /// <param name="propertyNames"></param>
        public static void Fire([CanBeNull] this PropertyChangedEventHandler handler, object sender, params string[] propertyNames)
        {
            if (handler == null) return;
            foreach (var propertyName in propertyNames)
                handler(sender, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// if e != null, call e() with mulit propertyNames
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        /// <param name="propertyNames"></param>
        public static void Fire([CanBeNull] this PropertyChangedEventHandler handler, object sender, IEnumerable<string> propertyNames)
        {
            if (handler == null) return;
            foreach (var propertyName in propertyNames)
                handler(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
