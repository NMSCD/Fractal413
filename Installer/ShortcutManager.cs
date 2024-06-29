using System;
using System.Collections.Generic;
using System.IO;
using IWshRuntimeLibrary;
using System.Runtime.InteropServices;
public class ShortcutManager
{
    public static void CreateShortcut(
    List<string> shortcutPaths,
    string shortcutName,
    string targetPath,
    string arguments = null,
    string[] hotKey = null,
    string workingDirectory = null,
    string description = null,
    string iconLocation = null,
    string windowStyle = "Default",
    bool runAsAdmin = false)
    {
        foreach (var shortcutPath in shortcutPaths)
        {
            try
            {
                string shortcutFullPath = Path.Combine(shortcutPath, shortcutName);
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutFullPath);

                shortcut.TargetPath = targetPath;

                switch (windowStyle)
                {
                    case "Maximized":
                        shortcut.WindowStyle = 3;
                        break;
                    case "Minimized":
                        shortcut.WindowStyle = 7;
                        break;
                    default:
                        shortcut.WindowStyle = 1;
                        break;
                }

                if (arguments != null)
                {
                    shortcut.Arguments = arguments;
                }

                if (hotKey != null && hotKey.Length > 0)
                {
                    shortcut.Hotkey = string.Join("+", hotKey).ToUpperInvariant();
                }

                if (!string.IsNullOrEmpty(iconLocation))
                {
                    shortcut.IconLocation = iconLocation;
                }

                if (!string.IsNullOrEmpty(description))
                {
                    shortcut.Description = description;
                }

                if (!string.IsNullOrEmpty(workingDirectory))
                {
                    shortcut.WorkingDirectory = workingDirectory;
                }
                else
                {
                    shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
                }

                shortcut.Save();

                if (runAsAdmin)
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(shortcutFullPath);
                    bytes[21] = 0x22; // set byte no. 21 to ASCII value 34
                    System.IO.File.WriteAllBytes(shortcutFullPath, bytes);
                }

                Marshal.ReleaseComObject(shortcut);
                Marshal.ReleaseComObject(shell);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating shortcut: {ex.Message}");
            }
        }
    }
}