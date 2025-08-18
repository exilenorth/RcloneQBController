using System.Windows;

namespace RcloneQBController.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            var viewModel = new RcloneQBController.ViewModels.SettingsViewModel();
            DataContext = viewModel;
            viewModel.CloseAction = (result) =>
            {
                DialogResult = result;
                Close();
            };
        }
    }
}