using MagicianHub.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.ApplicationModel.Resources;

namespace MagicianHub.Authorization
{
    public static class AuthorizationNotifyBuilder
    {
        public static ToastContent GenerateToastWrongPasswordContent(
            string login = "",
            string password = "",
            string token = "",
            bool isUseToken = false
        )
        {
            var arguments = isUseToken 
                ? $"tryReconnect:auth?token={token}&login={login}" 
                : $"tryReconnect:auth?login={login}&pass={password}";

            var launchString = isUseToken
                ? $"action=wrongToken&eventId={NotificationIdBase.WrongTokenId}"
                : $"action=wrongPassword&eventId={NotificationIdBase.WrongPasswordId}";

            var title = ResourceLoader.GetForCurrentView()
                .GetString("AuthorizationNotifyTitle");

            var messageBase = ResourceLoader.GetForCurrentView()
                .GetString("AuthorizationNotifyBaseMessage");
            
            var messageDescription = isUseToken
                ? ResourceLoader.GetForCurrentView().GetString("IncorrectToken")
                : ResourceLoader.GetForCurrentView().GetString("IncorrectPassword");

            return new ToastContent
            {
                Launch = launchString,
                Visual = new ToastVisual
                {
                    BindingGeneric = new ToastBindingGeneric
                    {
                        Children =
                        {
                            new AdaptiveText
                            {
                                Text = title
                            },

                            new AdaptiveText
                            {
                                Text = messageBase
                            },

                            new AdaptiveText
                            {
                                Text = messageDescription
                            }
                        }
                    }
                },

                Actions = new ToastActionsCustom
                {
                    Buttons =
                    {
                        new ToastButton(
                            ResourceLoader.GetForCurrentView()
                                .GetString("AuthorizationNotifyTryReconnectMessage"),
                            arguments
                        ),
                        new ToastButtonDismiss(
                            ResourceLoader.GetForCurrentView()
                                .GetString("AuthorizationNotifyUnderstandMessage")
                        )
                    }
                }
            };
        }
    }
}