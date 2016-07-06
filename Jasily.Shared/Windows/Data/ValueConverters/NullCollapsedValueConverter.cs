using System;
using System.Globalization;

#if DESKTOP
using System.Windows;
using System.Windows.Data;
#elif WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#endif

namespace Jasily.Windows.Data.ValueConverters
{
    public sealed class NullCollapsedValueConverter : IValueConverter
#if DESKTOP

        , IMultiValueConverter
#endif
    {
        private Visibility Convert(object value, Type targetType, object parameter)
            => ReferenceEquals(value, null) ? Visibility.Collapsed : Visibility.Visible;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => this.Convert(value, targetType, parameter);

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            => this.Convert(values, targetType, parameter);

        public object Convert(object value, Type targetType, object parameter, string language)
            => this.Convert(value, targetType, parameter);

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => this.Convert(value, targetType, parameter);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}