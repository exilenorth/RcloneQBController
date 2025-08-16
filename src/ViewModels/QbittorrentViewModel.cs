using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace RcloneQBController.ViewModels
{
    public class QbittorrentViewModel : INotifyPropertyChanged
    {
        private string? _host;
        private int _port = 8080;
        private string? _username;

        public string? Host
        {
            get => _host;
            set { _host = value; OnPropertyChanged(); }
        }

        public int Port
        {
            get => _port;
            set { _port = value; OnPropertyChanged(); }
        }

        public string? Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public ICommand FindVpnIpCommand { get; }
        public ICommand TestConnectionCommand { get; }

        public QbittorrentViewModel()
        {
            FindVpnIpCommand = new RelayCommand(FindVpnIp);
            TestConnectionCommand = new RelayCommand(TestConnection);
        }

        private void FindVpnIp(object parameter)
        {
            // Logic to run ipconfig and parse output
        }

        private void TestConnection(object parameter)
        {
            if (parameter is PasswordBox passwordBox)
            {
                var password = passwordBox.Password;
                // Logic to test qBittorrent API connection
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}