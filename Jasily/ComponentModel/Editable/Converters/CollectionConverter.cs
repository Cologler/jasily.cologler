using System.Collections.Generic;

namespace Jasily.ComponentModel.Editable.Converters
{
    public class CollectionConverter<T, TElement> : CollectionConverter<T, T, TElement>
        where T : class, ICollection<TElement>, new()
    {
    }

    public class CollectionConverter<TIn, TOut, TElement> : Converter<TIn, TOut>
        where TIn : class, ICollection<TElement>, new()
        where TOut : class, ICollection<TElement>, new()
    {
        public override bool CanConvert(TIn value) => true;

        public override TOut Convert(TIn value) => value == null ? null : new TOut().AddRange2<TOut, TElement>(value);

        public override TIn ConvertBack(TOut value) => value == null ? null : new TIn().AddRange2<TIn, TElement>(value);

        public override bool CanConvertBack(TOut value) => true;
    }
}