using DailyFortune.Application.Interfaces;
using DailyFortune.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DailyFortune.Infrastructure.Services;

public class IpLocationService : ILocationService
{
    private readonly IHttpClientFactory httpClientFactory;

    public IpLocationService(IHttpClientFactory factory)
    {
        httpClientFactory = factory;
    }

    private string GetSettingsPath()
    {
        var settingsPath = Path.Combine(
            AppContext.BaseDirectory,
            "settings.json");

        return settingsPath;
    }

    public async Task<Coordinates> GetCoordinatesAsync()
    {
        var existingCoordinates = GetSettingsCoordinates();
        if (existingCoordinates != null && existingCoordinates.IsNotNull())
        { 
            return existingCoordinates;
        }

        var http = httpClientFactory.CreateClient("LocationClient");
        var response = await http.GetAsync("");
        Coordinates coordinates = new();

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            coordinates = JsonSerializer.Deserialize<Coordinates>(responseContent);
            if (coordinates != null)
            {
                var settingsPath = GetSettingsPath();
                
                var json = JsonSerializer.Serialize(
                    coordinates,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                await File.WriteAllTextAsync(settingsPath, json);
            }
        }

        return coordinates;
    }

    private Coordinates GetSettingsCoordinates()
    {
        var settingsPath = GetSettingsPath();

        var settings = File.ReadAllText(settingsPath);

        var coordinates = JsonSerializer.Deserialize<Coordinates>(settings ?? "{}");

        return coordinates;
    }
}
