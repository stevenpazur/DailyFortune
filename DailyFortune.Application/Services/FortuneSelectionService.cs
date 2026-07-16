using DailyFortune.Application.Interfaces;
using DailyFortune.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyFortune.Application.Services;

public class FortuneSelectionService
{
    private readonly IWeatherService _weatherService;
    private readonly IFortuneRepository _fortuneRepository;
    private readonly IStreakService _streakService;

    public FortuneSelectionService(IWeatherService weatherService, IFortuneRepository fortuneRepository, IStreakService streakService)
    {
        _weatherService = weatherService;
        _fortuneRepository = fortuneRepository;
        _streakService = streakService;
    }

    public async Task<Fortune> GetFortuneAsync()
    {
        var weather = await _weatherService.GetCurrentWeatherAsync();

        var fortunes = _fortuneRepository.GetFortunes();
        var weatherFortunes = _fortuneRepository.GetWeatherFortunes();
        var specialFortunes = _fortuneRepository.GetSpecialFortunes();

        var matchingSpecialFortunes = FindMatchingSpecialFortunes(specialFortunes);

        // Find fortunes matching today's weather
        var matchingWeatherFortunes = weatherFortunes
            .Where(f => string.Compare(f.WeatherType, weather.WeatherType.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
            .ToList();

        if (matchingSpecialFortunes.Any())
        {
            return RandomSpecialFortune(specialFortunes);
        }

        if (matchingWeatherFortunes.Any())
        {
            var chance = GetWeatherFortuneChance(weather.WeatherType);

            if (Random.Shared.NextDouble() < chance)
            {
                return RandomWeatherFortune(weatherFortunes);
            }
        }

        return RandomGeneralFortune(fortunes);
    }

    private List<SpecialFortune> FindMatchingSpecialFortunes(
        List<SpecialFortune> specialFortunes)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        // ❤ We met on November 28, 2025
        var relationshipStart = new DateOnly(2025, 11, 28);

        var results = new List<SpecialFortune>();

        // Relationship milestones
        var daysTogether = today.DayNumber - relationshipStart.DayNumber;

        foreach (var fortune in specialFortunes)
        {
            if (fortune.RelationshipDay.HasValue &&
                fortune.RelationshipDay == daysTogether)
            {
                results.Add(fortune);
            }
        }

        // Holidays
        foreach (var fortune in specialFortunes)
        {
            var dates = fortune.Date?.Split("-", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (dates != null && dates.Length == 2)
            {
                if (dates[0] == today.Month.ToString() &&
                    dates[1] == today.Day.ToString())
                {
                    results.Add(fortune);
                }
            }
        }

        return results;
    }

    private Fortune RandomSpecialFortune(List<SpecialFortune> specialFortunes)
    {
        // Try up to 3 times to find a special fortune not already recorded in history
        Fortune? candidate = null;
        for (int attempt = 0; attempt < 3; attempt++)
        {
            var selected = specialFortunes[Random.Shared.Next(specialFortunes.Count)];
            if (!_streakService.HasFortuneBeenOpened(selected))
            {
                candidate = selected;
                break;
            }
            candidate = selected; // keep last selected if all attempts fail
        }

        return candidate!;
    }

    private Fortune RandomWeatherFortune(List<WeatherFortune> weatherFortunes)
    {
        Fortune? candidate = null;
        for (int attempt = 0; attempt < 3; attempt++)
        {
            var selected = weatherFortunes[Random.Shared.Next(weatherFortunes.Count)];
            if (!_streakService.HasFortuneBeenOpened(selected))
            {
                candidate = selected;
                break;
            }
            candidate = selected;
        }

        return candidate!;
    }

    private Fortune RandomGeneralFortune(List<Fortune> fortunes)
    {
        Fortune? candidate = null;
        for (int attempt = 0; attempt < 3; attempt++)
        {
            var selected = fortunes[Random.Shared.Next(fortunes.Count)];
            if (!_streakService.HasFortuneBeenOpened(selected))
            {
                candidate = selected;
                break;
            }
            candidate = selected;
        }

        return candidate!;
    }

    private static double GetWeatherFortuneChance(WeatherType weather)
    {
        return weather switch
        {
            WeatherType.Sunny => 0.15,

            WeatherType.Cloudy => 0.20,

            WeatherType.Windy => 0.20,

            WeatherType.Fog => 0.20,

            WeatherType.Rain => 0.70,

            WeatherType.Snow => 0.85,

            WeatherType.Storm => 0.95,

            _ => 0.0
        };
    }
}
