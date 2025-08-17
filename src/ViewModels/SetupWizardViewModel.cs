using RcloneQBController.Models;
using RcloneQBController.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RcloneQBController.ViewModels
{
    public class SetupWizardViewModel : INotifyPropertyChanged
    {
        private object? _currentStepViewModel;
        private readonly List<object> _steps;
        private int _currentStepIndex;

        public object CurrentStepViewModel
        {
            get => _currentStepViewModel!;
            set
            {
                _currentStepViewModel = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsFinishButtonVisible));
                OnPropertyChanged(nameof(IsBackButtonVisible));
            }
        }

        public bool IsBackButtonVisible => _currentStepIndex > 0;
        public bool IsFinishButtonVisible => _currentStepIndex == _steps.Count - 1;

        public ICommand NextCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand FinishCommand { get; }

        public SetupWizardViewModel()
        {
            _steps = new List<object>
            {
                new WelcomeViewModel(),
                new RcloneInstallViewModel(),
                new SeedboxConnectionViewModel(),
                new TransferJobViewModel(),
                new RcloneSummaryViewModel(),
                new OpenVPNViewModel(),
                new QbittorrentViewModel(new ScriptRunnerService(), new ActivityDashboardViewModel()),
                new CleanupSummaryViewModel()
            };

            _currentStepIndex = 0;
            CurrentStepViewModel = _steps[_currentStepIndex];

            NextCommand = new RelayCommand(GoToNextStep, CanGoToNextStep);
            BackCommand = new RelayCommand(GoToPreviousStep, CanGoToPreviousStep);
            FinishCommand = new RelayCommand(FinishWizard, CanFinish);
        }

        private void GoToNextStep(object? parameter)
        {
            if (CanGoToNextStep(parameter))
            {
                _currentStepIndex++;
                CurrentStepViewModel = _steps[_currentStepIndex];
            }
        }

        private bool CanGoToNextStep(object? parameter)
        {
            if (_currentStepIndex >= _steps.Count - 1)
            {
                return false;
            }

            // Add validation logic for each step here
            if (CurrentStepViewModel is RcloneInstallViewModel rcloneVM)
            {
                return rcloneVM.IsRcloneInstalled;
            }
            if (CurrentStepViewModel is SeedboxConnectionViewModel seedboxVM)
            {
                return !string.IsNullOrWhiteSpace(seedboxVM.Host) &&
                       !string.IsNullOrWhiteSpace(seedboxVM.Username) &&
                       seedboxVM.Port > 0;
            }
            // Add more validation as needed for other steps

            return true; // Default to true if no specific validation
        }

        private void GoToPreviousStep(object? parameter)
        {
            if (CanGoToPreviousStep(parameter))
            {
                _currentStepIndex--;
                CurrentStepViewModel = _steps[_currentStepIndex];
            }
        }

        private bool CanGoToPreviousStep(object? parameter) => _currentStepIndex > 0;

        private void FinishWizard(object? parameter)
        {
            var rcloneInstallVM = _steps.OfType<RcloneInstallViewModel>().First();
            var seedboxConnectionVM = _steps.OfType<SeedboxConnectionViewModel>().First();
            var transferJobVM = _steps.OfType<TransferJobViewModel>().First();
            var openVPNVM = _steps.OfType<OpenVPNViewModel>().First();
            var qbittorrentVM = _steps.OfType<QbittorrentViewModel>().First();

            var config = new AppConfig
            {
                Rclone = new RcloneConfig
                {
                    RclonePath = rcloneInstallVM.RclonePath,
                    RemoteName = seedboxConnectionVM.RemoteName,
                    Jobs = new List<RcloneJobConfig>(transferJobVM.Jobs ?? Enumerable.Empty<RcloneJobConfig>())
                },
                Seedbox = new SeedboxConfig
                {
                    Host = seedboxConnectionVM.Host,
                    Port = seedboxConnectionVM.Port,
                    Username = seedboxConnectionVM.Username
                    // Password needs to be handled securely, not stored in ViewModel
                },
                Vpn = new VpnConfig
                {
                    ConfigFile = openVPNVM.OvpnFilePath
                },
                QBittorrent = new QBittorrentConfig
                {
                    Host = qbittorrentVM.Host,
                    Port = qbittorrentVM.Port,
                    Username = qbittorrentVM.Username
                    // Password needs to be handled securely
                },
                // Initialize other config sections as needed
                AppSettings = new AppSettings { LogRetentionDays = 14 },
                Cleanup = new CleanupScriptConfig { TargetRatio = 2.0 },
                Schedule = new SchedulerConfig { PullEveryMinutes = 15 }
            };

            ConfigurationService.Instance.SaveConfig(config);
            var scriptService = new ScriptGenerationService(ConfigurationService.Instance);
            scriptService.GenerateScripts(config);

            // Logic to close the wizard window
            // This is typically handled by the View, e.g., closing the window
            // after the command has executed.
        }

        private bool CanFinish(object? parameter)
        {
            // Add final validation logic here
            return true;
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}