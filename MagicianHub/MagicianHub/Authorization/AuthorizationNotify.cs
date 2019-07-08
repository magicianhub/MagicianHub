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
            ToastContent content = GenerateToastContent();
            ToastNotificationManager
                .CreateToastNotifier()
                .Show(new ToastNotification(content.GetXml()));
        }

        private static ToastContent GenerateToastContent()
        {
            return new ToastContent()
            {
                Scenario = ToastScenario.Default,
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "MagicianHub - Log in"
                            },

                            new AdaptiveText()
                            {
                                Text = "Authorization failed, ERR: 430"
                            },

                            new AdaptiveText()
                            {
                                Text = "Maybe: wrong password or login"
                            }
                        }
                    }
                },

                Actions = new ToastActionsCustom()
                {
                    Buttons =
                    {
                        new ToastButton("Try reconnect", ""),
                        new ToastButton("Understand", "")
                    }
                }
            };
        }
    }
}