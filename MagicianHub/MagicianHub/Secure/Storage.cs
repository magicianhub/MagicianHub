using MagicianHub.Github;
using MagicianHub.Settings;
using System;
using System.Linq;
using Windows.Security.Credentials;
using static MagicianHub.Logger.Logger;

namespace MagicianHub.Secure
{
    public static class Storage
    {
        public static PasswordCredential GetSecuredCreds(string login)
        {
            var vault = new PasswordVault();
            PasswordCredential creds;
            try
            {
                creds = vault.Retrieve(GitHubClientBase.ClientName, login);
            }
            catch (Exception)
            {
                Log.Error($"An error occurred while getting account creds for: {login}");
                return new PasswordCredential();
            }
            return creds ?? new PasswordCredential();
        }

        public static void AddSecuredCreds(string login, string password)
        {
            if (SettingsManager.GetSettings().Auth.SavedAccounts.Any(
                authSavedAccount => authSavedAccount.Nickname == login
            )) return;

            var vault = new PasswordVault();
            vault.Add(new PasswordCredential(
                GitHubClientBase.ClientName,
                login,
                password
            ));
        }

        public static void RemoveSecuredCreds(string login)
        {
            var vault = new PasswordVault();
            try
            {
                vault.Remove(new PasswordCredential(
                    GitHubClientBase.ClientName,
                    login,
                    GetSecuredCreds(login).Password
                ));
            }
            catch (ArgumentException)
            {
                Log.Error($"An error occurred while removing saved account ({login})");
            }
        }

        public static bool IsToken(string password) => password.Contains("&token=true");

        public static string PasswordFromRawPassword(string password) =>
            password.Split("&token=true")[0];
    }
}
