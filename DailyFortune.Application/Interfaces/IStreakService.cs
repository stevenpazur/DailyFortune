using DailyFortune.Domain.Entities;
using System;

namespace DailyFortune.Application.Interfaces;

public interface IStreakService
{
    StreakInfo GetStreak();

    StreakInfo UpdateStreak();
    History GetHistory();
    void ResetForNewDay();
    bool HasFortuneBeenOpened(Fortune fortune);
    void RecordOpenedFortune(Fortune fortune);
    bool HasLocation();
    void SaveLocation(string city, string state, string country, double lat, double lon);
    event Action? StreakUpdated;
}