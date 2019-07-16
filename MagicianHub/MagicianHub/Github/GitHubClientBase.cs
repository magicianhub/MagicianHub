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
    }
}
