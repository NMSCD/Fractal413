using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Diagnostics;

namespace Install
{
    /// <summary>
    /// Interaction logic for CompleteWindow.xaml
    /// </summary>
    public partial class CompleteWindow : Window
    {
        string installPath;
        public CompleteWindow()
        {
            InitializeComponent();
            installPath = Statics.OptionsWindow.txtInstallLocation.Text;
            try { Statics.LoginWindow.Close(); } catch { }
            try { Statics.ProgressWindow.Close(); } catch { }
            try { Statics.SteamGuardWindow.Close(); } catch { }
            try { Statics.OptionsWindow.Close(); } catch { }
        }
     
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            this.Close();
        }

        private void StartNMSFractal_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.Combine(installPath, "Binaries\\NMS.exe");
            Process.Start(path);
        }

        private void StartNMSFractalDebug_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.Combine(installPath, "Binaries\\No Man's Sky Fractal 4.13 (Debug Version).lnk");
            Process.Start(path);
        }

        private void StartSmartSaveFolder_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("SmartSaveFolder.exe");
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "explorer.exe";
            startInfo.Arguments = installPath;

            // Start the process.
            Process.Start(startInfo);
        }
    }
}
