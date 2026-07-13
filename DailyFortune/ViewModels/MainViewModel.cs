using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyFortune.Application.Services;
using DailyFortune.Domain.Entities;
using System.Threading.Tasks;

namespace DailyFortune.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public FortuneCookieViewModel Cookie { get; }

    public MainViewModel(
        FortuneCookieViewModel cookie)
    {
        Cookie = cookie;
    }
}