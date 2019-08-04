using MagicianHub.Github;
using Newtonsoft.Json;
using System.IO;
using static MagicianHub.Logger.Logger;

namespace MagicianHub.Settings
{
    public static class SettingsManager
    {
        private static Settings.Rootobject _settings;

        public static async void LoadSettings()
        {
            try
            {
                string json = await File.ReadAllTextAsync(GitHubClientBase.SettingsPath);
                _settings = JsonConvert.DeserializeObject<Settings.Rootobject>(json);
            }
            catch (JsonReaderException)
            {
                Log.Error("Using default settings because json file not correct");
                _settings = new Settings.Rootobject();
            }
            catch (FileNotFoundException)
            {
                Log.Warn("Using default settings because json file not exists");
                _settings = new Settings.Rootobject();
            }
        }

        public static Settings.Rootobject GetSettings() => _settings;

        public static async void SaveSettings()
        {
            string output = JsonConvert.SerializeObject(GetSettings(), Formatting.Indented);
            await File.WriteAllTextAsync(
                GitHubClientBase.SettingsPath,
                output
            );
        }
    }
}
