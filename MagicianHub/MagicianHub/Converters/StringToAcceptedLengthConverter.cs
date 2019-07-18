using System;
using Windows.UI.Xaml.Data;

namespace MagicianHub.Converters
{
    public class StringToAcceptedLengthConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            string language)
        {
            var text = value as string;
            var acceptedLength = int.Parse(parameter as string);
            return text?.Length == acceptedLength;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            string language) =>
            throw new NotImplementedException();
    }
}
