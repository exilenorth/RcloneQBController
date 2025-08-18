using Microsoft.Toolkit.Uwp.Notifications;

namespace RcloneQBController.Services
{
    public class NotificationService : INotificationService
    {
        public void ShowNotification(string title, string message)
        {
            new ToastContentBuilder()
                .AddText(title)
                .AddText(message)
                .Show();
        }
    }
}