namespace Jasily
{
    public abstract class Converter<TIn, TOut> : IConverter<TIn, TOut>
    {
        public abstract bool CanConvert(TIn value);

        public abstract bool CanConvertBack(TOut value);

        public abstract TOut Convert(TIn value);

        public abstract TIn ConvertBack(TOut value);

        #region non generic type

        public bool CanConvert(object value) => this.CanConvert((TIn)value);

        public bool CanConvertBack(object value) => this.CanConvertBack((TOut)value);

        public object Convert(object value) => this.Convert((TIn)value);

        public object ConvertBack(object value) => this.ConvertBack((TOut)value);

        #endregion
    }
}