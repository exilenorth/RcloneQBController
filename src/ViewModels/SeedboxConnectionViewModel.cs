using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace RcloneQBController.ViewModels
{
    public class SeedboxConnectionViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private string? _host;
        private int _port = 22;
        private string? _username;
        private string _remoteName = "seedbox";
        private bool _isTestingConnection;
        private string? _sourcePath;
        private bool _sourcePathManuallySet = false;
 
        public bool IsTestingConnection
        {
            get => _isTestingConnection;
            set { _isTestingConnection = value; OnPropertyChanged(); }
        }
 
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
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                    UpdateSourcePath();
                }
            }
        }
 
        public string? SourcePath
        {
            get => _sourcePath;
            set
            {
                if (_sourcePath != value)
                {
                    _sourcePath = value;
                    _sourcePathManuallySet = true;
                    OnPropertyChanged();
                }
            }
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

        private async void TestConnection(object parameter)
        {
            if (parameter is PasswordBox passwordBox)
            {
                var password = passwordBox.Password;
                IsTestingConnection = true;
                try
                {
                    // Logic to run rclone obscure and rclone lsd
                    await System.Threading.Tasks.Task.Delay(2000); // Simulate network delay
                }
                finally
                {
                    IsTestingConnection = false;
                }
            }
        }
 
        private void UpdateSourcePath()
        {
            if (!_sourcePathManuallySet && !string.IsNullOrWhiteSpace(Username))
            {
                SourcePath = $"/home/{Username}/torrents/";
                _sourcePathManuallySet = false; // Reset flag after programmatic change
            }
        }
 
        public event PropertyChangedEventHandler? PropertyChanged;
 
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // IDataErrorInfo Implementation
        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                string? result = null;
                if (columnName == nameof(Host))
                {
                    if (string.IsNullOrWhiteSpace(Host))
                        result = "Host cannot be empty.";
                }
                else if (columnName == nameof(Port))
                {
                    if (Port <= 0 || Port > 65535)
                        result = "Port must be between 1 and 65535.";
                }
                else if (columnName == nameof(Username))
                {
                    if (string.IsNullOrWhiteSpace(Username))
                        result = "Username cannot be empty.";
                }
                return result ?? string.Empty;
            }
        }
    }
}