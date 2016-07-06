using System;
using System.Globalization;
using System.Windows;

#if DESKTOP
using System.Windows.Data;
#elif WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#endif

namespace Jasily.Windows.Data.ValueConverters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        private Visibility Convert(object value, Type targetType, object parameter)
        {
            var bValue = value as bool? ?? false;
            var arg = parameter as string ?? "";
            switch (arg)
            {
#if DESKTOP
                case "01":
                    return bValue ? Visibility.Visible : Visibility.Hidden;

                case "21":
                    return bValue ? Visibility.Collapsed : Visibility.Hidden;

                case "10":
                    return bValue ? Visibility.Hidden : Visibility.Visible;

                case "12":
                    return bValue ? Visibility.Hidden : Visibility.Collapsed;

#endif

                case "02":
                default:
                    return bValue ? Visibility.Visible : Visibility.Collapsed;

                case "20":
                    return bValue ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => this.Convert(value, targetType, parameter);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value as Visibility? == Visibility.Visible;

        public object Convert(object value, Type targetType, object parameter, string language)
            => this.Convert(value, targetType, parameter);

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => value as Visibility? == Visibility.Visible;
    }
}