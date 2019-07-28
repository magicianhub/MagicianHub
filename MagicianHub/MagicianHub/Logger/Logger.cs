using MagicianHub.Github;
using MagicianHub.Settings;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Text;

namespace MagicianHub.Logger
{
    public static class Logger
    {
        public static readonly NLog.Logger Log = LogManager.GetCurrentClassLogger();

        private const string DefaultLayout =
            "[${longdate}] [${threadid}/${uppercase:${level}}]: ${message}";

        public static void Init()
        {
            if (!SettingsManager.GetSettings().Logging.EnableLogging) return;

            LoggingConfiguration config = new LoggingConfiguration();
            FileTarget logfile = new FileTarget("logfile")
            {
                FileName = $"{GitHubClientBase.LatestLogFilePath}",
                Layout = ResolveLayout(
                    SettingsManager.GetSettings().Logging.EnableLoggingMethodNames
                ),
                ArchiveFileName =
                    $"{GitHubClientBase.LoggerDirPath}//${{shortdate}}.log.gz",
                ArchiveOldFileOnStartup = 
                    SettingsManager.GetSettings().Logging.EnableArchiveOldFileOnStartup,
                EnableArchiveFileCompression = 
                    SettingsManager.GetSettings().Logging.EnableArchiveFileCompression,
                MaxArchiveFiles = 
                    SettingsManager.GetSettings().Logging.MaxLoggerArchiveLogFiles,
                Encoding = Encoding.UTF8
            };

            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);
            LogManager.Configuration = config;
        }

        private static string ResolveLayout(bool methodLogging)
        {
            return methodLogging
                ? DefaultLayout.Replace(
                    "[${longdate}] ",
                    "[${longdate}] [${callsite}] "
                )
                : DefaultLayout;
        }
    }
}