namespace Jasily.ComponentModel.Editable.Converters
{
    public sealed class TrimStringConverter : ToStringConverter<string>
    {
        public override string ConvertBack(string value) => value?.Trim();
    }
}