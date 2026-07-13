using DailyFortune.Application.Interfaces;
using DailyFortune.Domain.Entities;

namespace DailyFortune.Application.Services;

public class FortuneService
{
    private readonly IFortuneRepository _repository;
    private readonly FortuneSelectionService _fortuneSelectionService;

    public FortuneService(IFortuneRepository repository, FortuneSelectionService fortuneSelectionService)
    {
        _repository = repository;
        _fortuneSelectionService = fortuneSelectionService;
    }

    public async Task<Fortune> GetRandomFortune()
    {
        return await _fortuneSelectionService.GetFortuneAsync();
    }

    public async Task<Fortune> GetFortuneAsync() => await _fortuneSelectionService.GetFortuneAsync();
}