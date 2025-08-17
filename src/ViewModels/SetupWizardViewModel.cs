using RcloneQBController.Models;
using RcloneQBController.Services;
using System;
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
        public event Action<bool> RequestClose;

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
                if (CurrentStepViewModel is SeedboxConnectionViewModel seedboxVM && _steps[_currentStepIndex + 1] is TransferJobViewModel transferJobVM)
                {
                    transferJobVM.UpdateDefaultPaths(seedboxVM.Username);
                }

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

                        if (CurrentStepViewModel is IDataErrorInfo vm && !string.IsNullOrEmpty(vm.Error))
                        {
                            return false;
                        }
            
                        if (CurrentStepViewModel is RcloneInstallViewModel rcloneVM)
                        {
                            return rcloneVM.UserConfirmedInstall;
                        }
                        if (CurrentStepViewModel is TransferJobViewModel transferJobVM)
                        {
                            return transferJobVM.Jobs != null && transferJobVM.Jobs.Any();
                        }
            
                        return true;
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
            if (parameter is Func<string> passwordAccessor)
            {
                var rcloneInstallVM = _steps.OfType<RcloneInstallViewModel>().First();
                var seedboxConnectionVM = _steps.OfType<SeedboxConnectionViewModel>().First();
                var transferJobVM = _steps.OfType<TransferJobViewModel>().First();
                var openVPNVM = _steps.OfType<OpenVPNViewModel>().First();
                var qbittorrentVM = _steps.OfType<QbittorrentViewModel>().First();

                var password = passwordAccessor();
                // In a real app, you'd use this password to call rclone obscure
                // and then store the obscured password.

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
                    },
                    AppSettings = new AppSettings { LogRetentionDays = 14 },
                    Cleanup = new CleanupScriptConfig { TargetRatio = 2.0 },
                    Schedule = new SchedulerConfig { PullEveryMinutes = 15 }
                };

                ConfigurationService.Instance.SaveConfig(config);
                var scriptService = new ScriptGenerationService(ConfigurationService.Instance);
                try
                {
                    scriptService.GenerateScripts(config);
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    System.Windows.MessageBox.Show($"A script template file is missing and the scripts could not be generated.\n\nDetails: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    RequestClose?.Invoke(false);
                    return;
                }

                RequestClose?.Invoke(true);
            }
        }

                        private bool CanFinish(object? parameter)
                        {
                            return _steps.OfType<IDataErrorInfo>().All(vm => string.IsNullOrEmpty(vm.Error));
                        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}