using DailyFortune.Domain.Entities;

namespace DailyFortune.Application.Services;

public class FortuneService
{
    private readonly FortuneSelectionService _fortuneSelectionService;

    public FortuneService(FortuneSelectionService fortuneSelectionService)
    {
        _fortuneSelectionService = fortuneSelectionService;
    }

    public async Task<Fortune> GetFortuneAsync() => await _fortuneSelectionService.GetFortuneAsync();
}