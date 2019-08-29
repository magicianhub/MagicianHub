using MagicianHub.Extensions;
using MagicianHub.Github;
using Octokit;
using System;
using System.Threading.Tasks;
using Windows.System;
using static MagicianHub.Logger.Logger;

namespace MagicianHub.Authorization
{
    public static class Authorization
    {
        public static async Task<AuthorizationResponseTypes> DoAuthorizationAsync(
            string login,
            string password,
            string accessToken,
            bool useAccessToken
        )
        {
            await Task.Delay(AuthorizationRequestDelay.CalculateDelay());
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
                    return AuthorizationResponseTypes.WrongAccessToken;
                }
                catch (ApiException)
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
                catch (ApiException)
                {
                    return AuthorizationResponseTypes.UnexpectedResponse;
                }
            }
        }

        public static async Task DoAuthorizationViaBrowserAsync()
        {
            var request = new OauthLoginRequest(GitHubClientBase.ClientId);
            GitHubClientBase.Scopes.ForEach(request.Scopes.Add);

            var oauthLoginUrl = GitHubClientBase.Instance.Oauth.GetGitHubLoginUrl(request);
            var result = await Launcher.LaunchUriAsync(oauthLoginUrl);
            if (!result)
            {
                Log.Error($"An error occurred while opening oauth uri ({oauthLoginUrl})");
            }
        }

        public static async Task<string> ProcessBrowserCodeResponseAsync(string code)
        {
            var tokenRequest = new OauthTokenRequest(
                GitHubClientBase.ClientId,
                GitHubClientBase.ClientSecret.FromHex(),
                code
            );

            var token = await GitHubClientBase.Instance.Oauth.CreateAccessToken(tokenRequest);
            return token.AccessToken;
        }
    }
}
