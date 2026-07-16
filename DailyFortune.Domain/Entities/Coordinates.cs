using System.Text.Json.Serialization;

namespace DailyFortune.Domain.Entities;

public class Coordinates
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; init; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; init; }

    [JsonPropertyName("city")]
    public string City { get; init; }

    [JsonPropertyName("region_code")]
    public string RegionCode { get; set; }

    [JsonPropertyName("country_code")]
    public string CountryCode { get; set; }


    public bool IsNotNull() => Latitude != 0 && Longitude != 0 && City != null;
}
