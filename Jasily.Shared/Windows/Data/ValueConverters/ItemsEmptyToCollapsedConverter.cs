using System;
using System.Collections;
using System.Globalization;
using System.Linq;

#if DESKTOP
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
#elif WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
#endif

namespace Jasily.Windows.Data.ValueConverters
{
    public class ItemsEmptyToCollapsedConverter : IValueConverter
    {
        private Visibility Convert(object value, Type targetType, object parameter)
        {
            var list = value as IList;
            var enumerable = value as IEnumerable;

            if (list == null && enumerable == null)
            {
                var itemControl = value as ItemsControl;
                if (itemControl?.ItemsSource != null)
                {
                    list = itemControl.ItemsSource as IList;
                    enumerable = itemControl.ItemsSource as IEnumerable;
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