using Octokit;

namespace MagicianHub.Github
{
    public static class GitHubClientBase
    {
        public static GitHubClient Instance;
        public const string BaseClientId = "MagicianHub@";
        // ReSharper disable once InconsistentNaming
        public const string Client2FAAction = "2FA";
        public const string ClientId = "cc7b59ba4777e92ddb18";
        public const string ClientSecret = "30343433393563626465646265313331326236626136343239373763303863623534396630336638";
    }
}
