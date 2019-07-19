using MagicianHub.ViewModels;
using System.Collections.Generic;

namespace MagicianHub.Authorization
{
    public static class AuthorizationProtocol
    {
        public static async void ProcessProtocolAsync(Dictionary<string, string> authParameters)
        {
            if (authParameters.Count == 1)
            {
                if (authParameters.ContainsKey("code"))
                {
                    authParameters.TryGetValue("code", out var code);
                    var token = await LoginPageViewModel.Instance.GetTokenFromBrowserUri(code);
                    LoginPageViewModel.Instance.UseAccessToken = true;
                    LoginPageViewModel.Instance.AccessToken = token;
                    LoginPageViewModel.Instance.AuthorizationCommand.Execute(null);
                }
            }
        }
    }
}
