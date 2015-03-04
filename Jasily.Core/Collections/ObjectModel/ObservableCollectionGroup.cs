using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace System.Collections.ObjectModel
{
    public class ObservableCollectionGroup<TKey, TElement> : ObservableCollection<TElement>, IGrouping<TKey, TElement>
    {
        TKey _key;

        public ObservableCollectionGroup(TKey key)
            : base()
        {
            _key = key;
        }
        public ObservableCollectionGroup(TKey key, IEnumerable<TElement> collection)
            : base(collection)
        {
            _key = key;
        }
        public ObservableCollectionGroup(TKey key, List<TElement> list)
            : base(list)
        {
            _key = key;
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
            get { return _key; }
            set
            {
                if (!_key.Equals(value))
                {
                    _key = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Key"));
                }
            }
        }
    }
}
