using CommunityToolkit.Mvvm.ComponentModel;
using DailyFortune.Application.Interfaces;

using System;

namespace DailyFortune.ViewModels;

public partial class StreakDisplayViewModel : ObservableObject
{
    private readonly IStreakService _streakService;

    [ObservableProperty]
    private int currentStreak;

    [ObservableProperty]
    private int longestStreak;

    [ObservableProperty]
    private DateTime? lastOpenedDate;

    public StreakDisplayViewModel(IStreakService streakService)
    {
        _streakService = streakService;

        var s = _streakService.GetStreak();
        CurrentStreak = s.CurrentStreak;
        LongestStreak = s.LongestStreak;
        LastOpenedDate = s.LastOpenedDate;

        _streakService.StreakUpdated += OnStreakUpdated;
    }

    private void OnStreakUpdated()
    {
        var s = _streakService.GetStreak();
        CurrentStreak = s.CurrentStreak;
        LongestStreak = s.LongestStreak;
        LastOpenedDate = s.LastOpenedDate;
    }

    public bool ShowStreak => true;

    partial void OnCurrentStreakChanged(int value)
    {
        OnPropertyChanged(nameof(ShowStreak));
    }
}
