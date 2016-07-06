using System;

#if DESKTOP
using System.Windows;
using System.Windows.Data;
#elif WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#endif

namespace Jasily.Windows.Data.ValueConverters
{
    public sealed class EmptyCollapsedValueConverter : IValueConverter
    {
        private Visibility Convert(object value, Type targetType, object parameter)
        {
            if (ReferenceEquals(value, null))
                return Visibility.Collapsed;

            var str = parameter as string;
            return str == "IsNullOrWhiteSpace"
                ? ((value as string).IsNullOrWhiteSpace() ? Visibility.Collapsed : Visibility.Visible)
                : ((value as string).IsNullOrEmpty() ? Visibility.Collapsed : Visibility.Visible);
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => this.Convert(value, targetType, parameter);

        public object Convert(object value, Type targetType, object parameter, string language)
            => this.Convert(value, targetType, parameter);

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}