using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace System.Collections.ObjectModel
{
    public class ObservableCollectionGroup<TKey, TElement> : ObservableCollection<TElement>, IGrouping<TKey, TElement>
    {
        TKey _key;

        public ObservableCollectionGroup(TKey key)
            : base()
        {
            this._key = key;
        }
        public ObservableCollectionGroup(TKey key, IEnumerable<TElement> collection)
            : base(collection)
        {
            this._key = key;
        }
        public ObservableCollectionGroup(TKey key, List<TElement> list)
            : base(list)
        {
            this._key = key;
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
            get { return this._key; }
            set
            {
                if (!this._key.Equals(value))
                {
                    this._key = value;
                    this.OnPropertyChanged(new PropertyChangedEventArgs("Key"));
                }
            }
        }
    }
}
