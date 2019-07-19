using MagicianHub.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                    await LoginPageViewModel.Instance.GetTokenFromBrowserUri(code)
                        .ContinueWith(token =>
                    {
                        LoginPageViewModel.Instance.UseAccessToken = true;
                        LoginPageViewModel.Instance.AccessToken = token.Result;
                        LoginPageViewModel.Instance.AuthorizationCommand.Execute(null);
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                    
                }
            }
        }
    }
}
