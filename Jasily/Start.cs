using System;
using System.Collections.Generic;
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
            while (time-- > 0)
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
                        if (time == 0)
                        {
                            throw;
                        }
                    }
                }
            }
            throw new AggregateException(errors);
        }

        public static T Retry<T>([NotNull] Func<T> action, uint time, bool aggregateError = false)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (time <= 0) throw new ArgumentOutOfRangeException(nameof(time));

            List<Exception> errors = null;
            while (time-- > 0)
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
                        if (time == 0)
                        {
                            throw;
                        }
                    }
                }
            }
            throw new AggregateException(errors);
        }
    }
}