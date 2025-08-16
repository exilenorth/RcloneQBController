using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RcloneQBController.ViewModels
{
    public class CleanupSummaryViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<KeyValuePair<string, string>>? ConfigurationSummary { get; set; }
        public ICommand FinalTestCommand { get; }

        public CleanupSummaryViewModel()
        {
            ConfigurationSummary = new ObservableCollection<KeyValuePair<string, string>>();
            FinalTestCommand = new RelayCommand(RunFinalTest);
        }

        public void UpdateSummary(object vpnConfig, object qbConfig)
        {
            ConfigurationSummary?.Clear();
            // Populate summary from config objects
        }

        private void RunFinalTest(object parameter)
        {
            // Logic to run rclone ls and qB API test
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}