using System.Collections.Generic;
using System.Threading.Tasks;

namespace System
{
    public static class JasilyEvent
    {
        /// <summary>
        /// if e != null, call e() with arg
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        public static void Fire(this EventHandler e, object sender, EventArgs arg = null)
        {
            if (e != null)
                e(sender, arg);
        }
        /// <summary>
        /// if e != null, call e() with mulit args
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void Fire(this EventHandler e, object sender, params EventArgs[] args)
        {
            if (e != null)
            {
                foreach (var arg in args)
                    e(sender, arg);
            }
        }
        /// <summary>
        /// if e != null, call e() with mulit args
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void Fire(this EventHandler e, object sender, IEnumerable<EventArgs> args)
        {
            if (e != null)
            {
                foreach (var arg in args)
                    e(sender, arg);
            }
        }

        /// <summary>
        /// if e != null, call e() with arg
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        public static void Fire<T>(this EventHandler<T> e, object sender, T arg)
        {
            if (e != null)
                e(sender, arg);
        }
        /// <summary>
        /// if e != null, call e() with mulit args
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void Fire<T>(this EventHandler<T> e, object sender, params T[] args)
        {
            if (e != null)
            {
                foreach (var arg in args)
                    e(sender, arg);
            }
        }
        /// <summary>
        /// if e != null, call e() with mulit args
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void Fire<T>(this EventHandler<T> e, object sender, IEnumerable<T> args)
        {
            if (e != null)
            {
                foreach (var arg in args)
                    e(sender, arg);
            }
        }

        /// <summary>
        /// if e != null, call e() with arg
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        public static async Task FireAsync<T>(this EventHandler<T> e, object sender, T arg)
        {
            if (e != null)
                await Task.Run(() => e(sender, arg));
        }
    }
}
