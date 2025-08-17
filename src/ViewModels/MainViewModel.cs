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

        private readonly ScriptRunnerService _scriptRunner;
        private readonly ConfigurationService _configurationService;
        private readonly ScriptGenerationService _scriptGenerationService;

        public MainViewModel()
        {
            ActivityDashboard = new ActivityDashboardViewModel();
            _scriptRunner = new ScriptRunnerService();
            Qbittorrent = new QbittorrentViewModel(_scriptRunner, ActivityDashboard);
            _configurationService = ConfigurationService.Instance;
            _scriptGenerationService = new ScriptGenerationService(_configurationService);

            var config = _configurationService.LoadConfig();
            if (config != null && config.Rclone != null && config.Rclone.Jobs != null)
            {
                Jobs = new ObservableCollection<RcloneJobConfig>(config.Rclone.Jobs);
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
        }
    }
}