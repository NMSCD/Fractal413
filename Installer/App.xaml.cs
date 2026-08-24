using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Install
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // DepotDownloader 3.4.0 needs the .NET 9.0 runtime - gate startup on it before showing any window.
            if (!DotNetCheck.EnsureDotNet9())
            {
                Shutdown();
                return;
            }
            base.OnStartup(e);
        }
    }
}
