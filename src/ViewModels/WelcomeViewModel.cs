using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RcloneQBController.ViewModels
{
    public class WelcomeViewModel : INotifyPropertyChanged
    {
        public string Title { get; } = "Welcome to Rclone QB Controller";
        public string Description { get; } = "This application helps you automate file transfers from your seedbox. It will download your completed files and clean up the torrents when they are finished seeding.";
        public string SetupOverview { get; } = "This wizard will guide you through the setup process. Click 'Next' to get started.";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}