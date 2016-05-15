using JetBrains.Annotations;

namespace System.Threading.Tasks
{
    public static class JasilyTaskFactory
    {
        #region Task<T>

        public static async Task<T> FromAsync<T>([NotNull] Func<AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, T> endMethod)
        {
            if (beginMethod == null) throw new ArgumentNullException(nameof(beginMethod));
            if (endMethod == null) throw new ArgumentNullException(nameof(endMethod));

            var task = new TaskCompletionSource<T>();
            beginMethod(ac =>
            {
                try
                {
                    task.SetResult(endMethod(ac));
                }
                catch (Exception e)
                {
                    task.SetException(e);
                }
            }, null);
            return await task.Task;
        }

        public static async Task<T> FromAsync<T>([NotNull] Func<AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, T> endMethod,
            CancellationToken cancellationToken)
        {
            if (beginMethod == null) throw new ArgumentNullException(nameof(beginMethod));
            if (endMethod == null) throw new ArgumentNullException(nameof(endMethod));

            var task = new TaskCompletionSource<T>();
            using (cancellationToken.Register(() => task.TrySetCanceled(), false))
            {
                beginMethod(ac =>
                {
                    try
                    {
                        task.TrySetResult(endMethod(ac));
                    }
                    catch (Exception e)
                    {
                        task.TrySetException(e);
                    }
                }, null);
                return await task.Task;
            }
        }

        public static async Task<T> FromAsync<T>([NotNull] Func<AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, T> endMethod,
            CancellationToken cancellationToken, [NotNull] Action cancelingCallback)
        {
            if (beginMethod == null) throw new ArgumentNullException(nameof(beginMethod));
            if (endMethod == null) throw new ArgumentNullException(nameof(endMethod));
            if (cancelingCallback == null) throw new ArgumentNullException(nameof(cancelingCallback));

            var task = new TaskCompletionSource<T>();
            using (cancellationToken.Register(() =>
            {
                cancelingCallback();
                task.TrySetCanceled();
            }, false))
            {
                beginMethod(ac =>
                {
                    try
                    {
                        task.TrySetResult(endMethod(ac));
                    }
                    catch (Exception e)
                    {
                        task.TrySetException(e);
                    }
                }, null);
                return await task.Task;
            }
        }

        public static async Task<T> FromAsync<T>([NotNull] Func<AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, CancellationToken, T> endMethod,
            CancellationToken cancellationToken)
        {
            if (beginMethod == null) throw new ArgumentNullException(nameof(beginMethod));
            if (endMethod == null) throw new ArgumentNullException(nameof(endMethod));

            var task = new TaskCompletionSource<T>();
            using (cancellationToken.Register(() => task.TrySetCanceled(), false))
            {
                beginMethod(ac =>
                {
                    try
                    {
                        task.TrySetResult(endMethod(ac, cancellationToken));
                    }
                    catch (Exception e)
                    {
                        task.TrySetException(e);
                    }
                }, null);
                return await task.Task;
            }
        }

        public static async Task<T> FromAsync<T>([NotNull] Func<AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Func<IAsyncResult, CancellationToken, T> endMethod,
            CancellationToken cancellationToken, [NotNull] Action cancelingCallback)
        {
            if (beginMethod == null) throw new ArgumentNullException(nameof(beginMethod));
            if (endMethod == null) throw new ArgumentNullException(nameof(endMethod));
            if (cancelingCallback == null) throw new ArgumentNullException(nameof(cancelingCallback));

            var task = new TaskCompletionSource<T>();
            using (cancellationToken.Register(() =>
            {
                cancelingCallback();
                task.TrySetCanceled();
            }, false))
            {
                beginMethod(ac =>
                {
                    try
                    {
                        task.TrySetResult(endMethod(ac, cancellationToken));
                    }
                    catch (Exception e)
                    {
                        task.TrySetException(e);
                    }
                }, null);
                return await task.Task;
            }
        }

        #endregion

        #region Task

        public static async Task FromAsync([NotNull] Func<AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Action<IAsyncResult> endMethod)
        {
            if (beginMethod == null) throw new ArgumentNullException(nameof(beginMethod));
            if (endMethod == null) throw new ArgumentNullException(nameof(endMethod));

            var task = new TaskCompletionSource<bool>();
            beginMethod(ac =>
            {
                try
                {
                    endMethod(ac);
                    task.SetResult(true);
                }
                catch (Exception e)
                {
                    task.SetException(e);
                }
            }, null);
            await task.Task;
        }

        public static async Task FromAsync([NotNull] Func<AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Action<IAsyncResult> endMethod, CancellationToken cancellationToken)
        {
            if (beginMethod == null) throw new ArgumentNullException(nameof(beginMethod));
            if (endMethod == null) throw new ArgumentNullException(nameof(endMethod));

            var task = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(() => task.TrySetCanceled(), false))
            {
                beginMethod(ac =>
                {
                    try
                    {
                        endMethod(ac);
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

        public static async Task FromAsync([NotNull] Func<AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Action<IAsyncResult> endMethod, CancellationToken cancellationToken,
            [NotNull] Action cancelingCallback)
        {
            if (beginMethod == null) throw new ArgumentNullException(nameof(beginMethod));
            if (endMethod == null) throw new ArgumentNullException(nameof(endMethod));
            if (cancelingCallback == null) throw new ArgumentNullException(nameof(cancelingCallback));

            var task = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(() =>
            {
                cancelingCallback();
                task.TrySetCanceled();
            }, false))
            {
                beginMethod(ac =>
                {
                    try
                    {
                        endMethod(ac);
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

        public static async Task FromAsync([NotNull] Func<AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Action<IAsyncResult, CancellationToken> endMethod, CancellationToken cancellationToken)
        {
            if (beginMethod == null) throw new ArgumentNullException(nameof(beginMethod));
            if (endMethod == null) throw new ArgumentNullException(nameof(endMethod));

            var task = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(() => task.TrySetCanceled(), false))
            {
                beginMethod(ac =>
                {
                    try
                    {
                        endMethod(ac, cancellationToken);
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

        public static async Task FromAsync([NotNull] Func<AsyncCallback, object, IAsyncResult> beginMethod,
            [NotNull] Action<IAsyncResult, CancellationToken> endMethod, CancellationToken cancellationToken,
            [NotNull] Action cancelingCallback)
        {
            if (beginMethod == null) throw new ArgumentNullException(nameof(beginMethod));
            if (endMethod == null) throw new ArgumentNullException(nameof(endMethod));
            if (cancelingCallback == null) throw new ArgumentNullException(nameof(cancelingCallback));

            var task = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(() =>
            {
                cancelingCallback();
                task.TrySetCanceled();
            }, false))
            {
                beginMethod(ac =>
                {
                    try
                    {
                        endMethod(ac, cancellationToken);
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

        #endregion
    }
}