using System.Collections.Generic;

namespace Jasily.ComponentModel.Editable.Converters
{
    public class EmptyToNullCollectionConverter<TIn, TOut, TElement> : CollectionConverter<TIn, TOut, TElement>
        where TIn : class, ICollection<TElement>, new()
        where TOut : class, ICollection<TElement>, new()
    {
        public override TIn ConvertBack(TOut value)
            => value == null || value.Count == 0 ? null : new TIn().AddRange2<TIn, TElement>(value);
    }
}