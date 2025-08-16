using System.Windows.Controls;

namespace RcloneQBController.Views
{
    /// <summary>
    /// Interaction logic for OpenVPNView.xaml
    /// </summary>
    public partial class OpenVPNView : UserControl
    {
        public OpenVPNView()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}