using System.Text.Json.Serialization;

namespace DailyFortune.Domain.Entities;

public class StreakInfo
{
    [JsonPropertyName("currentStreak")]
    public int CurrentStreak { get; set; }

    [JsonPropertyName("lastOpenedDate")]
    public DateTime? LastOpenedDate { get; set; }

    [JsonPropertyName("longestStreak")]
    public int LongestStreak { get; set; }
}