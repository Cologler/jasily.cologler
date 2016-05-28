using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Jasily.Windows.Data.ValueConverters
{
    public class BooleanToVisibilityConverter2 : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bValue = value as bool? ?? false;
            var arg = parameter as string ?? "";
            switch (arg)
            {
                case "01":
                    return bValue ? Visibility.Visible : Visibility.Hidden;

                case "02":
                default:
                    return bValue ? Visibility.Visible : Visibility.Collapsed;

                case "10":
                    return bValue ? Visibility.Hidden : Visibility.Visible;

                case "12":
                    return bValue ? Visibility.Hidden : Visibility.Collapsed;

                case "20":
                    return bValue ? Visibility.Collapsed : Visibility.Visible;

                case "21":
                    return bValue ? Visibility.Collapsed : Visibility.Hidden;
            }
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as Visibility? == Visibility.Visible;
        }

        #endregion
    }
}