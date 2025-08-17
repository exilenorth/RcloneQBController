using RcloneQBController.Views;
using System.IO;
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

            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
            if (!File.Exists(configPath))
            {
                                var wizard = new SetupWizardWindow();
                                wizard.DataContext = new RcloneQBController.ViewModels.SetupWizardViewModel();
                                if (wizard.ShowDialog() == true)
                                {
                                    var mainWindow = new MainWindow();
                                    mainWindow.Show();
                                }
                                else
                                {
                                    Shutdown();
                                }
                            }
                            else
                            {
                                var mainWindow = new MainWindow();
                                mainWindow.Show();
                            }
        }
    }
}