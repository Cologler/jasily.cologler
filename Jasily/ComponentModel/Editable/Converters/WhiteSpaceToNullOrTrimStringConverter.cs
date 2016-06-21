namespace Jasily.ComponentModel.Editable.Converters
{
    public sealed class WhiteSpaceToNullOrTrimStringConverter : ToStringConverter<string>
    {
        public override string ConvertBack(string value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}