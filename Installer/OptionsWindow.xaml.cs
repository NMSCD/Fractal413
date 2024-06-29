using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Install
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        private string baseInstallPath = string.Empty;

        public OptionsWindow()
        {
            InitializeComponent();
        }

        private void btnSetLocation_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                baseInstallPath = dialog.SelectedPath;
                UpdateInstallPath();
            }
        }

        private void chkCreateFractalFolder_Checked(object sender, RoutedEventArgs e)
        {
            UpdateInstallPath();
        }

        private void chkCreateFractalFolder_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateInstallPath();
        }

        private void UpdateInstallPath()
        {
            if (string.IsNullOrEmpty(baseInstallPath))
            {
                txtInstallLocation.Text = "No folder selected";
                return;
            }

            if (chkCreateFractalFolder.IsChecked == true)
            {
                txtInstallLocation.Text = Path.Combine(baseInstallPath, "Fractal413");
            }
            else
            {
                txtInstallLocation.Text = baseInstallPath;
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
