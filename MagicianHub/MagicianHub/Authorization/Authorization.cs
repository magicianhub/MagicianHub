using MagicianHub.Github;
using Octokit;
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
    }
}
