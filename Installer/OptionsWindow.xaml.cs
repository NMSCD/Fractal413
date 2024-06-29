using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Install
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public OptionsWindow()
        {
            InitializeComponent();
        }

        private void btnSetLocation_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtInstallLocation.Text = dialog.SelectedPath;
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            string installLocation = txtInstallLocation.Text;
            bool installVCRuntime = chkInstallVCRuntime.IsChecked == true;

            if (string.IsNullOrEmpty(installLocation) || installLocation == "No folder selected")
            {
                MessageBox.Show("Please select an install location.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Statics.LoginWindow.Show();           
            this.Hide();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            this.Close();
        }
    }
}
