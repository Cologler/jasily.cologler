using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace System.Windows.Data
{
    public class JasilySelectorCollectionView<T> : JasilyCollectionView<T>
    {
        public JasilySelectorCollectionView()
        {
            this.SelectedItems = new ObservableCollection<T>();
            this.SelectedItems.CollectionChanged += this.SelectedItems_CollectionChanged;
            this.Filter = this.OnFilter;
        }

        void SelectedItems_CollectionChanged(object sender, Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    this.View.Refresh();
                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ObservableCollection<T> SelectedItems { get; private set; }

        protected virtual bool OnFilter(T obj)
        {
            return !this.SelectedItems.Contains(obj);
        }
    }
}