
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
            Source = source;
        }

        public TSource Source
        {
            get { return _source; }
            set { SetPropertyRef(ref _source, value); }
        }
    }
}
