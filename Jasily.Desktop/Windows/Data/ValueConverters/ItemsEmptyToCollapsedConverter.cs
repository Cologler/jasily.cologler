using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Jasily.Windows.Data.ValueConverters
{
    public class ItemsEmptyToCollapsedConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = value as IList;
            var enumerable = value as IEnumerable;

            if (list == null && enumerable == null)
            {
                var itemControl = value as ItemsControl;
                if (itemControl?.ItemsSource != null)
                {
                    list = itemControl.ItemsSource as IList;
                    enumerable = itemControl.ItemsSource;
                }
            }

            if (list != null)
            {
                return list.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
            }

            if (enumerable != null)
            {
                return enumerable.Cast<object>().Any() ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}