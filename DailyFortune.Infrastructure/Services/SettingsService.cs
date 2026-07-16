using DailyFortune.Application.Interfaces;
using DailyFortune.Domain.Entities;

namespace DailyFortune.Infrastructure.Services
{
    public class SettingsService : ISettingsService
    {
        public SettingsService() { }

        private string GetSettingsPath()
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "DailyFortune");

            Directory.CreateDirectory(folder);

            return Path.Combine(folder, "settings.json");
        }

        private AppSettings ReadSettingsFromFile()
        {
            var settingsPath = GetSettingsPath();
            if (!File.Exists(settingsPath))
            {
                var defaultSettings = new AppSettings
                {
                    LocationInfo = new LocationInfo
                    {
                        Latitude = 0.0,
                        Longitude = 0.0,
                        City = "",
                        RegionCode = "",
                        CountryCode = ""
                    },
                };
                File.WriteAllText(settingsPath, System.Text.Json.JsonSerializer.Serialize(defaultSettings, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
            }
            var settingsJson = File.ReadAllText(settingsPath);
            var settings = System.Text.Json.JsonSerializer.Deserialize<AppSettings>(settingsJson);
            return settings ?? new AppSettings();
        }

        public AppSettings GetSettings()
        {
            return ReadSettingsFromFile();
        }

        public void UpdateSettings(AppSettings settings)
        {
            var settingsPath = GetSettingsPath();
            File.WriteAllText(settingsPath, System.Text.Json.JsonSerializer.Serialize(settings, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
