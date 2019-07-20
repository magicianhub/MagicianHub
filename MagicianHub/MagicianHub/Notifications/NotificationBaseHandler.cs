using MagicianHub.Authorization;
using Microsoft.QueryStringDotNET;
using Windows.UI.Xaml;

namespace MagicianHub.Notifications
{
    public static class NotificationBaseHandler
    {
        public static void ProcessNotify(QueryString args)
        {
            if (args == null)
            {
                Window.Current.Activate();
                return;
            }
            if (args.ToString().Contains("tryReconnect"))
            {
                AuthorizationNotifyHandler.ProcessAuthorizationNotify(
                    AuthorizationNotifyTypes.TryReconnect,
                    args
                );
                return;
            }
        }
    }
}
