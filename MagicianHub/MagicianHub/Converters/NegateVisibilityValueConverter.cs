using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MagicianHub.Converters
{
    public class NegateVisibilityValueConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            string language)
        {
            var visibility = (Visibility) value;
            return visibility == Visibility.Visible 
                ? Visibility.Collapsed 
                : Visibility.Visible;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            string language)
        {
            var visibility = (Visibility)value;
            return visibility == Visibility.Collapsed
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}
