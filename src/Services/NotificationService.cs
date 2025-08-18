using Microsoft.Toolkit.Uwp.Notifications;

namespace RcloneQBController.Services
{
    public static class NotificationService
    {
        public static void ShowNotification(string title, string message)
        {
            new ToastContentBuilder()
                .AddText(title)
                .AddText(message)
                .Show();
        }
    }
}