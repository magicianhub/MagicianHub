using MagicianHub.Extensions;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using Windows.UI.Xaml;

namespace MagicianHub.Verification
{
    public class VerificationNotify
    {
        public static void NotifyWrongVerifyCode()
        {
            if (Window.Current.CoreWindow.IsActive()) return;
            ToastContent content =
                VerificationNotifyBuilder.GenerateToastWrongVerifyCodeContent();
            ToastNotificationManager
                .CreateToastNotifier()
                .Show(new ToastNotification(content.GetXml()));
        }
    }
}