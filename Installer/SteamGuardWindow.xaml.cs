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
            this.Hide();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Statics.LoginWindow.Show();
            this.Hide();
        }
    }
}
