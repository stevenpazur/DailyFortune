using DailyFortune.Application.Interfaces;
using DailyFortune.Domain.Entities;
using DailyFortune.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace DailyFortune.Infrastructure.Services;

public class OpenWeatherMapService : IWeatherService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILocationService locationService;
    private readonly WeatherCodeMapper weatherCodeMapper;
    private readonly string apiKey;

    public OpenWeatherMapService(
        IHttpClientFactory httpClientFactory,
        ILocationService locationService,
        WeatherCodeMapper weatherCodeMapper,
        IConfiguration config)
    {
        this.httpClientFactory = httpClientFactory;
        this.locationService = locationService;
        this.weatherCodeMapper = weatherCodeMapper;
        apiKey = config["OpenWeatherMap:ApiKey"] 
            ?? throw new ArgumentNullException("OpenWeatherMap:ApiKey");
    }

    public async Task<WeatherInfo> GetCurrentWeatherAsync()
    {
        // 1. Get user's location
        var coordinates = await locationService.GetCoordinatesAsync();

        // 2. Call OpenWeather
        var http = httpClientFactory.CreateClient("WeatherClient");

        var url =
            $"data/2.5/weather" +
            $"?lat={coordinates.Latitude}" +
            $"&lon={coordinates.Longitude}" +
            $"&appid={apiKey}" +
            $"&units=metric";

        var response = await http.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var weatherResponse =
            JsonSerializer.Deserialize<OpenWeatherResponse>(json);

        if (weatherResponse == null)
        {
            throw new Exception("Unable to parse weather response.");
        }

        return new WeatherInfo
        {
            City = weatherResponse.Name,

            TemperatureCelsius =
                weatherResponse.Main.Temp,

            Description =
                weatherResponse.Weather.FirstOrDefault()?.Description
                ?? "",

            WeatherType =
                weatherCodeMapper.Map(
                    weatherResponse.Weather.FirstOrDefault()?.Main
                    ?? "")
        };
    }
}