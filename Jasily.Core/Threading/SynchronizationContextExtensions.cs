using JetBrains.Annotations;

namespace System.Threading
{
    public static class SynchronizationContextExtensions
    {
        public static void Send([NotNull] this SynchronizationContext context, [NotNull] Action action)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (action == null) throw new ArgumentNullException(nameof(action));

            context.Send(_ => action(), null);
        }

        public static void Send<T>([NotNull] this SynchronizationContext context, [NotNull] Action<T> action, T state)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (action == null) throw new ArgumentNullException(nameof(action));

            context.Send(_ => action(state), null);
        }

        public static void Post([NotNull] this SynchronizationContext context, [NotNull] Action action)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (action == null) throw new ArgumentNullException(nameof(action));

            context.Post(_ => action(), null);
        }

        public static void Post<T>([NotNull] this SynchronizationContext context, [NotNull] Action<T> action, T state)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (action == null) throw new ArgumentNullException(nameof(action));

            context.Post(_ => action(state), null);
        }
    }
}