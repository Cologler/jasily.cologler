
namespace System.ComponentModel
{
    public class JasilyViewModel : NotifyPropertyChangedObject
    {
    }

    public class JasilyViewModel<TSource> : JasilyViewModel
    {
        public JasilyViewModel(TSource source)
        {
            Source = source;
        }

        public TSource Source { get; private set; }
    }
}
