using SpohnStory.Models;
using System.Text.Json;

namespace SpohnStory.Configuration
{
    public interface IConfigurationService
    {
        LauncherConfiguration GetConfiguration();
        void SaveConfiguration(LauncherConfiguration config);
        void ResetToDefaults();
        string GetConfigurationPath();
    }

    public class ConfigurationService : IConfigurationService
    {
        private readonly string _configPath;
        private LauncherConfiguration _configuration;
        private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

        public ConfigurationService()
        {
            _configPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory ?? Environment.CurrentDirectory,
                "launcher.json");
            _configuration = LoadConfiguration();
        }

        public LauncherConfiguration GetConfiguration()
        {
            return _configuration;
        }

        public void SaveConfiguration(LauncherConfiguration config)
        {
            _configuration = config;
            var json = JsonSerializer.Serialize(_configuration, JsonOptions);
            File.WriteAllText(_configPath, json);
        }

        public void ResetToDefaults()
        {
            _configuration = new LauncherConfiguration();
            SaveConfiguration(_configuration);
        }

        public string GetConfigurationPath()
        {
            return _configPath;
        }

        private LauncherConfiguration LoadConfiguration()
        {
            try
            {
                if (File.Exists(_configPath))
                {
                    var json = File.ReadAllText(_configPath);
                    return JsonSerializer.Deserialize<LauncherConfiguration>(json, JsonOptions)
                           ?? new LauncherConfiguration();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configuration: {ex.Message}");
            }

            return new LauncherConfiguration();
        }
    }
}
