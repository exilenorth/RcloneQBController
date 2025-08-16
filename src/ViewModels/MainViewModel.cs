using System.Collections.ObjectModel;
using System.Windows.Input;
using RcloneQBController.Models;
using RcloneQBController.Services;

namespace RcloneQBController.ViewModels
{
    public class MainViewModel
    {
        public ActivityDashboardViewModel ActivityDashboard { get; }
        public ObservableCollection<RcloneJobConfig> Jobs { get; }
        public ICommand RunJobCommand { get; }

        private readonly ScriptRunnerService _scriptRunner;
        private readonly ConfigurationService _configurationService;

        public MainViewModel()
        {
            ActivityDashboard = new ActivityDashboardViewModel();
            _scriptRunner = new ScriptRunnerService();
            _configurationService = ConfigurationService.Instance;

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
                    await _scriptRunner.RunScriptAsync(rcloneJob, (output) =>
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            ActivityDashboard.ParseOutput(output);
                        });
                    });
                }
            });
        }
    }
}