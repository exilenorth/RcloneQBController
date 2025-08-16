using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RcloneQBController.ViewModels
{
    public class RcloneSummaryViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<KeyValuePair<string, string>>? ConfigurationSummary { get; set; }
        public ICommand TestConnectionCommand { get; }

        public RcloneSummaryViewModel()
        {
            ConfigurationSummary = new ObservableCollection<KeyValuePair<string, string>>();
            TestConnectionCommand = new RelayCommand(TestConnection);
        }

        public void UpdateSummary(object rcloneConfig, object seedboxConfig, object jobs)
        {
            ConfigurationSummary?.Clear();
            // Populate summary from config objects
        }

        private void TestConnection(object parameter)
        {
            // Logic to run rclone lsd
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}