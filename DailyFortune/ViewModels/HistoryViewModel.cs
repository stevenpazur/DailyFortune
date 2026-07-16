using CommunityToolkit.Mvvm.ComponentModel;
using DailyFortune.Application.Interfaces;
using DailyFortune.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System;

namespace DailyFortune.ViewModels;

public partial class HistoryViewModel : ObservableObject
{
    private readonly IFortuneRepository _fortuneRepository;
    private readonly IStreakService _streakService;

    public ObservableCollection<HistoryItemViewModel> Items { get; } = new ObservableCollection<HistoryItemViewModel>();

    public HistoryViewModel(IFortuneRepository fortuneRepository, IStreakService streakService)
    {
        _fortuneRepository = fortuneRepository;
        _streakService = streakService;

        BuildItems();

        _streakService.StreakUpdated += () => BuildItems();
    }

    private void BuildItems()
    {
        Items.Clear();

        var history = _streakService.GetHistory();

        var repo = _fortuneRepository;

        var all = history.Fortunes.Select(h => (h.FortuneId, h.OpenedDate, Category: "general"))
            .Concat(history.WeatherFortunes.Select(h => (h.FortuneId, h.OpenedDate, Category: "weather")))
            .Concat(history.SpecialFortunes.Select(h => (h.FortuneId, h.OpenedDate, Category: "special")))
            .OrderByDescending(t => t.OpenedDate)
            .ToList();

        foreach (var entry in all)
        {
            Fortune? fortune = null;
            if (entry.Category == "special")
                fortune = repo.GetSpecialFortunes().FirstOrDefault(f => f.Id == entry.FortuneId);
            else if (entry.Category == "weather")
                fortune = repo.GetWeatherFortunes().FirstOrDefault(f => f.Id == entry.FortuneId);
            else
                fortune = repo.GetFortunes().FirstOrDefault(f => f.Id == entry.FortuneId);

            if (fortune != null)
            {
                Items.Add(new HistoryItemViewModel(fortune, entry.OpenedDate));
            }
        }
    }
}
