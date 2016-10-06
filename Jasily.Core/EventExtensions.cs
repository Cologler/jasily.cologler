using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace System
{
    public static class EventExtensions
    {
        public static void Invoke([NotNull] this EventHandler e, object sender)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            e.Invoke(sender, EventArgs.Empty);
        }

        public static void Invoke([NotNull] this EventHandler e, object sender, [NotNull] params EventArgs[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            foreach (var arg in args) e.Invoke(sender, arg);
        }

        public static void Invoke([NotNull] this EventHandler e, object sender, [NotNull] IEnumerable<EventArgs> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            foreach (var arg in args) e.Invoke(sender, arg);
        }

        public static void Invoke<T>([NotNull] this EventHandler<T> e, object sender)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            e.Invoke(sender, default(T));
        }

        public static void Invoke<T>([NotNull] this EventHandler<T> e, object sender, [NotNull] params T[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            foreach (var arg in args) e.Invoke(sender, arg);
        }

        public static void Invoke<T>([NotNull] this EventHandler<T> e, object sender, [NotNull] IEnumerable<T> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            foreach (var arg in args) e.Invoke(sender, arg);
        }

        public static void Invoke<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender,
            TEventArgs arg)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            e.Invoke(sender, arg);
        }

        public static void Invoke<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender,
            [NotNull] params EventArgs[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            foreach (var arg in args) e.Invoke(sender, arg);
        }

        public static void Invoke<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender,
            [NotNull] IEnumerable<TEventArgs> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            foreach (var arg in args) e.Invoke(sender, arg);
        }

        #region async

        public static async Task InvokeAsync([NotNull] this EventHandler e, object sender)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            await Task.Run(() => e.Invoke(sender));
        }

        public static async Task InvokeAsync([NotNull] this EventHandler e, object sender, EventArgs arg)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            await Task.Run(() => e.Invoke(sender, arg));
        }

        public static async Task InvokeAsync([NotNull] this EventHandler e, object sender,
            [NotNull] params EventArgs[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            await Task.Run(() => e.Invoke(sender, args));
        }

        public static async Task InvokeAsync([NotNull] this EventHandler e, object sender,
            [NotNull] IEnumerable<EventArgs> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            await Task.Run(() => e.Invoke(sender, args));
        }

        public static async Task InvokeAsync<T>([NotNull] this EventHandler<T> e, object sender)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            await Task.Run(() => e.Invoke(sender));
        }

        public static async Task InvokeAsync<T>([NotNull] this EventHandler<T> e, object sender, T arg)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            await Task.Run(() => e.Invoke(sender, arg));
        }

        public static async Task InvokeAsync<T>([NotNull] this EventHandler<T> e, object sender, params T[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            await Task.Run(() => e.Invoke(sender, args));
        }

        public static async Task InvokeAsync<T>([NotNull] this EventHandler<T> e, object sender,
            [NotNull] IEnumerable<T> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            await Task.Run(() => e.Invoke(sender, args));
        }

        public static async Task InvokeAsync<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            await Task.Run(() => e.Invoke(sender));
        }

        public static async Task InvokeAsync<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender, TEventArgs arg)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            await Task.Run(() => e.Invoke(sender, arg));
        }

        public static async Task InvokeAsync<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender, params TEventArgs[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            await Task.Run(() => e.Invoke(sender, args));
        }

        public static async Task InvokeAsync<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender,
            [NotNull] IEnumerable<TEventArgs> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (args == null) throw new ArgumentNullException(nameof(args));
            await Task.Run(() => e.Invoke(sender, args));
        }

        public static async Task InvokeAsync([NotNull] this EventHandler e, object sender, Action callback)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            await Task.Run(() =>
            {
                e.Invoke(sender);
                callback();
            });
        }

        public static async Task InvokeAsync([NotNull] this EventHandler e, object sender, [NotNull] Action callback, EventArgs arg)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            await Task.Run(() =>
            {
                e.Invoke(sender, arg);
                callback();
            });
        }

        public static async Task InvokeAsync([NotNull] this EventHandler e, object sender, [NotNull] Action callback,
            [NotNull] params EventArgs[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            if (args == null) throw new ArgumentNullException(nameof(args));
            await Task.Run(() =>
            {
                e.Invoke(sender, args);
                callback();
            });
        }

        public static async Task InvokeAsync([NotNull] this EventHandler e, object sender, [NotNull] Action callback,
            [NotNull] IEnumerable<EventArgs> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            if (args == null) throw new ArgumentNullException(nameof(args));
            await Task.Run(() =>
            {
                e.Invoke(sender, args);
                callback();
            });
        }

        public static async Task InvokeAsync<T>([NotNull] this EventHandler<T> e, object sender,
            [NotNull] Action callback)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            await Task.Run(() =>
            {
                e.Invoke(sender);
                callback();
            });
        }

        public static async Task InvokeAsync<T>([NotNull] this EventHandler<T> e, object sender,
            [NotNull] Action callback, T arg)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            await Task.Run(() =>
            {
                e.Invoke(sender, arg);
                callback();
            });
        }

        public static async Task InvokeAsync<T>([NotNull] this EventHandler<T> e, object sender,
            [NotNull] Action callback, params T[] args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            await Task.Run(() =>
            {
                e.Invoke(sender, args);
                callback();
            });
        }

        public static async Task InvokeAsync<T>([NotNull] this EventHandler<T> e, object sender,
            [NotNull] Action callback, [NotNull] IEnumerable<T> args)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            if (args == null) throw new ArgumentNullException(nameof(args));
            await Task.Run(() =>
            {
                e.Invoke(sender, args);
                callback();
            });
        }

        #endregion

        #region begin

        public static async void BeginInvoke(this EventHandler e, object sender)
            => await InvokeAsync(e, sender);

        public static async void BeginInvoke(this EventHandler e, object sender, EventArgs arg)
            => await InvokeAsync(e, sender, arg);

        public static async void BeginInvoke(this EventHandler e, object sender, params EventArgs[] args)
            => await InvokeAsync(e, sender, args);

        public static async void BeginInvoke(this EventHandler e, object sender, IEnumerable<EventArgs> args)
            => await InvokeAsync(e, sender, args);

        public static async void BeginInvoke<T>(this EventHandler<T> e, object sender)
            => await InvokeAsync(e, sender);

        public static async void BeginInvoke<T>(this EventHandler<T> e, object sender, T arg)
            => await InvokeAsync(e, sender, arg);

        public static async void BeginInvoke<T>(this EventHandler<T> e, object sender, params T[] args)
            => await InvokeAsync(e, sender, args);

        public static async void BeginInvoke<T>(this EventHandler<T> e, object sender, IEnumerable<T> args)
            => await InvokeAsync(e, sender, args);

        public static async void BeginInvoke<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender)
            => await InvokeAsync(e, sender);

        public static async void BeginInvoke<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender, TEventArgs arg)
            => await InvokeAsync(e, sender, arg);

        public static async void BeginInvoke<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender, params TEventArgs[] args)
            => await InvokeAsync(e, sender, args);

        public static async void BeginInvoke<T, TEventArgs>([NotNull] this TypedEventHandler<T, TEventArgs> e, T sender, IEnumerable<TEventArgs> args)
            => await InvokeAsync(e, sender, args);

        public static async void BeginInvoke(this EventHandler e, object sender, Action callback)
            => await InvokeAsync(e, sender, callback);

        public static async void BeginInvoke(this EventHandler e, object sender, Action callback, EventArgs arg)
            => await InvokeAsync(e, sender, callback, arg);

        public static async void BeginInvoke(this EventHandler e, object sender, Action callback, params EventArgs[] args)
            => await InvokeAsync(e, sender, callback, args);

        public static async void BeginInvoke(this EventHandler e, object sender, Action callback, IEnumerable<EventArgs> args)
            => await InvokeAsync(e, sender, callback, args);

        public static async void BeginInvoke<T>(this EventHandler<T> e, object sender, Action callback)
            => await InvokeAsync(e, sender, callback);

        public static async void BeginInvoke<T>(this EventHandler<T> e, object sender, Action callback, T arg)
            => await InvokeAsync(e, sender, callback, arg);

        public static async void BeginInvoke<T>(this EventHandler<T> e, object sender, Action callback, params T[] args)
            => await InvokeAsync(e, sender, callback, args);

        public static async void BeginInvoke<T>(this EventHandler<T> e, object sender, Action callback, IEnumerable<T> args)
            => await InvokeAsync(e, sender, callback, args);

        #endregion

        #region fire

        public static void Fire(this EventHandler e, object sender) => e?.Invoke(sender);

        public static void Fire(this EventHandler e, object sender, EventArgs arg) => e?.Invoke(sender, arg);

        public static void Fire(this EventHandler e, object sender, [NotNull] params EventArgs[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            e?.Invoke(sender, args);
        }

        public static void Fire(this EventHandler e, object sender, [NotNull] IEnumerable<EventArgs> args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            e?.Invoke(sender, args);
        }

        public static void Fire<T>(this EventHandler<T> e, object sender) => e?.Invoke(sender);

        public static void Fire<T>(this EventHandler<T> e, object sender, T arg) => e?.Invoke(sender, arg);

        public static void Fire<T>(this EventHandler<T> e, object sender, [NotNull] params T[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            e?.Invoke(sender, args);
        }

        public static void Fire<T>(this EventHandler<T> e, object sender, [NotNull] IEnumerable<T> args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            e?.Invoke(sender, args);
        }

        public static void Fire<T, TEventArgs>(this TypedEventHandler<T, TEventArgs> e, T sender) => e?.Invoke(sender);

        public static void Fire<T, TEventArgs>(this TypedEventHandler<T, TEventArgs> e, T sender, TEventArgs arg) => e?.Invoke(sender, arg);

        public static void Fire<T, TEventArgs>(this TypedEventHandler<T, TEventArgs> e, T sender, [NotNull] params TEventArgs[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            e?.Invoke(sender, args);
        }

        public static void Fire<T, TEventArgs>(this TypedEventHandler<T, TEventArgs> e, T sender, [NotNull] IEnumerable<TEventArgs> args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            e?.Invoke(sender, args);
        }

        #region fire async

        public static async Task FireAsync(this EventHandler e, object sender)
        {
            var invokeAsync = e?.InvokeAsync(sender);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync(this EventHandler e, object sender, EventArgs arg)
        {
            var invokeAsync = e?.InvokeAsync(sender, arg);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync(this EventHandler e, object sender, [NotNull] params EventArgs[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync(this EventHandler e, object sender, [NotNull] IEnumerable<EventArgs> args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync<T>(this EventHandler<T> e, object sender)
        {
            var invokeAsync = e?.InvokeAsync(sender);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync<T>(this EventHandler<T> e, object sender, T arg)
        {
            var invokeAsync = e?.InvokeAsync(sender, arg);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync<T>(this EventHandler<T> e, object sender, [NotNull] params T[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync<T>(this EventHandler<T> e, object sender, [NotNull] IEnumerable<T> args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync<T, TEventArgs>(this TypedEventHandler<T, TEventArgs> e, T sender)
        {
            var invokeAsync = e?.InvokeAsync(sender);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync<T, TEventArgs>(this TypedEventHandler<T, TEventArgs> e, T sender, TEventArgs arg)
        {
            var invokeAsync = e?.InvokeAsync(sender, arg);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync<T, TEventArgs>(this TypedEventHandler<T, TEventArgs> e, T sender, [NotNull] params TEventArgs[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync;
        }

        public static async Task FireAsync<T, TEventArgs>(this TypedEventHandler<T, TEventArgs> e, T sender, [NotNull] IEnumerable<TEventArgs> args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync;
        }

        #endregion

        #region begin fire

        public static void BeginFire(this EventHandler e, object sender) => e?.BeginInvoke(sender);

        public static void BeginFire(this EventHandler e, object sender, EventArgs arg) => e?.BeginInvoke(sender, arg);

        public static void BeginFire(this EventHandler e, object sender, params EventArgs[] args)
        {
            if (args != null) e?.BeginInvoke(sender, args);
        }

        public static void BeginFire(this EventHandler e, object sender, IEnumerable<EventArgs> args)
        {
            if (args != null) e?.BeginInvoke(sender, args);
        }

        public static void BeginFire<T>(this EventHandler<T> e, object sender) => e?.BeginInvoke(sender);

        public static void BeginFire<T>(this EventHandler<T> e, object sender, T arg) => e?.BeginInvoke(sender, arg);

        public static void BeginFire<T>(this EventHandler<T> e, object sender, [NotNull] params T[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            e?.BeginInvoke(sender, args);
        }

        public static void BeginFire<T>(this EventHandler<T> e, object sender, [NotNull] IEnumerable<T> args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            e?.BeginInvoke(sender, args);
        }

        public static void BeginFire<T, TEventArgs>(this TypedEventHandler<T, TEventArgs> e, T sender)
            => e?.BeginInvoke(sender);

        public static void BeginFire<T, TEventArgs>(this TypedEventHandler<T, TEventArgs> e, T sender, TEventArgs arg)
            => e?.BeginInvoke(sender, arg);

        public static void BeginFire<T, TEventArgs>(this TypedEventHandler<T, TEventArgs> e, T sender, [NotNull] params TEventArgs[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            e?.BeginInvoke(sender, args);
        }

        public static void BeginFire<T, TEventArgs>(this TypedEventHandler<T, TEventArgs> e, T sender, [NotNull] IEnumerable<TEventArgs> args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            e?.BeginInvoke(sender, args);
        }

        #endregion

        #endregion
    }
}
