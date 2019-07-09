using Microsoft.Toolkit.Uwp.Notifications;

namespace MagicianHub.Authorization
{
    public static class AuthorizationNotifyBuilder
    {
        public static ToastContent GenerateToastWrongPasswordContent()
        {
            return new ToastContent
            {
                Launch = "action=wrongPassword&eventId=0001",
                Visual = new ToastVisual
                {
                    BindingGeneric = new ToastBindingGeneric
                    {
                        Children =
                        {
                            new AdaptiveText
                            {
                                Text = "MagicianHub - Authorization"
                            },

                            new AdaptiveText
                            {
                                Text = "GitHub account authorization failed."
                            },

                            new AdaptiveText
                            {
                                Text = "Incorrect username or password."
                            }
                        }
                    }
                },

                Actions = new ToastActionsCustom
                {
                    Buttons =
                    {
                        new ToastButton("Try reconnect", "tryReconnect"),
                        new ToastButtonDismiss("Understand")
                    }
                }
            };
        }
    }
}