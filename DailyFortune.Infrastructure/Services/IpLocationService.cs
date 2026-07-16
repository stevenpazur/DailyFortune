using DailyFortune.Application.Interfaces;
using DailyFortune.Domain.Entities;
using DailyFortune.Infrastructure.Models;
using Microsoft.Extensions.Configuration;

namespace DailyFortune.Infrastructure.Services;

public class IpLocationService : ILocationService
{
    private readonly ISettingsService _settingsService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string apiKey;

    public IpLocationService(ISettingsService settingsService, IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _settingsService = settingsService;
        apiKey = config["OpenWeatherMap:ApiKey"]
            ?? throw new ArgumentNullException("OpenWeatherMap:ApiKey");
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Coordinates?> GetIpLocation()
    {
        var http = _httpClientFactory.CreateClient("LocationClient");
        var response = await http.GetAsync("");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var locationData = System.Text.Json.JsonSerializer.Deserialize<Coordinates>(content);
            if (locationData != null)
            {
                return locationData;
            }
        }
        return null;
    }

    public async Task<Coordinates?> GetCoordinatesAsync()
    {
        var appSettings = _settingsService.GetSettings();
        if (appSettings == null || appSettings.LocationInfo == null)
        {
            return null;
        }

        var existingCoordinates = new Coordinates
        {
            Latitude = appSettings.LocationInfo.Latitude,
            Longitude = appSettings.LocationInfo.Longitude,
            City = appSettings.LocationInfo.City,
            RegionCode = appSettings.LocationInfo.RegionCode,
            CountryCode = appSettings.LocationInfo.CountryCode
        };
        if (existingCoordinates != null && existingCoordinates.IsNotNull())
        { 
            return existingCoordinates;
        }

        // We will not use the IP address here to get the location.  Instead, we will use the location from settings.json file.
        return null;
    }

    public async Task<Coordinates?> GetLocationByCityStateCountryAsync(string city, string state, string country)
    {
        var locationParts = new List<string>()
        {
            city,
            state,
            country
        }.Where(str => !string.IsNullOrWhiteSpace(str));

        var url = $"geo/1.0/direct" +
            $"?q={string.Join(',', locationParts)}" +
            $"&limit=1" +
            $"&appid={apiKey}";

        var http = _httpClientFactory.CreateClient("WeatherClient");

        var response = await http.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var locationData = System.Text.Json.JsonSerializer.Deserialize<List<OpenWeatherGeoResponse>>(content);
            if (locationData != null && locationData.Count > 0)
            {
                return new Coordinates
                {
                    Latitude = locationData[0].Lat,
                    Longitude = locationData[0].Lon,
                    City = city,
                    RegionCode = state,
                    CountryCode = country
                };
            }
        }

        return null;
    }
}
