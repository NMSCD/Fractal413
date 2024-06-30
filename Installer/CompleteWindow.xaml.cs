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
        public CompleteWindow()
        {
            InitializeComponent();           
        }
     
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            this.Close();
        }

        private void StartNMSFractal_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.Combine(Windows.OptionsWindow.txtInstallLocation.Text, "Binaries\\NMS.exe");
            Process.Start(path);
        }

        private void StartNMSFractalDebug_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.Combine(Windows.OptionsWindow.txtInstallLocation.Text, "Binaries\\No Man's Sky Fractal 4.13 (Debug Version).lnk");
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
            startInfo.Arguments = Windows.OptionsWindow.txtInstallLocation.Text;

            // Start the process.
            Process.Start(startInfo);
        }

        private void OpenWeb_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/NMSCD/Fractal413");
        }
    }
}
