using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyFortune.Application.Interfaces;
using DailyFortune.Application.Services;
using DailyFortune.Domain.Entities;
using DailyFortune.Services;
using System;
using System.Threading.Tasks;

namespace DailyFortune.ViewModels;

public partial class FortuneCookieViewModel : ObservableObject
{
    private readonly FortuneService _fortuneService;
    private readonly IStreakService _streakService;
    private Bitmap? _spriteSheet;
    private DateTime _lastCheckedDate;
    private readonly TrayIconService _trayService;

    [ObservableProperty]
    private Fortune currentFortune = new();

    [ObservableProperty]
    private bool isOpening;

    [ObservableProperty]
    private bool isOpened;

    [ObservableProperty]
    private int currentFrame;

    [ObservableProperty]
    private bool canOpen = true;

    [ObservableProperty]
    private CroppedBitmap? cookieImage;

    [ObservableProperty]
    private double cookieRotation;

    [ObservableProperty]
    private FortuneCookieType cookieType;

    [ObservableProperty]
    private string fortuneText = "Click the cookie!";


    public FortuneCookieViewModel(
    FortuneService fortuneService,
    IStreakService streakService,
    TrayIconService trayService)
    {
        _fortuneService = fortuneService;
        _streakService = streakService;
        _trayService = trayService;

        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        FortuneText = "Click the cookie!";
        await PrepareFortuneAsync();

        CookieImage = LoadCookieFrame(0);

        _ = StartIdleAnimation();

        _lastCheckedDate = DateTime.Today;
        _ = MonitorDateChange();

        _trayService.SetFortuneReadyIcon();
    }

    private async Task MonitorDateChange()
    {
        while (true)
        {
            try
            {
                var now = DateTime.Now;
                var nextMidnight = DateTime.Today.AddDays(1);
                var delay = nextMidnight - now;

                if (delay < TimeSpan.FromSeconds(1))
                    delay = TimeSpan.FromSeconds(1);

                await Task.Delay(delay);

                // At local midnight, adjust streak state and reset UI
                _streakService.ResetForNewDay();

                await ResetForNewDayAsync();

                _lastCheckedDate = DateTime.Today;
            }
            catch
            {
                // ignore and continue monitoring; wait a short time before retrying
                await Task.Delay(TimeSpan.FromSeconds(30));
            }
        }
    }

    private async Task ResetForNewDayAsync()
    {
        // Reset visual state
        IsOpening = false;
        IsOpened = false;
        CanOpen = true;
        CookieRotation = 0;

        // Load a new fortune and sprite sheet, then reset frame
        await PrepareFortuneAsync();

        CurrentFrame = 0;
        CookieImage = LoadCookieFrame(0);

        FortuneText = "Click the cookie!";
    }

    private async Task PrepareFortuneAsync()
    {
        CurrentFortune = await _fortuneService.GetFortuneAsync();

        CookieType = CurrentFortune is SpecialFortune
            ? FortuneCookieType.Gold
            : FortuneCookieType.Standard;

        LoadSpriteSheet();
    }

    private void LoadSpriteSheet()
    {
        var path = CookieType == FortuneCookieType.Gold
            ? "avares://DailyFortune/Assets/FortuneCookies/Gold/cookie-spritesheet.png"
            : "avares://DailyFortune/Assets/FortuneCookies/Standard/cookie-spritesheet.png";

        using var stream = AssetLoader.Open(new Uri(path));

        _spriteSheet = new Bitmap(stream);
    }

    [RelayCommand]
    private async Task OpenCookie()
    {
        //if (!CanOpen || IsOpening)
        //    return;

        IsOpening = true;
        CanOpen = false;

        CookieRotation = 0;

        for (int i = 0; i < 5; i++)
        {
            CurrentFrame = i;
            CookieImage = LoadCookieFrame(i);

            await Task.Delay(100);
        }

        IsOpened = true;
        IsOpening = false;
        FortuneText = CurrentFortune.Text;

        _streakService.UpdateStreak();
        _streakService.RecordOpenedFortune(CurrentFortune);

        // Ensure the cookie presents a still opened image (last frame)
        CurrentFrame = 4;
        CookieImage = LoadCookieFrame(4);
        CanOpen = false;
        _trayService.SetFortuneOpenedIcon();
    }

    private CroppedBitmap LoadCookieFrame(int frame)
    {
        if (_spriteSheet == null)
            throw new InvalidOperationException("Sprite sheet not loaded.");

        const int frameWidth = 128;
        const int frameHeight = 128;

        var x = frame * frameWidth;
        var y = (640 - frameHeight) / 2;

        return new CroppedBitmap(
            _spriteSheet,
            new PixelRect(
                x,
                y,
                frameWidth,
                frameHeight));
    }

    private async Task StartIdleAnimation()
    {
        while (true)
        {
            if (!IsOpening && !IsOpened)
            {
                await AnimateWildly();
            }
            else
            {
                CookieRotation = 0;

                await Task.Delay(250);
            }
        }
    }

    private async Task AnimateWildly()
    {
        CookieRotation = -8;
        await Task.Delay(50);

        CookieRotation = 8;
        await Task.Delay(50);

        CookieRotation = -6;
        await Task.Delay(50);

        CookieRotation = 6;
        await Task.Delay(50);

        CookieRotation = 0;
        await Task.Delay(75);

        CookieRotation = -8;
        await Task.Delay(50);

        CookieRotation = 8;
        await Task.Delay(50);

        CookieRotation = -6;
        await Task.Delay(50);

        CookieRotation = 6;
        await Task.Delay(50);

        CookieRotation = 0;

        await Task.Delay(300);
    }
}