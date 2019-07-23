using MagicianHub.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.ApplicationModel.Resources;

namespace MagicianHub.Verification
{
    public static class VerificationNotifyBuilder
    {
        public static ToastContent GenerateToastWrongVerifyCodeContent()
        {
            var title = ResourceLoader.GetForCurrentView()
                .GetString("VerificationNotifyTitle");

            var messageBase = ResourceLoader.GetForCurrentView()
                .GetString("Incorrect2FACode");

            return new ToastContent
            {
                Launch = $"action=wrongVerifyCode&eventId={NotificationIdBase.WrongVerifyCodeId}",
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
                            }
                        }
                    }
                },

                Actions = new ToastActionsCustom
                {
                    Buttons =
                    {
                        new ToastButtonDismiss(
                            ResourceLoader.GetForCurrentView()
                                .GetString("VerificationNotifyUnderstandMessage")
                        )
                    }
                }
            };
        }
    }
}