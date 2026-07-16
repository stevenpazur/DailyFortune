using System.Text.Json.Serialization;

namespace DailyFortune.Domain.Entities;

public class AppSettings
{
    [JsonPropertyName("locationInfo")]
    public LocationInfo LocationInfo { get; set; } = new LocationInfo();

    [JsonPropertyName("streakInfo")]
    public StreakInfo StreakInfo { get; set; } = new StreakInfo();

    [JsonPropertyName("history")]
    public History History { get; set; } = new History();
}

public class LocationInfo
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("regionCode")]
    public string? RegionCode { get; set; }

    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; set; }
}

public class History
{
    [JsonPropertyName("fortunes")]
    public List<FortuneHistoryItem> Fortunes { get; set; } = new List<FortuneHistoryItem>();

    [JsonPropertyName("weatherFortunes")]
    public List<FortuneHistoryItem> WeatherFortunes { get; set; } = new List<FortuneHistoryItem>();

    [JsonPropertyName("specialFortunes")]
    public List<FortuneHistoryItem> SpecialFortunes { get; set; } = new List<FortuneHistoryItem>();
}

public class FortuneHistoryItem
{
    [JsonPropertyName("fortuneId")]
    public int FortuneId { get; set; }

    [JsonPropertyName("openedDate")]
    public DateTime OpenedDate { get; set; }
}
