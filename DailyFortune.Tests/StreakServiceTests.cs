using NUnit.Framework;
using System;
using System.IO;
using DailyFortune.Infrastructure.Services;
using DailyFortune.Domain.Entities;
using Moq;
using DailyFortune.Application.Interfaces;

namespace DailyFortune.Tests;

[TestFixture]
public class StreakServiceTests
{
    private string _tempPath = null!;
    private string _settingsFile = null!;

    [SetUp]
    public void SetUp()
    {
        _tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempPath);
        _settingsFile = Path.Combine(_tempPath, "settings.json");
    }

    [TearDown]
    public void TearDown()
    {
        try { Directory.Delete(_tempPath, true); } catch { }
    }

    private StreakService CreateServiceWithPath(string path)
    {
        var settingsService = new Mock<ISettingsService>();
        var svc = new StreakService(settingsService.Object);
        // override private field _settingsPath
        var fi = typeof(StreakService).GetField("_settingsPath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        fi!.SetValue(svc, path);
        return svc;
    }

    [Test]
    public void UpdateStreak_FirstTime_SetsCurrentTo1()
    {
        var svc = CreateServiceWithPath(_settingsFile);

        var info = svc.UpdateStreak();

        Assert.That(info.CurrentStreak, Is.EqualTo(1));
        Assert.That(File.Exists(_settingsFile));
    }

    [Test]
    public void UpdateStreak_SameDay_NoChange()
    {
        var svc = CreateServiceWithPath(_settingsFile);

        var first = svc.UpdateStreak();
        var second = svc.UpdateStreak();

        Assert.That(second.CurrentStreak, Is.EqualTo(first.CurrentStreak));
    }

    [Test]
    public void UpdateStreak_ConsecutiveDays_Increments()
    {
        var svc = CreateServiceWithPath(_settingsFile);

        var first = svc.UpdateStreak();

        // simulate yesterday by loading the settings object and setting LastOpenedDate to yesterday
        var settingsJson = File.ReadAllText(_settingsFile);
        var settings = System.Text.Json.JsonSerializer.Deserialize<AppSettings>(settingsJson)
            ?? new AppSettings();

        settings.StreakInfo ??= new StreakInfo();
        settings.StreakInfo.LastOpenedDate = DateTime.Today.AddDays(-1);

        File.WriteAllText(_settingsFile, System.Text.Json.JsonSerializer.Serialize(settings, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

        var second = svc.UpdateStreak();

        Assert.That(second.CurrentStreak, Is.EqualTo(first.CurrentStreak + 1));
    }

    [Test]
    public void ResetForNewDay_ResetsWhenMissedPreviousDay()
    {
        var svc = CreateServiceWithPath(_settingsFile);

        var info = svc.UpdateStreak(); // day 1

        // simulate last opened two days ago
        var json = File.ReadAllText(_settingsFile);
        json = json.Replace("\"lastOpenedDate\": null", $"\"lastOpenedDate\": \"{DateTime.Today.AddDays(-2).ToString("yyyy-MM-ddTHH:mm:ss")}\"");
        File.WriteAllText(_settingsFile, json);

        svc.ResetForNewDay();

        var after = svc.GetStreak();
        Assert.That(after.CurrentStreak, Is.EqualTo(0));
    }
}
