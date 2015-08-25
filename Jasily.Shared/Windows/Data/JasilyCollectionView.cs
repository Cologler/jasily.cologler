using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Jasily.ComponentModel;

namespace Jasily.Windows.Data
{
    public class JasilyCollectionView<T> : JasilyViewModel
    {
        private T selected;
        private Predicate<T> filter;

        public JasilyCollectionView()
        {
            this.Collection = new ObservableCollection<T>();
            this.View = new ListCollectionView(this.Collection);
            this.View.Filter = this.OnFilter;
        }

        public ObservableCollection<T> Collection { get; private set; }

        public ListCollectionView View { get; private set; }

        public T Selected
        {
            get { return this.selected; }
            set { this.SetPropertyRef(ref this.selected, value); }
        }

        public Predicate<T> Filter
        {
            get { return this.filter; }
            set
            {
                this.filter = value;
                this.View.Filter = value != null ? this.OnFilter : (Predicate<object>) null;
            }
        }

        private bool OnFilter(object obj)
        {
            var filter = this.Filter;
            return filter == null || filter((T) obj);
        }
    }
}
