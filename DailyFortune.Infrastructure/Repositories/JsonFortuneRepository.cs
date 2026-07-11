using System.Text.Json;
using DailyFortune.Application.Interfaces;
using DailyFortune.Domain.Entities;

namespace DailyFortune.Infrastructure.Repositories;

public class JsonFortuneRepository : IFortuneRepository
{
    private readonly List<Fortune> _fortunes;

    public JsonFortuneRepository()
    {
        var json = File.ReadAllText("Data/fortunes.json");

        var data = JsonSerializer.Deserialize<FortuneData>(json);

        _fortunes = data?.Fortunes ?? [];
    }

    public List<Fortune> GetFortunes()
    {
        return _fortunes;
    }
}

public class FortuneData
{
    public List<Fortune> Fortunes { get; set; } = [];
}