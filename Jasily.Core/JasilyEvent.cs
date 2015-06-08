using System.Collections.Generic;
using System.Linq;
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
        public static async Task FireAsync(this EventHandler e, object sender, EventArgs arg = null)
        {
            if (e != null)
                await Task.Run(() => e(sender, arg));
        }
        /// <summary>
        /// if e != null, call e() with mulit args
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static async Task FireAsync(this EventHandler e, object sender, params EventArgs[] args)
        {
            if (e != null)
                await Task.Run(() => e.Fire(sender, args));
        }
        /// <summary>
        /// if e != null, call e() with mulit args
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static async Task FireAsync(this EventHandler e, object sender, IEnumerable<EventArgs> args)
        {
            if (e != null)
                await Task.Run(() => e.Fire(sender, args.ToArray()));
        }

        /// <summary>
        /// if e != null, call e() with arg
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        public static async void BeginFire(this EventHandler e, object sender, EventArgs arg = null)
        {
            if (e != null)
                await Task.Run(() => e(sender, arg));
        }
        /// <summary>
        /// if e != null, call e() with mulit args
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static async void BeginFire(this EventHandler e, object sender, params EventArgs[] args)
        {
            if (e != null)
                await Task.Run(() => e.Fire(sender, args));
        }
        /// <summary>
        /// if e != null, call e() with mulit args
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static async void BeginFire(this EventHandler e, object sender, IEnumerable<EventArgs> args)
        {
            if (e != null)
                await Task.Run(() => e.Fire(sender, args.ToArray()));
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
        /// <summary>
        /// if e != null, call e() with mulit args
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static async Task FireAsync<T>(this EventHandler<T> e, object sender, params T[] args)
        {
            if (e != null)
                await Task.Run(() => e.Fire(sender, args));
        }
        /// <summary>
        /// if e != null, call e() with mulit args
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static async Task FireAsync<T>(this EventHandler<T> e, object sender, IEnumerable<T> args)
        {
            if (e != null)
                await Task.Run(() => e.Fire(sender, args.ToArray()));
        }

        /// <summary>
        /// if e != null, call e() with arg
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        public static async void BeginFire<T>(this EventHandler<T> e, object sender, T arg)
        {
            if (e != null)
                await Task.Run(() => e(sender, arg));
        }
        /// <summary>
        /// if e != null, call e() with mulit args
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static async void BeginFire<T>(this EventHandler<T> e, object sender, params T[] args)
        {
            if (e != null)
                await Task.Run(() => e.Fire(sender, args));
        }
        /// <summary>
        /// if e != null, call e() with mulit args
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static async void BeginFire<T>(this EventHandler<T> e, object sender, IEnumerable<T> args)
        {
            if (e != null)
                await Task.Run(() => e.Fire(sender, args.ToArray()));
        }
    }
}
