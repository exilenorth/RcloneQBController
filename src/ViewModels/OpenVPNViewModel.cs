using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RcloneQBController.ViewModels
{
    public class OpenVPNViewModel : INotifyPropertyChanged
    {
        private string? _ovpnFilePath;

        public string? OvpnFilePath
        {
            get => _ovpnFilePath;
            set { _ovpnFilePath = value; OnPropertyChanged(); }
        }

        public ICommand BrowseCommand { get; }
        public ICommand TestVpnCommand { get; }

        public OpenVPNViewModel()
        {
            BrowseCommand = new RelayCommand(BrowseForFile);
            TestVpnCommand = new RelayCommand(TestVpnConnection, CanTestVpnConnection);
        }

        private void BrowseForFile(object parameter)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "OpenVPN Configuration|*.ovpn",
                Title = "Select .ovpn File"
            };
            if (dialog.ShowDialog() == true)
            {
                OvpnFilePath = dialog.FileName;
            }
        }

        private void TestVpnConnection(object parameter)
        {
            // Logic to copy file and test connection
        }

        private bool CanTestVpnConnection(object parameter) => !string.IsNullOrEmpty(OvpnFilePath) && File.Exists(OvpnFilePath);

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}