using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows;

namespace Install
{
    public static class Functions
    {
        public static void DoDownload()
        {
            var folder = Statics.OptionsWindow.txtInstallLocation.Text;
            string arguments = string.Format("-app 275850 -depot 275851 -manifest 3961348687487426672 -dir \"{0}\" -username {1} -password {2}", folder, Statics.LoginWindow.txtUsername.Text, Statics.LoginWindow.txtPassword.Password);
            Statics.ProgressWindow.Show();
            Download.DownloadActions.DoDownload(folder, Statics.LoginWindow.txtUsername.Text, Statics.LoginWindow.txtPassword.Password, DoDownloadCallback);
        }

        public static void DoDownloadCallback(Download.DownloadActions.DownloadActionType action, string message, float percentage)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {                
                message = message.Trim();
                Statics.ProgressWindow.SetProgress(message, percentage);

                if (action == Download.DownloadActions.DownloadActionType.Complete)
                {
                    string binariesFolder = Path.Combine(Statics.OptionsWindow.txtInstallLocation.Text, "Binaries");
                    string keyFile = Path.Combine(binariesFolder, "NMS.exe");
                    Restore.RestoreActions.DoRestore(binariesFolder, keyFile, DoRestoreCallback);                    
                    return;
                }
                else if (action == Download.DownloadActions.DownloadActionType.Input)
                {

                    Statics.SteamGuardWindow.Show();
                    Statics.SteamGuardWindow.txtMessage.Text = message;
                    Statics.SteamGuardWindow.OnSteamGuardCodeEntered = code =>
                    {
                        Download.DownloadActions.SendResponse(code);
                    };
                }
                else if (action == Download.DownloadActions.DownloadActionType.Error)
                {
                    Statics.ProgressWindow.Hide();
                    Statics.LoginWindow.Show();
                    Statics.LoginWindow.SetErrorText(message);
                }
            });
        }

        public static void DoRestoreCallback(Restore.RestoreActions.RestoreActionType action, string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                message = message.Trim();
                Statics.ProgressWindow.SetProgress(message);

                if (action == Restore.RestoreActions.RestoreActionType.Complete)
                {
                    InstallVCRedist();
                    return;
                }                
            });
        }

        public static void InstallVCRedist()
        {
            if (Statics.OptionsWindow.chkInstallVCRuntime.IsChecked == true)
            {
                Statics.ProgressWindow.SetProgress("Installing Visual C++ Runtime...");
                new Thread(() =>
                {
                    try
                    {
                        string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "3rdParty", "VC_redist.x64.exe");
                        Process installerProcess = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = exePath,
                                UseShellExecute = false,
                                CreateNoWindow = true
                            }
                        };
                        installerProcess.Start();
                        installerProcess.WaitForExit();
                    }
                    catch (Exception ex)
                    {
                        Statics.ProgressWindow.SetProgress("Error launching installer: " + ex.Message);
                        Thread.Sleep(3000);
                    }

                    FinalSetup();
                }).Start();
            }
            else
            {
                FinalSetup();
            }
        }

        public static void FinalSetup()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                bool createDesktopIcons = (Statics.OptionsWindow.chkCreateShortcut.IsChecked == true);                
                string installFolder = Statics.OptionsWindow.txtInstallLocation.Text;
                string binariesFolder = Path.Combine(installFolder, "Binaries\\");
                string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                var paths = new List<string>() { binariesFolder, installFolder };
                if (createDesktopIcons)
                    paths.Add(desktopFolder);

                ShortcutManager.CreateShortcut(
                    shortcutPaths: paths,
                    shortcutName: "No Man's Sky Fractal 4.13.lnk",
                    targetPath: Path.Combine(binariesFolder, "NMS.exe"),
                    iconLocation: Path.Combine(binariesFolder, "_icon.ico")) ;

                ShortcutManager.CreateShortcut(
                    shortcutPaths: paths,
                    shortcutName: "No Man's Sky Fractal 4.13 (Debug Version).lnk",
                    targetPath: Path.Combine(binariesFolder, "_RunAsDate.exe"),
                    iconLocation: Path.Combine(binariesFolder, "_icondebug.ico"),
                    arguments: "/movetime 01\\01\\2022 \"" + Path.Combine(binariesFolder, "XGOG Release_x64.exe") + "\"");            

                Statics.ProgressWindow.Hide();
                (new CompleteWindow()).Show();
            });
        }


        public static string GetSteamPath()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("SteamPath");
                        if (o != null)
                        {
                            return o.ToString().Replace('/', '\\');
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error accessing registry: " + ex.Message);
            }
            return null;
        }

        public static string GetSteamUsername(string steamPath)
        {
            string configPath = Path.Combine(steamPath, "config", "loginusers.vdf");
            try
            {
                if (File.Exists(configPath))
                {
                    string[] lines = File.ReadAllLines(configPath);
                    foreach (string line in lines)
                    {
                        if (line.Contains("\"AccountName\""))
                        {
                            int startIndex = line.IndexOf("\"AccountName\"") + 14;
                            int endIndex = line.LastIndexOf("\"");
                            if (endIndex > startIndex)
                            {
                                string result = line.Substring(startIndex, endIndex - startIndex);
                                result = result.Replace("\"", "");
                                result = result.Trim();
                                return result;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
            }
            return null;
        }
    }
}
