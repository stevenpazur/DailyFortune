using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyFortune.Application.Interfaces;
using System;

namespace DailyFortune.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public FortuneCookieViewModel Cookie { get; }

    public StreakDisplayViewModel Streak { get; }

    public HistoryViewModel History { get; }

    public WeatherSetupViewModel WeatherSetup { get; }

    [ObservableProperty]
    private bool showHistory;

    [ObservableProperty]
    private bool showWeatherSetup;

    public event Action? ExitRequested;
    public event Action? ShowWindowRequested;

    private readonly App _app;

    public MainViewModel(
        FortuneCookieViewModel cookie,
        StreakDisplayViewModel streak,
        HistoryViewModel history,
        WeatherSetupViewModel weatherSetup,
        IStreakService streakService,
        INotificationService notificationService,
        App app)
    {
        Cookie = cookie;
        Streak = streak;
        History = history;
        WeatherSetup = weatherSetup;
        _app = app;

        // Show weather setup when there is no saved location
        ShowHistory = false;
        // initialize backing field directly to avoid referencing generated property before source gen
        showWeatherSetup = !streakService.HasLocation();

        // Listen for location saved
        streakService.StreakUpdated += () =>
        {
            if (streakService.HasLocation())
            {
                ShowWeatherSetup = false;
            }
        };

        //notificationService.Show(
        //    "🥠 Daily Fortune",
        //    "Dear, your fortune is ready 💖");
    }

    [RelayCommand]
    private void ToggleHistory()
    {
        ShowHistory = !ShowHistory;
    }

    [RelayCommand]
    private void ToggleLocationEditor()
    {
        ShowWeatherSetup = !ShowWeatherSetup;
    }
}
