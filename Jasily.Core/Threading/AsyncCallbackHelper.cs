using System.Threading.Tasks;
using JetBrains.Annotations;

namespace System.Threading
{
    public static class AsyncCallbackHelper
    {
        public static async Task<T> ToTask<T>([NotNull] Func<AsyncCallback, object, IAsyncResult> asyncFunc,
            [NotNull] Func<IAsyncResult, T> callback)
        {
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            var task = new TaskCompletionSource<T>();
            asyncFunc(ac =>
            {
                try
                {
                    task.SetResult(callback(ac));
                }
                catch (Exception e)
                {
                    task.SetException(e);
                }
            }, null);
            return await task.Task;
        }

        public static async Task<T> ToTask<T>([NotNull] Func<AsyncCallback, object, IAsyncResult> asyncFunc,
            [NotNull] Func<IAsyncResult, T> callback, CancellationToken cancellationToken)
        {
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            var task = new TaskCompletionSource<T>();
            using (cancellationToken.Register(() => task.TrySetCanceled(), false))
            {
                asyncFunc(ac =>
                {
                    try
                    {
                        task.TrySetResult(callback(ac));
                    }
                    catch (Exception e)
                    {
                        task.TrySetException(e);
                    }
                }, null);
                return await task.Task;
            }
        }

        public static async Task<T> ToTask<T>([NotNull] Func<AsyncCallback, object, IAsyncResult> asyncFunc,
            [NotNull] Func<IAsyncResult, T> callback, CancellationToken cancellationToken,
            [NotNull] Action cancelingCallback)
        {
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            if (cancelingCallback == null) throw new ArgumentNullException(nameof(cancelingCallback));

            var task = new TaskCompletionSource<T>();
            using (cancellationToken.Register(() =>
            {
                cancelingCallback();
                task.TrySetCanceled();
            }, false))
            {
                asyncFunc(ac =>
                {
                    try
                    {
                        task.TrySetResult(callback(ac));
                    }
                    catch (Exception e)
                    {
                        task.TrySetException(e);
                    }
                }, null);
                return await task.Task;
            }
        }

        public static async Task<T> ToTask<T>([NotNull] Func<AsyncCallback, object, IAsyncResult> asyncFunc,
            [NotNull] Func<IAsyncResult, CancellationToken, T> callback, CancellationToken cancellationToken)
        {
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            var task = new TaskCompletionSource<T>();
            using (cancellationToken.Register(() => task.TrySetCanceled(), false))
            {
                asyncFunc(ac =>
                {
                    try
                    {
                        task.TrySetResult(callback(ac, cancellationToken));
                    }
                    catch (Exception e)
                    {
                        task.TrySetException(e);
                    }
                }, null);
                return await task.Task;
            }
        }

        public static async Task<T> ToTask<T>([NotNull] Func<AsyncCallback, object, IAsyncResult> asyncFunc,
            [NotNull] Func<IAsyncResult, CancellationToken, T> callback, CancellationToken cancellationToken,
            [NotNull] Action cancelingCallback)
        {
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            if (cancelingCallback == null) throw new ArgumentNullException(nameof(cancelingCallback));

            var task = new TaskCompletionSource<T>();
            using (cancellationToken.Register(() =>
            {
                cancelingCallback();
                task.TrySetCanceled();
            }, false))
            {
                asyncFunc(ac =>
                {
                    try
                    {
                        task.TrySetResult(callback(ac, cancellationToken));
                    }
                    catch (Exception e)
                    {
                        task.TrySetException(e);
                    }
                }, null);
                return await task.Task;
            }
        }

        public static async Task ToTask([NotNull] Func<AsyncCallback, object, IAsyncResult> asyncFunc,
            [NotNull] Action<IAsyncResult> callback)
        {
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            var task = new TaskCompletionSource<bool>();
            asyncFunc(ac =>
            {
                try
                {
                    callback(ac);
                    task.SetResult(true);
                }
                catch (Exception e)
                {
                    task.SetException(e);
                }
            }, null);
            await task.Task;
        }

        public static async Task ToTask([NotNull] Func<AsyncCallback, object, IAsyncResult> asyncFunc,
            [NotNull] Action<IAsyncResult> callback, CancellationToken cancellationToken)
        {
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            var task = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(() => task.TrySetCanceled(), false))
            {
                asyncFunc(ac =>
                {
                    try
                    {
                        callback(ac);
                        task.TrySetResult(true);
                    }
                    catch (Exception e)
                    {
                        task.TrySetException(e);
                    }
                }, null);
                await task.Task;
            }
        }

        public static async Task ToTask([NotNull] Func<AsyncCallback, object, IAsyncResult> asyncFunc,
            [NotNull] Action<IAsyncResult> callback, CancellationToken cancellationToken,
            [NotNull] Action cancelingCallback)
        {
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            if (cancelingCallback == null) throw new ArgumentNullException(nameof(cancelingCallback));

            var task = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(() =>
            {
                cancelingCallback();
                task.TrySetCanceled();
            }, false))
            {
                asyncFunc(ac =>
                {
                    try
                    {
                        callback(ac);
                        task.TrySetResult(true);
                    }
                    catch (Exception e)
                    {
                        task.TrySetException(e);
                    }
                }, null);
                await task.Task;
            }
        }

        public static async Task ToTask([NotNull] Func<AsyncCallback, object, IAsyncResult> asyncFunc,
            [NotNull] Action<IAsyncResult, CancellationToken> callback, CancellationToken cancellationToken)
        {
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            var task = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(() => task.TrySetCanceled(), false))
            {
                asyncFunc(ac =>
                {
                    try
                    {
                        callback(ac, cancellationToken);
                        task.TrySetResult(true);
                    }
                    catch (Exception e)
                    {
                        task.TrySetException(e);
                    }
                }, null);
                await task.Task;
            }
        }

        public static async Task ToTask([NotNull] Func<AsyncCallback, object, IAsyncResult> asyncFunc,
            [NotNull] Action<IAsyncResult, CancellationToken> callback, CancellationToken cancellationToken,
            [NotNull] Action cancelingCallback)
        {
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            if (cancelingCallback == null) throw new ArgumentNullException(nameof(cancelingCallback));

            var task = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(() =>
            {
                cancelingCallback();
                task.TrySetCanceled();
            }, false))
            {
                asyncFunc(ac =>
                {
                    try
                    {
                        callback(ac, cancellationToken);
                        task.TrySetResult(true);
                    }
                    catch (Exception e)
                    {
                        task.TrySetException(e);
                    }
                }, null);
                await task.Task;
            }
        }
    }
}