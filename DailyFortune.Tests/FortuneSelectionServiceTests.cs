using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DailyFortune.Application.Services;
using DailyFortune.Application.Interfaces;
using DailyFortune.Domain.Entities;
using System.Linq;

namespace DailyFortune.Tests;

[TestFixture]
public class FortuneSelectionServiceTests
{
    [Test]
    public async Task RandomGeneral_AvoidsHistory_UpToThreeAttempts()
    {
        var weatherMock = new Mock<IWeatherService>();
        weatherMock.Setup(w => w.GetCurrentWeatherAsync()).ReturnsAsync(new WeatherInfo { WeatherType = WeatherType.Sunny });

        var repoMock = new Mock<IFortuneRepository>();
        var fortunes = new List<Fortune> {
            new Fortune { Id = 1, Text = "A" },
            new Fortune { Id = 2, Text = "B" },
            new Fortune { Id = 3, Text = "C" }
        };
        repoMock.Setup(r => r.GetFortunes()).Returns(fortunes);
        repoMock.Setup(r => r.GetWeatherFortunes()).Returns(new List<WeatherFortune>());
        repoMock.Setup(r => r.GetSpecialFortunes()).Returns(new List<SpecialFortune>());

        var streakMock = new Mock<IStreakService>();
        // Pretend fortune 1 and 2 have been opened
        streakMock.Setup(s => s.HasFortuneBeenOpened(It.Is<Fortune>(f => f.Id == 1))).Returns(true);
        streakMock.Setup(s => s.HasFortuneBeenOpened(It.Is<Fortune>(f => f.Id == 2))).Returns(true);
        streakMock.Setup(s => s.HasFortuneBeenOpened(It.Is<Fortune>(f => f.Id == 3))).Returns(false);

        var svc = new FortuneSelectionService(weatherMock.Object, repoMock.Object, streakMock.Object);

        var selected = await svc.GetFortuneAsync();

        Assert.That(new[] {1,2,3}.Contains(selected.Id));
        Assert.That(selected.Id, Is.EqualTo(3));
    }
}
