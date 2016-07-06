using System;
using System.Globalization;

#if DESKTOP
using System.Windows.Data;
#elif WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#endif

namespace Jasily.Windows.Data.ValueConverters
{
    public class ScaleConverter : IValueConverter
    {
        private double Convert(object value, Type targetType, object parameter)
        {
            if (targetType != typeof(double)) throw new InvalidOperationException();

            double scale = 1;
            if (parameter is double)
            {
                scale = (double)parameter;
            }
            else if (parameter is string)
            {
                double tmp;
                if (double.TryParse((string)parameter, out tmp)) scale = tmp;
            }

            if (value is double)
            {
                return (double)value * scale;
            }

            return 1;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => this.Convert(value, targetType, parameter);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;

        public object Convert(object value, Type targetType, object parameter, string language)
            => this.Convert(value, targetType, parameter);

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => null;
    }
}