namespace Jasily.ComponentModel.Editable
{
    public sealed class WriteToObjectIfNotNullOrEmptyAttribute : WriteToObjectConditionAttribute
    {
        public override bool IsMatch(object value) => !string.IsNullOrEmpty(value as string);
    }
}