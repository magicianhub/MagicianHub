using MagicianHub.Verification;
using MagicianHub.Views;
using System;
using System.Threading.Tasks;

namespace MagicianHub.Authorization
{
    public static class Authorization
    {
        public static async Task<AuthorizationResponseTypes> DoAuthorizationAsync(
            string login,
            string password,
            string accessToken,
            bool useAccessToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            return AuthorizationResponseTypes.NeedVerifyCode;
            await new TwoFactorAuthModeDialog().ShowAsync();
        }

        public static async Task<VerificationResponseTypes> DoVerification(
            string verifyCode)
        {
            return VerificationResponseTypes.Success;
        }

        public static void SendVerificationCode()
        {
            
        }
    }
}
