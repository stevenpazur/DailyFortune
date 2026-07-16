using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;

namespace DailyFortune.Services;

public class TrayIconService
{
    private TrayIcon? _trayIcon;


    public void Register(TrayIcon trayIcon)
    {
        _trayIcon = trayIcon;
    }


    public void SetNormalIcon()
    {
        if (_trayIcon == null)
            return;

        _trayIcon.Icon =
            new WindowIcon(
                AssetLoader.Open(
                    new Uri(
                    "avares://DailyFortune/Assets/App.ico")));
    }


    public void SetFortuneReadyIcon()
    {
        if (_trayIcon == null)
            return;

        _trayIcon.Icon =
            new WindowIcon(
                AssetLoader.Open(
                    new Uri(
                    "avares://DailyFortune/Assets/FortuneReady.ico")));
    }

    public void SetFortuneOpenedIcon()
    {
        if (_trayIcon == null)
            return;

        _trayIcon.Icon =
            new WindowIcon(
                AssetLoader.Open(
                    new Uri(
                    "avares://DailyFortune/Assets/FortuneBroken.ico")));
    }
}