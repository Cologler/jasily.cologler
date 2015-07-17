
namespace System.ComponentModel
{
    public class JasilyViewModel : NotifyPropertyChangedObject
    {
    }

    public class JasilyViewModel<TSource> : JasilyViewModel
    {
        private TSource source;

        public JasilyViewModel(TSource source)
        {
            this.Source = source;
        }

        [NotifyPropertyChanged(Order = -1)]
        public TSource Source
        {
            get { return this.source; }
            set { this.SetPropertyRef(ref this.source, value); }
        }
    }
}
