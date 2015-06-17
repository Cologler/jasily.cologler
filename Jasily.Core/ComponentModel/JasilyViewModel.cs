
namespace System.ComponentModel
{
    public class JasilyViewModel : NotifyPropertyChangedObject
    {
    }

    public class JasilyViewModel<TSource> : JasilyViewModel
    {
        private TSource _source;

        public JasilyViewModel(TSource source)
        {
            this.Source = source;
        }

        public TSource Source
        {
            get { return this._source; }
            set { this.SetPropertyRef(ref this._source, value); }
        }
    }
}
