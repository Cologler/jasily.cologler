namespace Jasily.ComponentModel.Editable
{
    public sealed class WriteToObjectIfNotNullOrWhiteSpaceAttribute : WriteToObjectConditionAttribute
    {
        public override bool IsMatch(object value) => !string.IsNullOrWhiteSpace(value as string);
    }
}