using System.Collections.Generic;
using System.Enums;
using System.Runtime.CompilerServices;

namespace System.Collections.ObjectModel
{
    public class CollectionChangedEventArgs
    {
        
    }

    public class CollectionChangedEventArgs<T> : CollectionChangedEventArgs
    {
        public CollectionChangedAction Action { get; private set; }

        private CollectionChangedEventArgs(CollectionChangedAction action)
        {
            Action = action;
        }

        /// <summary>
        /// if Action != Add or Initialized or Remove, should be null.
        /// </summary>
        public IReadOnlyList<T> Items { get; private set; }

        public static CollectionChangedEventArgs<T> Reset()
        {
            return new CollectionChangedEventArgs<T>(CollectionChangedAction.Reset)
            {
                
            };
        }
        public static CollectionChangedEventArgs<T> Initialized(IEnumerable<T> source)
        {
            return new CollectionChangedEventArgs<T>(CollectionChangedAction.Initialized)
            {
                Items = new List<T>(source)
            };
        }
        public static CollectionChangedEventArgs<T> Add(T obj)
        {
            return new CollectionChangedEventArgs<T>(CollectionChangedAction.Add)
            {
                Items = new List<T>() { obj }
            };
        }
        public static CollectionChangedEventArgs<T> Add(IEnumerable<T> source)
        {
            return new CollectionChangedEventArgs<T>(CollectionChangedAction.Add)
            {
                Items = new List<T>(source)
            };
        }
        public static CollectionChangedEventArgs<T> Remove(T obj)
        {
            return new CollectionChangedEventArgs<T>(CollectionChangedAction.Remove)
            {
                Items = new List<T>() { obj }
            };
        }
        public static CollectionChangedEventArgs<T> Remove(IEnumerable<T> source)
        {
            return new CollectionChangedEventArgs<T>(CollectionChangedAction.Remove)
            {
                Items = new List<T>(source)
            };
        }
    }
}