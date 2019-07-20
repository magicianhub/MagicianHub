using Microsoft.Toolkit.Uwp.Notifications;

namespace MagicianHub.Authorization
{
    public static class AuthorizationNotifyBuilder
    {
        public static ToastContent GenerateToastWrongPasswordContent(
            string login = "",
            string password = "",
            string token = "",
            bool isUseToken = false)
        {
            var arguments = isUseToken 
                ? $"tryReconnect:auth?token={token}&login={login}" 
                : $"tryReconnect:auth?login={login}&pass={password}";

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
                        new ToastButton("Try reconnect", arguments),
                        new ToastButtonDismiss("Understand")
                    }
                }
            };
        }
    }
}