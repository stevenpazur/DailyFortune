using DailyFortune.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyFortune.Application.Interfaces;

public interface IWeatherService
{
    Task<WeatherInfo> GetCurrentWeatherAsync();
}
