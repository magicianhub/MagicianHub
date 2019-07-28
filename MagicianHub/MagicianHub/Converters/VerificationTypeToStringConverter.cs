using MagicianHub.Verification;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace MagicianHub.Converters
{
    public class VerificationTypeToStringConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            string language
        )
        {
            return ResourceLoader.GetForCurrentView().GetString(
                (VerificationRequestTypes)value == VerificationRequestTypes.Application
                    ? "VerifyAppText"
                    : "VerifySmsText"
            );
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            string language
        ) =>
            throw new NotImplementedException();
    }
}
