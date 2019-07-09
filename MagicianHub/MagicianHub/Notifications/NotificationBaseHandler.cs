using MagicianHub.Authorization;
using Microsoft.QueryStringDotNET;

namespace MagicianHub.Notifications
{
    public static class NotificationBaseHandler
    {
        public static void ProcessNotify(QueryString args)
        {
            if (args["tryReconnect"] == "tryReconnect")
            {
                AuthorizationNotifyHandler.ProcessAuthorizationNotify(
                    AuthorizationNotifyTypes.TryReconnect
                );
            }
        }
    }
}
