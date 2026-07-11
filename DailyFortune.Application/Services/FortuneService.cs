using DailyFortune.Application.Interfaces;
using DailyFortune.Domain.Entities;

namespace DailyFortune.Application.Services;

public class FortuneService
{
    private readonly IFortuneRepository _repository;

    public FortuneService(IFortuneRepository repository)
    {
        _repository = repository;
    }

    public Fortune GetRandomFortune()
    {
        var fortunes = _repository.GetFortunes();

        return fortunes[
            Random.Shared.Next(fortunes.Count)
        ];
    }
}