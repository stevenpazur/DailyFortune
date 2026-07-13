using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DailyFortune.Domain.Entities;

public class WeatherFortune : Fortune
{
    [JsonPropertyName("weatherType")]
    public string WeatherType { get; set; }

    [JsonPropertyName("season")]
    public string Season { get; set; }

    [JsonPropertyName("mood")]
    public string Mood { get; set; }
}