namespace System.Windows.Data
{
    public sealed class EmptyCollapsedValueConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, Globalization.CultureInfo culture)
        {
            if (ReferenceEquals(value, null))
                return Visibility.Collapsed;

            var str = parameter as string;
            return str == "IsNullOrWhiteSpace"
                ? ((value as string).IsNullOrWhiteSpace() ? Visibility.Collapsed : Visibility.Visible)
                : ((value as string).IsNullOrEmpty() ? Visibility.Collapsed : Visibility.Visible);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}