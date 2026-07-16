using DailyFortune.Application.Interfaces;
using DailyFortune.Domain.Entities;
using System.Text.Json;

namespace DailyFortune.Infrastructure.Services;

public class StreakService : IStreakService
{
    public event Action? StreakUpdated;
    private readonly ISettingsService _settingsService;

    public TimeSpan RequiredInterval { get; set; } = TimeSpan.FromSeconds(30);

    public StreakService(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public History GetHistory()
    {
        var settings = _settingsService.GetSettings();
        return settings.History ?? new History();
    }

    public bool HasLocation()
    {
        var settings = _settingsService.GetSettings();
        return !string.IsNullOrWhiteSpace(settings.LocationInfo?.City);
    }

    public void SaveLocation(string city, string state, string country, double lat, double lon)
    {
        var settings = _settingsService.GetSettings();
        settings.LocationInfo ??= new LocationInfo();
        settings.LocationInfo.City = city;
        settings.LocationInfo.RegionCode = state;
        settings.LocationInfo.CountryCode = country;
        settings.LocationInfo.Latitude = lat;
        settings.LocationInfo.Longitude = lon;

        _settingsService.UpdateSettings(settings);

        StreakUpdated?.Invoke();
    }

    public bool HasFortuneBeenOpened(Fortune fortune)
    {
        var settings = _settingsService.GetSettings();

        if (fortune is SpecialFortune)
        {
            return settings.History.SpecialFortunes.Any(f => f.FortuneId == fortune.Id);
        }

        if (fortune is WeatherFortune)
        {
            return settings.History.WeatherFortunes.Any(f => f.FortuneId == fortune.Id);
        }

        return settings.History.Fortunes.Any(f => f.FortuneId == fortune.Id);
    }

    public void RecordOpenedFortune(Fortune fortune)
    {
        var settings = _settingsService.GetSettings();

        var entry = new FortuneHistoryItem
        {
            FortuneId = fortune.Id,
            OpenedDate = DateTime.Now
        };

        if (fortune is SpecialFortune)
        {
            settings.History.SpecialFortunes.Add(entry);
        }
        else if (fortune is WeatherFortune)
        {
            settings.History.WeatherFortunes.Add(entry);
        }
        else
        {
            settings.History.Fortunes.Add(entry);
        }

        _settingsService.UpdateSettings(settings);

        StreakUpdated?.Invoke();
    }


    public StreakInfo GetStreak()
    {
        var settings = _settingsService.GetSettings();

        return settings.StreakInfo ?? new StreakInfo();
    }


    public StreakInfo UpdateStreak()
    {
        var settings = _settingsService.GetSettings();

        var streak = settings.StreakInfo ?? new StreakInfo();

        // Simple daily logic: one fortune per calendar day. If already opened today, do nothing.
        var today = DateTime.Today;

        if (streak.LastOpenedDate.HasValue && streak.LastOpenedDate.Value.Date == today)
        {
            return streak;
        }

        var yesterday = today.AddDays(-1);

        if (streak.LastOpenedDate.HasValue && streak.LastOpenedDate.Value.Date == yesterday)
        {
            streak.CurrentStreak++;
        }
        else
        {
            streak.CurrentStreak = 1;
        }

        streak.LastOpenedDate = today;

        streak.LongestStreak = Math.Max(
            streak.LongestStreak,
            streak.CurrentStreak);

        settings.StreakInfo = streak;

        _settingsService.UpdateSettings(settings);

        StreakUpdated?.Invoke();

        return streak;
    }

    public void ResetForNewDay()
    {
        var settings = _settingsService.GetSettings();

        var streak = settings.StreakInfo ?? new StreakInfo();

        var today = DateTime.Today;
        var yesterday = today.AddDays(-1);

        // If there is a streak and the user did NOT open yesterday, reset the streak to 0 until opened
        if (streak.CurrentStreak > 0 && (!streak.LastOpenedDate.HasValue || streak.LastOpenedDate.Value.Date != yesterday))
        {
            streak.CurrentStreak = 0;

            settings.StreakInfo = streak;
            _settingsService.UpdateSettings(settings);

            StreakUpdated?.Invoke();
        }
    }
}