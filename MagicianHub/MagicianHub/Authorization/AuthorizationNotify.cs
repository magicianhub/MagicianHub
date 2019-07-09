using MagicianHub.Extensions;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using Windows.UI.Xaml;

namespace MagicianHub.Authorization
{
    public static class AuthorizationNotify
    {
        public static void NotifyWrongPassword()
        {
            if (Window.Current.CoreWindow.IsActive()) return;
            ToastContent content = AuthorizationNotifyBuilder.GenerateToastWrongPasswordContent();
            ToastNotificationManager
                .CreateToastNotifier()
                .Show(new ToastNotification(content.GetXml()));
        }
    }
}
