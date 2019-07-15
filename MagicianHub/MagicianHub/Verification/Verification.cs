using MagicianHub.Github;
using Octokit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace MagicianHub.Verification
{
    public static class Verification
    {
        public static async Task<(
            VerificationResponseTypes response,
            string token
        )> DoVerificationAsync(string verifyCode)
        {
            try
            {
                var deviceInfo = new EasClientDeviceInformation();
                var currentDate = DateTimeOffset.UtcNow.ToString("s");
                var auth = await GitHubClientBase.Instance.Authorization.Create(
                    new NewAuthorization(
                        $"{GitHubClientBase.BaseClientId}{GitHubClientBase.Client2FAAction}({deviceInfo.FriendlyName}:{currentDate})",
                        new List<string>()
                    ),
                    verifyCode
                );
                return !string.IsNullOrEmpty(auth.Token)
                    ? (VerificationResponseTypes.Success, auth.Token)
                    : (VerificationResponseTypes.WrongVerifyCode, null);
            }
            catch (ApiValidationException)
            {
                return (VerificationResponseTypes.WrongVerifyCode, null);
            }
            catch (ApiException)
            {
                return (VerificationResponseTypes.UnexpectedResponse, null);
            }
        }
    }
}
