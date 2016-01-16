using System.Threading.Tasks;

namespace System
{
    public static class DelegateExtensions
    {
        public static async Task InvokeAsync(this Action action) => await Task.Run(() => action.Invoke());

        public static async void BeginInvoke(this Action action) => await InvokeAsync(action);

        public static async Task<T> InvokeAsync<T>(this Func<T> func) => await Task.Run(() => func.Invoke());

        public static async void BeginInvoke<T>(this Func<T> func) => await InvokeAsync(func);
    }
}