using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Input;
using DailyFortune.Services;
using DailyFortune.ViewModels;
using DailyFortune.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DailyFortune;

public partial class App : Avalonia.Application
{
    private IClassicDesktopStyleApplicationLifetime? _desktop;

    private readonly TrayIconService _trayService;

    public App()
    {
        _trayService = Program.Services.GetRequiredService<TrayIconService>();

        DataContext = this;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        var trayIcons = TrayIcon.GetIcons(this);

        if (trayIcons != null && trayIcons.Count > 0)
        {
            var tray = trayIcons[0];

            _trayService.Register(tray);

            tray.Clicked += (_, _) =>
            {
                ShowMainWindow();
            };
        }
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime
            is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _desktop = desktop;

            ShowWindowRequested += ShowMainWindow;
            ExitRequested += ExitApplication;

            var mainWindow = new MainWindow
            {
                DataContext =
                    Program.Services.GetRequiredService<MainViewModel>()
            };

            mainWindow.Closing += (_, e) =>
            {
                // Hide instead of exiting
                e.Cancel = true;
                mainWindow.Hide();
            };

            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    public void ShowMainWindow()
    {
        if (_desktop?.MainWindow is null)
            return;

        _desktop.MainWindow.Show();
        _desktop.MainWindow.WindowState = WindowState.Normal;
        _desktop.MainWindow.Activate();
    }


    public void ExitApplication()
    {
        _desktop?.Shutdown();
    }

    public event Action? ShowWindowRequested;
    public event Action? ExitRequested;

    [RelayCommand]
    private void OpenWindow()
    {
        ShowWindowRequested?.Invoke();
    }

    [RelayCommand]
    private void Exit()
    {
        ExitRequested?.Invoke();
    }
}