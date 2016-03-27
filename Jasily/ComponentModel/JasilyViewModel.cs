
using JetBrains.Annotations;
using System;

namespace Jasily.ComponentModel
{
    public class JasilyViewModel : NotifyPropertyChangedObject
    {
    }

    public class JasilyViewModel<TSource> : JasilyViewModel
    {
        private TSource source;

        public JasilyViewModel(TSource source)
        {
            this.source = source;
        }

        [NotifyPropertyChanged(Order = -1)]
        public TSource Source
        {
            get { return this.source; }
            set { this.SetPropertyRef(ref this.source, value); }
        }

        public static implicit operator TSource([NotNull] JasilyViewModel<TSource> value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return value.Source;
        }
    }
}
