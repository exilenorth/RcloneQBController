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
        public ObservableCollection<RcloneJobConfig> Jobs { get; }
        public ICommand RunJobCommand { get; }
        public ICommand PreviewCommand { get; }

        private readonly ScriptRunnerService _scriptRunner;
        private readonly ConfigurationService _configurationService;
        private readonly ScriptGenerationService _scriptGenerationService;

        public MainViewModel()
        {
            ActivityDashboard = new ActivityDashboardViewModel();
            _scriptRunner = new ScriptRunnerService();
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
                    await _scriptRunner.RunRcloneJobAsync(rcloneJob, (output) =>
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            ActivityDashboard.ParseOutput(output);
                        });
                    });
                }
            });

            PreviewCommand = new RelayCommand((job) =>
            {
                if (job is RcloneJobConfig rcloneJob)
                {
                    var command = _scriptGenerationService.GetPreviewCommand(rcloneJob);
                    MessageBox.Show(command, "Preview Command");
                }
            });
        }
    }
}