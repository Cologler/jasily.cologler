using System.Collections.Generic;
using Windows.UI.Xaml.Data;

namespace Jasily.Collections.ObjectModel
{
    public struct IncrementalLoadingResult<T>
    {
        public LoadMoreItemsResult Result { get; }
        public IEnumerable<T> Items { get; }
        public bool HasMoreItems { get; }

        public IncrementalLoadingResult(uint loadedCount, IEnumerable<T> items, bool hasMoreItems)
        {
            this.Result = new LoadMoreItemsResult() { Count = loadedCount };
            this.Items = items;
            this.HasMoreItems = hasMoreItems;
        }
    }
}