using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace System.Collections.ObjectModel
{
    public class ObservableCollectionGroup<TKey, TElement> : ObservableCollection<TElement>, IGrouping<TKey, TElement>
    {
        private TKey key;

        public ObservableCollectionGroup(TKey key)
            : base()
        {
            this.key = key;
        }
        public ObservableCollectionGroup(TKey key, IEnumerable<TElement> collection)
            : base(collection)
        {
            this.key = key;
        }
        public ObservableCollectionGroup(TKey key, List<TElement> list)
            : base(list)
        {
            this.key = key;
        }
        public ObservableCollectionGroup()
            : base()
        {
        }
        public ObservableCollectionGroup(IEnumerable<TElement> collection)
            : base(collection)
        {
        }
        public ObservableCollectionGroup(List<TElement> list)
            : base(list)
        {
        }

        public TKey Key
        {
            get { return this.key; }
            set
            {
                if (!this.key.NormalEquals(value))
                {
                    this.key = value;
                    this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.Key)));
                }
            }
        }
    }
}
