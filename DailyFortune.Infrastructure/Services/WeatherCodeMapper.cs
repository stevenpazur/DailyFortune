using DailyFortune.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyFortune.Infrastructure.Services;

public class WeatherCodeMapper
{
    public WeatherType Map(string condition)
    {
        return condition switch
        {
            "Clear" => WeatherType.Sunny,

            "Clouds" => WeatherType.Cloudy,

            "Rain" => WeatherType.Rain,

            "Drizzle" => WeatherType.Rain,

            "Thunderstorm" => WeatherType.Storm,

            "Snow" => WeatherType.Snow,

            "Mist" => WeatherType.Fog,

            "Fog" => WeatherType.Fog,

            _ => WeatherType.Unknown
        };
    }
}
