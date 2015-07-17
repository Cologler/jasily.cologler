using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace System.Windows.Data
{
    public sealed class NullCollapsedValueConverter : IValueConverter, IMultiValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, Globalization.CultureInfo culture)
        {
            return ReferenceEquals(value, null) ? Visibility.Collapsed : Visibility.Visible;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, Globalization.CultureInfo culture)
        {
            return ReferenceEquals(values, null) ? Visibility.Collapsed : Visibility.Visible;
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}