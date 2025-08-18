using RcloneQBController.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows.Controls;
using System.Windows.Input;

namespace RcloneQBController.ViewModels
{
    public class QbittorrentViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private string? _host;
        private int _port = 8080;
        private string? _username;
        private bool _isDryRunEnabled;
        private bool _isTestingConnection;
        private bool _isRunning;
        private readonly IScriptRunnerService _scriptRunner;
        private readonly ActivityDashboardViewModel _activityDashboard;

        public bool IsTestingConnection
        {
            get => _isTestingConnection;
            set { _isTestingConnection = value; OnPropertyChanged(); }
        }

        public bool IsRunning
        {
            get => _isRunning;
            set { _isRunning = value; OnPropertyChanged(); }
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
            set { _username = value; OnPropertyChanged(); }
        }

        public bool IsDryRunEnabled
        {
            get => _isDryRunEnabled;
            set { _isDryRunEnabled = value; OnPropertyChanged(); }
        }

        public ICommand FindVpnIpCommand { get; }
        public ICommand TestConnectionCommand { get; }
        public ICommand RunCleanupScriptCommand { get; }
        public ICommand StopCleanupScriptCommand { get; }

        public QbittorrentViewModel(IScriptRunnerService scriptRunner, ActivityDashboardViewModel activityDashboard)
        {
            _scriptRunner = scriptRunner;
            _activityDashboard = activityDashboard;
            FindVpnIpCommand = new RelayCommand(FindVpnIp);
            TestConnectionCommand = new RelayCommand(async (param) => await TestConnection(param));
            RunCleanupScriptCommand = new RelayCommand(RunCleanupScript);
            StopCleanupScriptCommand = new RelayCommand(StopCleanupScript);
        }

        private void FindVpnIp(object? parameter)
        {
            // Logic to run ipconfig and parse output
        }

        private async System.Threading.Tasks.Task TestConnection(object? parameter)
        {
            if (parameter is PasswordBox passwordBox)
            {
                var password = passwordBox.SecurePassword;
                IsTestingConnection = true;
                try
                {
                    // Logic to test qBittorrent API connection
                    await System.Threading.Tasks.Task.Delay(2000); // Simulate network delay
                }
                finally
                {
                    IsTestingConnection = false;
                }
            }
        }

        private async void RunCleanupScript(object? parameter)
        {
            IsRunning = true;
            await _scriptRunner.RunCleanupScriptAsync(IsDryRunEnabled, (output) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    _activityDashboard.ParseOutput(output);
                });
            });
            IsRunning = false;
        }

        private void StopCleanupScript(object? parameter)
        {
            _scriptRunner.StopJob("cleanup");
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