using System.Text.Json;
using System.Text.Json.Serialization;
using DailyFortune.Application.Interfaces;
using DailyFortune.Domain.Entities;

namespace DailyFortune.Infrastructure.Repositories;

public class JsonFortuneRepository : IFortuneRepository
{
    private readonly List<Fortune> _fortunes;
    private readonly List<WeatherFortune> _weatherFortunes;
    private readonly List<SpecialFortune> _specialFortunes;

    public JsonFortuneRepository()
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "fortunes.json"
        );

        var json = File.ReadAllText(path);

        var data = JsonSerializer.Deserialize<FortuneData>(json);

        _fortunes = data?.Fortunes ?? [];
        _weatherFortunes = data?.WeatherFortunes ?? [];
        _specialFortunes = data?.SpecialFortunes ?? [];
    }

    public List<Fortune> GetFortunes() => _fortunes;
    public List<WeatherFortune> GetWeatherFortunes() => _weatherFortunes;
    public List<SpecialFortune> GetSpecialFortunes() => _specialFortunes;
}

public class FortuneData
{
    [JsonPropertyName("fortunes")]
    public List<Fortune> Fortunes { get; set; } = [];

    [JsonPropertyName("specialFortunes")]
    public List<SpecialFortune> SpecialFortunes { get; set; }

    [JsonPropertyName("weatherFortunes")]
    public List<WeatherFortune> WeatherFortunes { get; set; }
}