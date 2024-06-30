using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Install
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            txtError.Text = "";
            // Try and Get Steam Username From Registry
            string steamInstallPath = Functions.GetSteamPath();
            if (steamInstallPath != null)
            {
                string steamUsername = Functions.GetSteamUsername(steamInstallPath);
                if(steamUsername != null)
                {
                    txtUsername.Text = steamUsername;
                    txtPassword.Focus();
                    return;
                }
            }
            txtUsername.Focus();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogin_Click(sender, e); // or any method that handles the submit action
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {            
            Functions.DoDownload();
            Windows.WindowClose(this, Windows.ProgressWindow);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Windows.WindowClose(this, Windows.OptionsWindow);
        }

        public void SetErrorText(string errorText)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                boxInfo.Visibility = Visibility.Collapsed;
                txtError.Text = errorText;
            }));
        }


    }
}
