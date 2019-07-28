using MagicianHub.Github;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;

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
                Debug.WriteLine("Using default settings because json file not correct");
                _settings = new Settings.Rootobject();
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("Using default settings because json file not exists");
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
