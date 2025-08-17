using System.Windows;

namespace RcloneQBController.Views
{
    /// <summary>
    /// Interaction logic for SetupWizardWindow.xaml
    /// </summary>
    public partial class SetupWizardWindow : Window
    {
        public SetupWizardWindow()
        {
                        InitializeComponent();
                        DataContextChanged += (s, e) =>
                        {
                            if (e.NewValue is ViewModels.SetupWizardViewModel vm)
                            {
                                vm.RequestClose += (success) =>
                                {
                                    DialogResult = success;
                                    Close();
                                };
                            }
                        };
        }
    }
}