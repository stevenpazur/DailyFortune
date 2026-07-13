using Avalonia;
using DailyFortune.Application.Interfaces;
using DailyFortune.Application.Services;
using DailyFortune.Infrastructure.Repositories;
using DailyFortune.Infrastructure.Services;
using DailyFortune.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DailyFortune
{
    internal sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static IServiceProvider Services { get; private set; } = null!;

        public static AppBuilder BuildAvaloniaApp()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IFortuneRepository, JsonFortuneRepository>();
            services.AddSingleton<FortuneService>();
            services.AddSingleton<FortuneSelectionService>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<FortuneCookieViewModel>();
            services.AddSingleton<ILocationService, IpLocationService>();
            services.AddSingleton<IWeatherService, OpenWeatherMapService>();
            services.AddSingleton<WeatherCodeMapper>();

            services.AddHttpClient(
                "LocationClient",
                client =>
                {
                    client.BaseAddress =
                        new Uri("https://ipwho.is/");
                });


            services.AddHttpClient(
                "WeatherClient",
                client =>
                {
                    client.BaseAddress =
                        new Uri("https://api.openweathermap.org/");
                });

            Services = services.BuildServiceProvider();

            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
# if DEBUG
                .WithDeveloperTools()
# endif
                .LogToTrace();
        }
    }
}
