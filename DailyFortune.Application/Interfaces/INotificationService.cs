namespace DailyFortune.Application.Interfaces;

public interface INotificationService
{
    void ShowNotification(string title, string message);
    void SetIcon(string iconPath);
}
