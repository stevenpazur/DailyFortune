using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using DailyFortune.Domain.Entities;
using System;

namespace DailyFortune.ViewModels;

public partial class HistoryItemViewModel : ObservableObject
{
    [ObservableProperty]
    private string text = "";

    [ObservableProperty]
    private DateTime openedDate;

    [ObservableProperty]
    private CroppedBitmap? image;

    public HistoryItemViewModel(Fortune fortune, DateTime opened)
    {
        Text = fortune.Text;
        OpenedDate = opened;

        // determine sprite sheet based on fortune type
        var path = fortune is SpecialFortune
            ? "avares://DailyFortune/Assets/FortuneCookies/Gold/cookie-spritesheet.png"
            : "avares://DailyFortune/Assets/FortuneCookies/Standard/cookie-spritesheet.png";

        try
        {
            using var stream = AssetLoader.Open(new Uri(path));
            var bmp = new Bitmap(stream);

            // last frame index = 4, frame width/height as in FortuneCookieViewModel
            const int frame = 4;
            const int frameWidth = 128;
            const int frameHeight = 128;

            var x = frame * frameWidth;
            var y = (640 - frameHeight) / 2;

            Image = new CroppedBitmap(bmp, new Avalonia.PixelRect(x, y, frameWidth, frameHeight));
        }
        catch
        {
            Image = null;
        }
    }
}
