using System.Text.Json.Serialization;

namespace DailyFortune.Infrastructure.Models;

public class OpenWeatherResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("main")]
    public MainInfo Main { get; set; } = new();

    [JsonPropertyName("weather")]
    public List<WeatherDescription> Weather { get; set; } = [];
}


public class MainInfo
{
    [JsonPropertyName("temp")]
    public double Temp { get; set; }
}


public class WeatherDescription
{
    [JsonPropertyName("main")]
    public string Main { get; set; } = "";

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";
}