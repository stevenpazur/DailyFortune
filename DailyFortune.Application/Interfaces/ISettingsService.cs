using DailyFortune.Domain.Entities;

namespace DailyFortune.Application.Interfaces
{
    public interface ISettingsService
    {
        public AppSettings GetSettings();
        public void UpdateSettings(AppSettings settings);
    }
}
