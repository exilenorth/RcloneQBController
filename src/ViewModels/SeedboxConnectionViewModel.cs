using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace RcloneQBController.ViewModels
{
    public class SeedboxConnectionViewModel : INotifyPropertyChanged
    {
        private string? _host;
        private int _port = 22;
        private string? _username;
        private string _remoteName = "seedbox";

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

        public string RemoteName
        {
            get => _remoteName;
            set { _remoteName = value; OnPropertyChanged(); }
        }

        public ICommand TestConnectionCommand { get; }

        public SeedboxConnectionViewModel()
        {
            TestConnectionCommand = new RelayCommand(TestConnection);
        }

        private void TestConnection(object parameter)
        {
            if (parameter is PasswordBox passwordBox)
            {
                var password = passwordBox.Password;
                // Logic to run rclone obscure and rclone lsd
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}