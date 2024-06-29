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
    public partial class ProgressWindow : Window
    {
        public ProgressWindow()
        {
            InitializeComponent();
        }

        public void SetProgress(string text, float progress = 0)
        {
            bool isindeterminate = false;
            if(progress == 0)
            {
                progress = 20;
                isindeterminate = true;
            }
            Dispatcher.BeginInvoke(new Action(delegate
            {
                Statics.ProgressWindow.txtStatus.Text = text;
                Statics.ProgressWindow.pbProgressBar.IsIndeterminate = isindeterminate;
                Statics.ProgressWindow.pbProgressBar.Value = progress;
            }));
        }

    }
}
