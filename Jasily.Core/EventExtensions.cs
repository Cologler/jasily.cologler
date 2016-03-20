using System.Collections.Generic;
using System.Threading.Tasks;

namespace System
{
    public static class EventExtensions
    {
        public static void Invoke(this EventHandler e, object sender) => e.Invoke(sender, EventArgs.Empty);
        public static void Invoke(this EventHandler e, object sender, params EventArgs[] args)
        {
            foreach (var arg in args) e.Invoke(sender, arg);
        }
        public static void Invoke(this EventHandler e, object sender, IEnumerable<EventArgs> args)
        {
            foreach (var arg in args) e.Invoke(sender, arg);
        }
        public static void Invoke<T>(this EventHandler<T> e, object sender) => e.Invoke(sender, default(T));
        public static void Invoke<T>(this EventHandler<T> e, object sender, params T[] args)
        {
            foreach (var arg in args) e.Invoke(sender, arg);
        }
        public static void Invoke<T>(this EventHandler<T> e, object sender, IEnumerable<T> args)
        {
            foreach (var arg in args) e.Invoke(sender, arg);
        }

        #region async

        public static async Task InvokeAsync(this EventHandler e, object sender)
            => await Task.Run(() => e.Invoke(sender));

        public static async Task InvokeAsync(this EventHandler e, object sender, EventArgs arg)
            => await Task.Run(() => e.Invoke(sender, arg));

        public static async Task InvokeAsync(this EventHandler e, object sender, params EventArgs[] args)
            => await Task.Run(() => e.Invoke(sender, args));

        public static async Task InvokeAsync(this EventHandler e, object sender, IEnumerable<EventArgs> args)
            => await Task.Run(() => e.Invoke(sender, args));

        public static async Task InvokeAsync<T>(this EventHandler<T> e, object sender)
            => await Task.Run(() => e.Invoke(sender));

        public static async Task InvokeAsync<T>(this EventHandler<T> e, object sender, T arg)
            => await Task.Run(() => e.Invoke(sender, arg));

        public static async Task InvokeAsync<T>(this EventHandler<T> e, object sender, params T[] args)
            => await Task.Run(() => e.Invoke(sender, args));

        public static async Task InvokeAsync<T>(this EventHandler<T> e, object sender, IEnumerable<T> args)
            => await Task.Run(() => e.Invoke(sender, args));

        public static async Task InvokeAsync(this EventHandler e, object sender, Action callback)
        {
            await Task.Run(() =>
            {
                e.Invoke(sender);
                callback();
            });
        }

        public static async Task InvokeAsync(this EventHandler e, object sender, Action callback, EventArgs arg)
        {
            await Task.Run(() =>
            {
                e.Invoke(sender, arg);
                callback();
            });
        }

        public static async Task InvokeAsync(this EventHandler e, object sender, Action callback, params EventArgs[] args)
        {
            await Task.Run(() =>
            {
                e.Invoke(sender, args);
                callback();
            });
        }

        public static async Task InvokeAsync(this EventHandler e, object sender, Action callback, IEnumerable<EventArgs> args)
        {
            await Task.Run(() =>
            {
                e.Invoke(sender, args);
                callback();
            });
        }

        public static async Task InvokeAsync<T>(this EventHandler<T> e, object sender, Action callback)
        {
            await Task.Run(() =>
            {
                e.Invoke(sender);
                callback();
            });
        }

        public static async Task InvokeAsync<T>(this EventHandler<T> e, object sender, Action callback, T arg)
        {
            await Task.Run(() =>
            {
                e.Invoke(sender, arg);
                callback();
            });
        }

        public static async Task InvokeAsync<T>(this EventHandler<T> e, object sender, Action callback, params T[] args)
        {
            await Task.Run(() =>
            {
                e.Invoke(sender, args);
                callback();
            });
        }

        public static async Task InvokeAsync<T>(this EventHandler<T> e, object sender, Action callback, IEnumerable<T> args)
        {
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
        public static void Fire(this EventHandler e, object sender, params EventArgs[] args) => e?.Invoke(sender, args);
        public static void Fire(this EventHandler e, object sender, IEnumerable<EventArgs> args) => e?.Invoke(sender, args);
        public static void Fire<T>(this EventHandler<T> e, object sender) => e?.Invoke(sender);
        public static void Fire<T>(this EventHandler<T> e, object sender, T arg) => e?.Invoke(sender, arg);
        public static void Fire<T>(this EventHandler<T> e, object sender, params T[] args) => e?.Invoke(sender, args);
        public static void Fire<T>(this EventHandler<T> e, object sender, IEnumerable<T> args) => e?.Invoke(sender, args);

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
        public static async Task FireAsync(this EventHandler e, object sender, params EventArgs[] args)
        {
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync;
        }
        public static async Task FireAsync(this EventHandler e, object sender, IEnumerable<EventArgs> args)
        {
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
        public static async Task FireAsync<T>(this EventHandler<T> e, object sender, params T[] args)
        {
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync;
        }
        public static async Task FireAsync<T>(this EventHandler<T> e, object sender, IEnumerable<T> args)
        {
            var invokeAsync = e?.InvokeAsync(sender, args);
            if (invokeAsync != null) await invokeAsync;
        }

        public static void BeginFire(this EventHandler e, object sender) => e?.BeginInvoke(sender);
        public static void BeginFire(this EventHandler e, object sender, EventArgs arg) => e?.BeginInvoke(sender, arg);
        public static void BeginFire(this EventHandler e, object sender, params EventArgs[] args) => e?.BeginInvoke(sender, args);
        public static void BeginFire(this EventHandler e, object sender, IEnumerable<EventArgs> args) => e?.BeginInvoke(sender, args);
        public static void BeginFire<T>(this EventHandler<T> e, object sender) => e?.BeginInvoke(sender);
        public static void BeginFire<T>(this EventHandler<T> e, object sender, T arg) => e?.BeginInvoke(sender, arg);
        public static void BeginFire<T>(this EventHandler<T> e, object sender, params T[] args) => e?.BeginInvoke(sender, args);
        public static void BeginFire<T>(this EventHandler<T> e, object sender, IEnumerable<T> args) => e?.BeginInvoke(sender, args);

        #endregion
    }
}
