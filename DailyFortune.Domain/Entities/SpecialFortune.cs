using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DailyFortune.Domain.Entities;

public class SpecialFortune : Fortune
{
    [JsonPropertyName("occasionType")]
    public string OccasionType { get; set; }

    [JsonPropertyName("occasion")]
    public string Occasion { get; set; }

    [JsonPropertyName("recurrence")]
    public string Recurrence { get; set; }

    [JsonPropertyName("relationshipDay")]
    public int? RelationshipDay { get; set; }

    public string Date { get; set; }
}