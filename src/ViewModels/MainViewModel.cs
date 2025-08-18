using System.Collections.ObjectModel;
using System.Windows.Input;
using RcloneQBController.Models;
using RcloneQBController.Services;
using System.Windows;

namespace RcloneQBController.ViewModels
{
    public class MainViewModel
    {
        public ActivityDashboardViewModel ActivityDashboard { get; }
        public QbittorrentViewModel Qbittorrent { get; }
        public ObservableCollection<RcloneJobConfig> Jobs { get; }
        public ICommand RunJobCommand { get; }
        public ICommand StopJobCommand { get; }
        public ICommand PreviewCommand { get; }
        public ICommand ToggleScheduledJobCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand OpenUserGuideCommand { get; }
        public ICommand OpenLogFolderCommand { get; }
        public ICommand ExitCommand { get; }
       public ICommand ShowWindowCommand { get; }

       private readonly IScriptRunnerService _scriptRunner;
        private readonly IConfigurationService _configurationService;
        private readonly ICredentialService _credentialService;
        private readonly IScriptGenerationService _scriptGenerationService;
        private readonly SchedulingService _schedulingService;
        private AppConfig _config = new AppConfig();

        public MainViewModel(IConfigurationService configurationService, ICredentialService credentialService, IScriptGenerationService scriptGenerationService, IScriptRunnerService scriptRunnerService, IUserNotifierService userNotifierService, INotificationService notificationService)
        {
            _configurationService = configurationService;
            _credentialService = credentialService;
            _scriptGenerationService = scriptGenerationService;
            _scriptRunner = scriptRunnerService;
            _schedulingService = new SchedulingService(_scriptRunner);
            ActivityDashboard = new ActivityDashboardViewModel();
            Qbittorrent = new QbittorrentViewModel(_scriptRunner, ActivityDashboard);

            _config = _configurationService.LoadConfig() ?? new AppConfig();
            if (_config != null && _config.Rclone != null && _config.Rclone.Jobs != null)
            {
                Jobs = new ObservableCollection<RcloneJobConfig>(_config.Rclone.Jobs);
            }
            else
            {
                Jobs = new ObservableCollection<RcloneJobConfig>();
            }


            RunJobCommand = new RelayCommand(async (job) =>
            {
                if (job is RcloneJobConfig rcloneJob)
                {
                    rcloneJob.IsRunning = true;
                    ActivityDashboard.FileTransfers.Clear();
                    await _scriptRunner.RunRcloneJobAsync(rcloneJob, (output) =>
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            ActivityDashboard.ParseOutput(output);
                        });
                    });
                    rcloneJob.IsRunning = false;
                    ActivityDashboard.FileTransfers.Clear();
                }
            });

            StopJobCommand = new RelayCommand((job) =>
            {
                if (job is RcloneJobConfig rcloneJob)
                {
                    if (rcloneJob?.Name != null)
                    {
                        _scriptRunner.StopJob(rcloneJob.Name);
                    }
                }
            });

            PreviewCommand = new RelayCommand((job) =>
            {
                if (job is RcloneJobConfig rcloneJob)
                {
                                        try
                                        {
                                            var command = _scriptGenerationService.GetPreviewCommand(rcloneJob);
                                            MessageBox.Show(command, "Preview Command");
                                        }
                                        catch (System.IO.FileNotFoundException ex)
                                        {
                                            MessageBox.Show($"Could not generate preview.\n\nDetails: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                        }
                }
            });

            ToggleScheduledJobCommand = new RelayCommand((job) =>
            {
                if (job is RcloneJobConfig rcloneJob)
                {
                    if (rcloneJob.IsScheduled)
                    {
                        var minutes = _config?.Schedule?.PullEveryMinutes > 0
                            ? _config!.Schedule!.PullEveryMinutes
                            : 15; // sensible default

                        _schedulingService.Start(rcloneJob, System.TimeSpan.FromMinutes(minutes));
                    }
                    else
                    {
                        _schedulingService.Stop(rcloneJob);
                    }
                    if (_config != null)
                    {
                        _configurationService.SaveConfig(_config);
                    }
                }
            });

            OpenSettingsCommand = new RelayCommand(_ => OpenSettings());
            OpenUserGuideCommand = new RelayCommand(_ => OpenUserGuide());
            OpenLogFolderCommand = new RelayCommand(_ => OpenLogFolder());
            ExitCommand = new RelayCommand(_ => ExitApplication());
           ShowWindowCommand = new RelayCommand(_ => ShowWindow());
       }
       private void ShowWindow()
       {
           Application.Current.MainWindow.Show();
           Application.Current.MainWindow.WindowState = WindowState.Normal;
       }

       private void OpenUserGuide()
        {
            var userGuideWindow = new RcloneQBController.Views.UserGuideWindow();
            userGuideWindow.Show();
        }

        private void OpenLogFolder()
        {
            string logPath = _config?.Rclone?.LogDir;

            if (!string.IsNullOrEmpty(logPath))
            {
                string fullPath = System.IO.Path.GetFullPath(logPath);
                if (System.IO.Directory.Exists(fullPath))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(fullPath)
                    {
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show($"Log folder not found at: {fullPath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Log folder path is not configured in settings.", "Configuration Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExitApplication()
        {
           (Application.Current.MainWindow as RcloneQBController.Views.MainWindow)?.MyNotifyIcon.Dispose();
           Application.Current.Shutdown();
       }

        private void OpenSettings()
        {
            var settingsWindow = new RcloneQBController.Views.SettingsWindow();
            var result = settingsWindow.ShowDialog();
            if (result == true)
            {
                _config = _configurationService.LoadConfig();
                Jobs.Clear();
                if (_config != null && _config.Rclone != null && _config.Rclone.Jobs != null)
                {
                    foreach (var job in _config.Rclone.Jobs)
                    {
                        Jobs.Add(job);
                    }
                }
            }
        }
    }
}