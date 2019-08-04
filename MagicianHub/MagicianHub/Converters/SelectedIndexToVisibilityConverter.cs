using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MagicianHub.Converters
{
    public class SelectedIndexToVisibilityConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            string language
        )
        {
            return (int)value == -1
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            string language
        ) => throw new NotImplementedException();
    }
}