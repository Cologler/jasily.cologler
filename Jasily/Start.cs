using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily
{
    public static class Start
    {
        public static void Retry([NotNull] Action action, uint time, bool aggregateError = false)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (time <= 0) throw new ArgumentOutOfRangeException(nameof(time));

            List<Exception> errors = null;
            do
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception e)
                {
                    if (time == 0 && !aggregateError)
                    {
                        if (errors == null) errors = new List<Exception>();
                        errors.Add(e);
                    }
                    else
                    {
                        if (time == 1)
                        {
                            throw;
                        }
                    }
                }
            } while (--time > 0);
            throw new AggregateException(errors);
        }

        public static T Retry<T>([NotNull] Func<T> action, uint time, bool aggregateError = false)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (time <= 0) throw new ArgumentOutOfRangeException(nameof(time));

            List<Exception> errors = null;
            do
            {
                try
                {
                    return action();
                }
                catch (Exception e)
                {
                    if (time == 0 && !aggregateError)
                    {
                        if (errors == null) errors = new List<Exception>();
                        errors.Add(e);
                    }
                    else
                    {
                        if (time == 1)
                        {
                            throw;
                        }
                    }
                }
            } while (--time > 0);
            throw new AggregateException(errors);
        }

        public static async Task RetryAsync([NotNull] Task task, uint time, bool aggregateError = false)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (time <= 0) throw new ArgumentOutOfRangeException(nameof(time));

            List<Exception> errors = null;
            do
            {
                try
                {
                    await task;
                    return;
                }
                catch (Exception e)
                {
                    if (time == 0 && !aggregateError)
                    {
                        if (errors == null) errors = new List<Exception>();
                        errors.Add(e);
                    }
                    else
                    {
                        if (time == 1)
                        {
                            throw;
                        }
                    }
                }
            } while (--time > 0);
            throw new AggregateException(errors);
        }

        public static async Task<T> RetryAsync<T>([NotNull] Task<T> task, uint time, bool aggregateError = false)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (time <= 0) throw new ArgumentOutOfRangeException(nameof(time));

            List<Exception> errors = null;
            do
            {
                try
                {
                    return await task;
                }
                catch (Exception e)
                {
                    if (time == 0 && !aggregateError)
                    {
                        if (errors == null) errors = new List<Exception>();
                        errors.Add(e);
                    }
                    else
                    {
                        if (time == 1)
                        {
                            throw;
                        }
                    }
                }
            } while (--time > 0);
            throw new AggregateException(errors);
        }
    }
}