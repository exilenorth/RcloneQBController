using RcloneQBController.Services;
using RcloneQBController.Views;
using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace RcloneQBController
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherUnhandledException += (sender, args) =>
            {
                MessageBox.Show(args.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            };

            var configPath = Path.Combine(AppContext.BaseDirectory, "config.json");
            bool useWizard = true;

            if (File.Exists(configPath))
            {
                try
                {
                    var json = File.ReadAllText(configPath);
                    var config = JsonSerializer.Deserialize<Models.AppConfig>(json);
                    var configService = new ConfigurationService();
                    if (config != null && configService.IsValid(config))
                    {
                        useWizard = false;
                    }
                }
                catch
                {
                    // Let useWizard remain true
                }
            }

            if (useWizard)
            {
                var wizard = new SetupWizardWindow();
                wizard.DataContext = new RcloneQBController.ViewModels.SetupWizardViewModel();
                if (wizard.ShowDialog() == true)
                {
                    Current.MainWindow = new MainWindow();
                    Current.MainWindow.Show();
                }
                else
                {
                    Shutdown();
                }
            }
            else
            {
                Current.MainWindow = new MainWindow();
                Current.MainWindow.Show();
            }
        }
    }
}