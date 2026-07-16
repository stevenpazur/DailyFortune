using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DailyFortune.Views;

public partial class WeatherSetupView : UserControl
{
    public WeatherSetupView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
