using NUnit.Framework;
using System;
using System.IO;
using DailyFortune.Infrastructure.Services;
using DailyFortune.Domain.Entities;
using Moq;
using DailyFortune.Application.Interfaces;

namespace DailyFortune.Tests;

[TestFixture]
public class HistoryTests
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
        var fi = typeof(StreakService).GetField("_settingsPath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        fi!.SetValue(svc, path);
        return svc;
    }

    [Test]
    public void RecordAndQueryHistory_Works()
    {
        var svc = CreateServiceWithPath(_settingsFile);

        var fortune = new Fortune { Id = 42, Text = "Hello" };

        svc.RecordOpenedFortune(fortune);

        var history = svc.GetHistory();
        Assert.That(history.Fortunes.Count, Is.EqualTo(1));
        Assert.That(history.Fortunes[0].FortuneId, Is.EqualTo(42));

        Assert.That(svc.HasFortuneBeenOpened(fortune));
    }
}
