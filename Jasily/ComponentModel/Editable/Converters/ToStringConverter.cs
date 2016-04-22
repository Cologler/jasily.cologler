namespace Jasily.ComponentModel.Editable.Converters
{
    public abstract class ToStringConverter<T> : Converter<T, string>
    {
        #region Overrides of Converter<T,string>

        public override bool CanConvert(T value) => true;

        public override bool CanConvertBack(string value) => true;

        public override string Convert(T value) => value?.ToString();

        #endregion
    }
}