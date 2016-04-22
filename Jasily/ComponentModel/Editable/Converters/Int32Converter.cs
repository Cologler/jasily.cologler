namespace Jasily.ComponentModel.Editable.Converters
{
    public class Int32Converter : ToStringConverter<int>
    {
        #region Overrides of Converter<int,string>

        public override int ConvertBack(string value) => int.Parse(value);

        #endregion
    }
}