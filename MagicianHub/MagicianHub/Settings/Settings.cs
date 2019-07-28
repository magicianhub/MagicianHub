using System;

namespace MagicianHub.Settings
{
    public class Settings
    {
        public class Rootobject
        {
            public Auth Auth { get; set; } = new Auth();
            public Logging Logging { get; set; } = new Logging();
        }

        public class Auth
        {
            public int AutoLogInAccountByIndex { get; set; } = 0;
            public bool LoadSavedAccounts { get; set; } = true;
            public string[] NeverAskSave { get; set; } = Array.Empty<string>();
            public Savedaccount[] SavedAccounts { get; set; } = Array.Empty<Savedaccount>();
        }

        public class Savedaccount
        {
            public string Name { get; set; } = string.Empty;
            public string Nickname { get; set; } = string.Empty;
        }

        public class Logging
        {
            public bool EnableLogging { get; set; } = true;
            public bool EnableLoggingMethodNames { get; set; } = false;
            public bool EnableArchiveOldFileOnStartup { get; set; } = true;
            public bool EnableArchiveFileCompression { get; set; } = true;
            public int MaxLoggerArchiveLogFiles { get; set; } = 10;
        }
    }
}