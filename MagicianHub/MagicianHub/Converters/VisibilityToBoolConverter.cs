using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MagicianHub.Converters
{
    public class VisibilityToBoolConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            string language
        )
        {
            if (!(value is Visibility visibility)) return false;
            return visibility == Visibility.Visible
                ? !(bool.TryParse(parameter.ToString(), out var outFalse) && outFalse)
                : bool.TryParse(parameter.ToString(), out var outTrue) && outTrue;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            string language
        )
        {
            return value is bool bl && bl
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}