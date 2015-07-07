using System.Collections.ObjectModel;
using System.ComponentModel;

namespace System.Windows.Data
{
    public class JasilyCollectionView<T> : JasilyViewModel
    {
        private T selected;

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

        public Predicate<T> Filter { get; set; }

        private bool OnFilter(object obj)
        {
            var filter = this.Filter;
            return filter == null || filter((T) obj);
        }
    }
}
