using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace System.Collections.ObjectModel
{
    public class CollectionChangedEventArgs<T>
    {
        public CollectionChangedAction Action { get; private set; }

        private CollectionChangedEventArgs(CollectionChangedAction action)
        {
            this.Action = action;
            this.Items = Empty<T>.Array;
        }

        private CollectionChangedEventArgs(CollectionChangedAction action, IEnumerable<T> source)
        {
            this.Action = action;
            this.Items = source.ToList().AsReadOnly();
        }
        
        [NotNull]
        public IReadOnlyList<T> Items { get; }

        public static CollectionChangedEventArgs<T> Reset()
            => new CollectionChangedEventArgs<T>(CollectionChangedAction.Reset);

        public static CollectionChangedEventArgs<T> Initialized([NotNull] IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new CollectionChangedEventArgs<T>(CollectionChangedAction.Initialized, source);
        }

        public static CollectionChangedEventArgs<T> Add(T obj)
            => new CollectionChangedEventArgs<T>(CollectionChangedAction.Add, new[] { obj });

        public static CollectionChangedEventArgs<T> Add([NotNull] IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new CollectionChangedEventArgs<T>(CollectionChangedAction.Add, source);
        }

        public static CollectionChangedEventArgs<T> Remove(T obj)
            => new CollectionChangedEventArgs<T>(CollectionChangedAction.Remove, new[] { obj });

        public static CollectionChangedEventArgs<T> Remove([NotNull] IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new CollectionChangedEventArgs<T>(CollectionChangedAction.Remove, source);
        }
    }
}