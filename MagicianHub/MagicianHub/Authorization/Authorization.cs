using MagicianHub.Github;
using MagicianHub.Verification;
using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (useAccessToken)
            {
                var tokenAuth = new Credentials(accessToken);
                GitHubClientBase.Instance.Credentials = tokenAuth;
                try
                {
                    await GitHubClientBase.Instance.User.Current();
                    return AuthorizationResponseTypes.Success;
                }
                catch (AuthorizationException)
                {
                    return AuthorizationResponseTypes.WrongCredentials;
                }
                catch (Exception)
                {
                    return AuthorizationResponseTypes.UnexpectedResponse;
                }
            }
            else
            {
                var basicAuth = new Credentials(login, password);
                GitHubClientBase.Instance.Credentials = basicAuth;
                try
                {
                    await GitHubClientBase.Instance.User.Current();
                    return AuthorizationResponseTypes.Success;
                }
                catch (TwoFactorRequiredException ex)
                {
                    return ex.TwoFactorType == TwoFactorType.AuthenticatorApp
                        ? AuthorizationResponseTypes.NeedVerifyCodeByApp
                        : AuthorizationResponseTypes.NeedVerifyCodeByPhone;
                }
                catch (AuthorizationException)
                {
                    return AuthorizationResponseTypes.WrongCredentials;
                }
                catch (Exception)
                {
                    return AuthorizationResponseTypes.UnexpectedResponse;
                }
            }
        }

        public static async Task<string> DoVerification(
            string verifyCode)
        {
            try
            {
                var auth = await GitHubClientBase.Instance.Authorization.Create(
                    new NewAuthorization("MagicianHub@Verify", new List<string>()),
                    verifyCode
                );
                Debug.WriteLine(auth.Token);
                return auth.Token;
            }
            catch (ApiValidationException ex)
            {
                Debug.WriteLine(ex.ApiError);
                Debug.WriteLine(ex.HelpLink);
                Debug.WriteLine(ex.Message);
                return "Exception";
            }
        }

        public static void SendVerificationCode(
            VerificationRequestTypes verificationRequestType)
        {
            
        }
    }
}
