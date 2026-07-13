using System;
using System.Collections.Generic;
using System.Text;

namespace DailyFortune.Domain.Entities;

public class WeatherInfo
{
    public WeatherType WeatherType { get; init; }

    public string City { get; init; } = "";

    public double TemperatureCelsius { get; init; }

    public string Description { get; init; } = "";
}
