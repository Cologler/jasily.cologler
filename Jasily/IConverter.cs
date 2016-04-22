namespace Jasily
{
    public interface IConverter<TIn, TOut> : IConverter
    {
        bool CanConvert(TIn value);

        TOut Convert(TIn value);

        bool CanConvertBack(TOut value);

        TIn ConvertBack(TOut value);
    }

    public interface IConverter
    {
        bool CanConvert(object value);

        object Convert(object value);

        bool CanConvertBack(object value);

        object ConvertBack(object value);
    }
}