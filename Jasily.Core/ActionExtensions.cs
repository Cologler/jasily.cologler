using System.Threading.Tasks;

namespace System
{
    public static class ActionExtensions
    {
        public static async void BeginFire(this Action action)
        {
            await action.FireAsync();
        }

        public static async Task FireAsync(this Action action)
        {
            if (action != null)
            {
                await Task.Run(action);
            }
        }

        public static async void BeginFire<T>(this Action<T> action, T t)
        {
            await action.FireAsync(t);
        }

        public static async Task FireAsync<T>(this Action<T> action, T t)
        {
            if (action != null)
            {
                await Task.Run(() => action(t));
            }
        }
    }
}