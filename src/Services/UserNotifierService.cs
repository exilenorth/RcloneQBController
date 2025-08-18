using System;
using System.IO;
using System.Windows;

namespace RcloneQBController.Services
{
    public static class UserNotifierService
    {
        public static void ShowFriendlyError(Exception ex)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                string message = GetFriendlyMessage(ex);
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }

        private static string GetFriendlyMessage(Exception ex)
        {
            switch (ex)
            {
                case FileNotFoundException fnfe when fnfe.FileName != null && fnfe.FileName.Contains("rclone.exe"):
                    return "rclone.exe not found. Please ensure rclone is installed and its location is correctly configured in the settings.";
                case DirectoryNotFoundException _:
                    return "A configured directory could not be found. Please check your path settings.";
                case InvalidOperationException ioe when ioe.Message.Contains("unresolved placeholder"):
                    return $"A configuration error was detected: {ioe.Message}. Please review your script and application settings.";
                default:
                    return $"An unexpected error occurred: {ex.Message}\n\nPlease check the logs for more details.";
            }
        }
    }
}