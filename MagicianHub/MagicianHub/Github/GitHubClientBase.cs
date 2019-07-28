using Octokit;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;

namespace MagicianHub.Github
{
    public static class GitHubClientBase
    {
        public static GitHubClient Instance;
        public const string BaseClientId = "MagicianHub@";
        // ReSharper disable once InconsistentNaming
        public const string Client2FAAction = "2FA";
        public const string ClientId = "cc7b59ba4777e92ddb18";
        public const string ClientSecret =
            "30343433393563626465646265313331326236626136343239373763303863623534396630336638";
        public static List<string> Scopes = new List<string>
        {
            "repo", "notifications", "user", "delete_repo"
        };

        public static string SettingsPath =
            Path.Combine(
                ApplicationData.Current.LocalFolder.Path,
                "settings.json"
            );
        
        public static string LoggerDirPath =
            Path.Combine(
                ApplicationData.Current.LocalFolder.Path,
                "logs"
            );

        public static string LatestLogFilePath =
            Path.Combine(
                LoggerDirPath,
                "latest.log"
            );
    }
}
