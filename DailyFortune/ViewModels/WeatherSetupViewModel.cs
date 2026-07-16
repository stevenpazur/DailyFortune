using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyFortune.Application.Interfaces;
using System.Threading.Tasks;

namespace DailyFortune.ViewModels;

public partial class WeatherSetupViewModel : ObservableObject
{
    private readonly ILocationService _locationService;
    private readonly IStreakService _streakService;

    [ObservableProperty]
    private string city = "";

    [ObservableProperty]
    private string regionCode = "";

    [ObservableProperty]
    private string countryCode = "";

    [ObservableProperty]
    private double latitude = 0;

    [ObservableProperty]
    private double longitude = 0;

    [ObservableProperty]
    private bool canContinue = false;

    public WeatherSetupViewModel(ILocationService locationService, IStreakService streakService)
    {
        _locationService = locationService;
        _streakService = streakService;
    }

    partial void OnLatitudeChanged(double value)
    {
        if (value != 0 && Longitude != 0)
        {
            CanContinue = true;
        }
    }

    partial void OnLongitudeChanged(double value)
    {
        if (value != 0 && Latitude != 0)
        {
            CanContinue = true;
        }
    }

    [RelayCommand]
    private async Task UseCurrentLocation()
    {
        try
        {
            var coords = await _locationService.GetIpLocation();
            if (!string.IsNullOrWhiteSpace(coords.City))
            {
                City = coords.City;
            }
            if (!string.IsNullOrWhiteSpace(coords.RegionCode))
            {
                RegionCode = coords.RegionCode;
            }
            if (!string.IsNullOrWhiteSpace(coords.CountryCode))
            {
                CountryCode = coords.CountryCode;
            }
            if (coords.Latitude != 0)
            {
                Latitude = coords.Latitude;
            }
            if (coords.Longitude != 0)
            {
                Longitude = coords.Longitude;
            }
        }
        catch
        {
            // ignore failures; caller may show an error in UI later
        }
    }

    [RelayCommand]
    private async Task GetLocationByCityStateCountry()
    {
        try
        {
            var coords = await _locationService.GetLocationByCityStateCountryAsync(City, RegionCode, CountryCode);
            if (coords != null)
            {
                City = coords.City;
                RegionCode = coords.RegionCode;
                CountryCode = coords.CountryCode;
                Latitude = coords.Latitude;
                Longitude = coords.Longitude;
            }
        }
        catch
        {

        }
    }

    [RelayCommand]
    private void Continue()
    {
        if (!string.IsNullOrWhiteSpace(City))
        {
            _streakService.SaveLocation(City, RegionCode, CountryCode, Latitude, Longitude);
        }
    }
}
