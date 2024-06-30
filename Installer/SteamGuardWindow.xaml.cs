using System;
using System.Windows;

namespace Install
{
    public partial class SteamGuardWindow : Window
    {
        public Action<string> OnSteamGuardCodeEntered;

        public SteamGuardWindow()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            OnSteamGuardCodeEntered?.Invoke(txtSteamGuard.Text);
            Windows.WindowClose(this);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Download.DownloadActions.TerminateProcess();
            Windows.ProgressWindow.Hide();
            Windows.LoginWindow.Show();            
        }
    }
}
