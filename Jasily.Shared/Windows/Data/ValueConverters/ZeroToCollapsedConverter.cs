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
    public sealed class ZeroToCollapsedConverter : IValueConverter
    {
        private Visibility Convert(object value, Type targetType, object parameter)
            => value?.ToString() == "0" ? Visibility.Collapsed : Visibility.Visible;

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