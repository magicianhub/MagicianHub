using MagicianHub.Annotations;
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

        public static void AddSecuredCreds([NotNull] string login, string password)
        {
            if (string.IsNullOrEmpty(login))
            {
                try
                {
                    login = GitHubClientBase.Instance.User.Current().Result.Login;
                }
                catch (Exception e)
                {
                    Log.Error("Unable get user login, because unable");
                    Log.Error(e.ToString);
                }
            }
            Log.Info($"Checking exsisting login ({login}) in saved accounts.");
            if (SettingsManager.GetSettings().Auth.SavedAccounts.Any(
                authSavedAccount => authSavedAccount.Nickname == login
            )) return;
            Log.Info($"Login ({login}) not found in saved accounts.");
            var vault = new PasswordVault();
            vault.Add(new PasswordCredential(
                GitHubClientBase.ClientName,
                login,
                password
            ));
            AddUserAbleCreds(login);
        }

        private static void AddUserAbleCreds([NotNull] string login)
        {
            var listFromSavedAccount = SettingsManager.GetSettings().Auth.SavedAccounts.ToList();
            listFromSavedAccount.Add(
                new Settings.Settings.Savedaccount
                {
                    Nickname = login,
                    Name = GitHubClientBase.Instance.User.Current().Result.Name
                }
            );
            SettingsManager.GetSettings().Auth.SavedAccounts = listFromSavedAccount.ToArray();
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
