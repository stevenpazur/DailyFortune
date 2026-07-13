using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DailyFortune.Domain.Entities;

public class Fortune
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; } = "";

    [JsonPropertyName("text")]
    public string Text { get; set; } = "";
}