using MagicianHub.Extensions;
using MagicianHub.ViewModels;
using Microsoft.QueryStringDotNET;
using System;

namespace MagicianHub.Authorization
{
    public static class AuthorizationNotifyHandler
    {
        public static void ProcessAuthorizationNotify(
            AuthorizationNotifyTypes notifyType,
            QueryString args)
        {
            if (notifyType == AuthorizationNotifyTypes.TryReconnect)
            {
                var parameters = Uri.UnescapeDataString(args.ToString()).GetPartParameters();

                if (parameters.ContainsKey("login"))
                {
                    parameters.TryGetValue("login", out var login);
                    parameters.TryGetValue("pass", out var pass);
                    LoginPageViewModel.Instance.Login = login;
                    LoginPageViewModel.Instance.Password = pass;
                    LoginPageViewModel.Instance.UseAccessToken = false;
                    LoginPageViewModel.Instance.AuthorizationCommand.Execute(null);
                    return;
                }

                if (parameters.ContainsKey("token"))
                {
                    parameters.TryGetValue("token", out var token);
                    parameters.TryGetValue("login", out var login);
                    LoginPageViewModel.Instance.AccessToken = token;
                    LoginPageViewModel.Instance.Login = login;
                    LoginPageViewModel.Instance.UseAccessToken = true;
                    LoginPageViewModel.Instance.AuthorizationCommand.Execute(null);
                }
            }
            
        }
    }
}
