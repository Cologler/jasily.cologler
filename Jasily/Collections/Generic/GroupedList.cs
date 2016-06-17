using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Jasily.Collections.Generic
{
    public static class GroupedList
    {
        public static GroupedList<TKey, TElement> ToList<TKey, TElement>([NotNull] this IGrouping<TKey, TElement> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new GroupedList<TKey, TElement>(source);
        }
    }

    public class GroupedList<TKey, TElement> : List<TElement>, IGrouping<TKey, TElement>
    {
        public GroupedList() { }

        public GroupedList(TKey key)
        {
            this.Key = key;
        }

        public GroupedList(IGrouping<TKey, TElement> grouping)
            : base(grouping)
        {
            this.Key = grouping.Key;
        }

        public TKey Key { get; set; }
    }
}