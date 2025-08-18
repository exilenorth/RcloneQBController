using System;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace RcloneQBController.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal TaskbarIcon MyNotifyIcon
        {
            get { return (TaskbarIcon)FindName("MyNotifyIcon"); }
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new RcloneQBController.ViewModels.MainViewModel(
                new RcloneQBController.Services.ConfigurationService(),
                new RcloneQBController.Services.CredentialService(),
                new RcloneQBController.Services.ScriptGenerationService(new RcloneQBController.Services.ConfigurationService(), new RcloneQBController.Services.CredentialService()),
                new RcloneQBController.Services.ScriptRunnerService(new RcloneQBController.Services.UserNotifierService(), new RcloneQBController.Services.NotificationService()),
                new RcloneQBController.Services.UserNotifierService(),
                new RcloneQBController.Services.NotificationService()
            );
        }
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                this.Hide();
            }

            base.OnStateChanged(e);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
        }
    }
}