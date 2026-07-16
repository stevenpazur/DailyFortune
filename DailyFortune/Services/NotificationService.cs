using DailyFortune.Application.Interfaces;
//using H.NotifyIcon;
//using H.NotifyIcon.Core;

namespace DailyFortune.Services;

public sealed class NotificationService : INotificationService
{
    //private readonly TrayIcon _trayIcon;

    public NotificationService(
        //TrayIcon taskbarIcon
        )
    {
        //_trayIcon = taskbarIcon;
    }

    public void ShowNotification(string title, string message)
    {
        //_trayIcon.ShowNotification(
            //title,
            //message,
            //NotificationIcon.Info);
    }

    public void SetIcon(string iconPath)
    {
        //_trayIcon.Icon = new System.Drawing.Icon(iconPath);
    }
}