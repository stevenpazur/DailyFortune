using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyFortune.Application.Services;
using DailyFortune.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DailyFortune.ViewModels
{
    public partial class FortuneCookieViewModel : ObservableObject
    {
        private readonly FortuneService _fortuneService;

        [ObservableProperty]
        private Fortune currentFortune = new();

        [ObservableProperty]
        private bool isOpening;

        [ObservableProperty]
        private bool isOpened;

        [ObservableProperty]
        private int currentFrame;

        [ObservableProperty]
        private bool canOpen = true;

        public FortuneCookieViewModel(
            FortuneService fortuneService)
        {
            _fortuneService = fortuneService;
        }

        [RelayCommand]
        private async Task OpenCookie()
        {
            var todaysFortune = await _fortuneService.GetFortuneAsync();
            CurrentFortune = todaysFortune;
        }
    }
}
