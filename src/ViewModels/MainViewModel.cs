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

        private readonly ScriptRunnerService _scriptRunner;
        private readonly ConfigurationService _configurationService;
        private readonly ScriptGenerationService _scriptGenerationService;
        private readonly SchedulingService _schedulingService;
        private readonly AppConfig _config;

        public MainViewModel()
        {
            ActivityDashboard = new ActivityDashboardViewModel();
            _scriptRunner = new ScriptRunnerService();
            _schedulingService = new SchedulingService(_scriptRunner);
            Qbittorrent = new QbittorrentViewModel(_scriptRunner, ActivityDashboard);
            _configurationService = ConfigurationService.Instance;
            _scriptGenerationService = new ScriptGenerationService(_configurationService);

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
                    _scriptRunner.StopJob(rcloneJob.Name);
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
                    _configurationService.SaveConfig(_config);
                }
            });
        }
    }
}