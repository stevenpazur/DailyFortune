using Avalonia;
using DailyFortune.Application.Interfaces;
using DailyFortune.Application.Services;
using DailyFortune.Infrastructure.Repositories;
using DailyFortune.Infrastructure.Services;
using DailyFortune.Services;
using DailyFortune.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace DailyFortune
{
    internal sealed class Program
    {
        private static Mutex? _mutex;

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            const string appName = "DailyFortune.SingleInstance";

            _mutex = new Mutex(
                true,
                appName,
                out bool createdNew);

            if (!createdNew)
            {
                // App is already running
                return;
            }

            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static IServiceProvider Services { get; private set; } = null!;

        public static AppBuilder BuildAvaloniaApp()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddSingleton<IFortuneRepository, JsonFortuneRepository>();
            services.AddSingleton<FortuneService>();
            services.AddSingleton<FortuneSelectionService>();
            services.AddSingleton<HistoryViewModel>();
            services.AddSingleton<WeatherSetupViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<StreakDisplayViewModel>();
            services.AddSingleton<FortuneCookieViewModel>();
            services.AddSingleton<ILocationService, IpLocationService>();
            services.AddSingleton<IWeatherService, OpenWeatherMapService>();
            services.AddSingleton<WeatherCodeMapper>();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<IStreakService, StreakService>();
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<App>();

            services.AddSingleton<TrayIconService>();

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
