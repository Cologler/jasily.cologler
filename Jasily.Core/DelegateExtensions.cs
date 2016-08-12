using System.Threading.Tasks;
using JetBrains.Annotations;

namespace System
{
    public static class DelegateExtensions
    {
        public static async Task InvokeAsync([NotNull] this Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            await Task.Run(() => action.Invoke());
        }

        public static async void BeginInvoke([NotNull] this Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            await InvokeAsync(action);
        }

        public static async Task<T> InvokeAsync<T>([NotNull] this Func<T> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            return await Task.Run(() => func.Invoke());
        }

        public static async void BeginInvoke<T>([NotNull] this Func<T> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            await InvokeAsync(func);
        }
    }
}