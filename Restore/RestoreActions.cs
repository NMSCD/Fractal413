using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Restore
{
    public static class RestoreActions
    {
        public enum RestoreActionType
        {
            Status,
            Error,
            Complete
        }

        public static void DoRestore(string targetDir, string keyfilePath, Action<RestoreActionType, string> callback)
        {
            try
            {
                // Step 1: Copy all files from "Files" folder to targetDir
                string sourceFilesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");
                if (Directory.Exists(sourceFilesDir))
                {
                    CopyFiles(sourceFilesDir, targetDir, callback);
                }
                else
                {
                    callback(RestoreActionType.Error, $"Source directory '{sourceFilesDir}' does not exist.");
                    return;
                }

                // Step 2: Rename the keyfile
                string renamedKeyfilePath = RenameKeyfile(keyfilePath, callback);

                // Step 3: Apply patches using xdelta3
                string patchesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Patches");
                if (Directory.Exists(patchesDir))
                {
                    Thread patchThread = new Thread(() => ApplyPatches(patchesDir, renamedKeyfilePath, callback));
                    patchThread.Start();
                }
                else
                {
                    callback(RestoreActionType.Error, $"Patches directory '{patchesDir}' does not exist.");
                    return;
                }
            }
            catch (Exception ex)
            {
                callback(RestoreActionType.Error, $"An error occurred: {ex.Message}");
            }
        }

        private static void CopyFiles(string sourceDir, string targetDir, Action<RestoreActionType, string> callback)
        {
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                string targetFilePath = Path.Combine(targetDir, Path.GetFileName(file));
                try
                {
                    File.Copy(file, targetFilePath, true);
                    callback(RestoreActionType.Status, $"Copied {Path.GetFileName(file)}");
                }
                catch (Exception ex)
                {
                    callback(RestoreActionType.Error, $"Failed to copy {Path.GetFileName(file)}: {ex.Message}");
                }
            }
        }

        private static string RenameKeyfile(string keyfilePath, Action<RestoreActionType, string> callback)
        {
            try
            {
                string directory = Path.GetDirectoryName(keyfilePath);
                string filename = Path.GetFileName(keyfilePath);
                string newFileName = "KEY." + filename;
                string newFilePath = Path.Combine(directory, newFileName);

                if (!File.Exists(newFilePath))
                {
                    File.Move(keyfilePath, newFilePath);
                    callback(RestoreActionType.Status, $"Renamed {filename} to {newFileName}");
                }
                return newFilePath;
            }
            catch (Exception ex)
            {
                callback(RestoreActionType.Error, $"Failed to rename keyfile {Path.GetFileName(keyfilePath)}: {ex.Message}");
                throw;
            }
        }

        private static void ApplyPatches(string patchesDir, string keyfilePath, Action<RestoreActionType, string> callback)
        {
            foreach (var patchFile in Directory.GetFiles(patchesDir, "*.xdelta3"))
            {
                try
                {
                    string patchFileName = Path.GetFileName(patchFile);
                    string originalFileName = patchFileName.Replace(".xdelta3", "");
                    string outputFile = Path.Combine(Path.GetDirectoryName(keyfilePath), originalFileName);
                    string xdelta3Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "3rdparty", "xdelta-3.1.0-x86_64.exe");

                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = xdelta3Path,
                            Arguments = $"-f -d -s \"{keyfilePath}\" \"{patchFile}\" \"{outputFile}\"",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        }
                    };

                    callback(RestoreActionType.Status, $"Applying patch {Path.GetFileName(patchFile)}");

                    process.Start();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        callback(RestoreActionType.Status, $"Successfully applied patch {Path.GetFileName(patchFile)}");
                    }
                    else
                    {
                        string errorOutput = process.StandardError.ReadToEnd();
                        callback(RestoreActionType.Error, $"Failed to apply patch {Path.GetFileName(patchFile)}: {errorOutput}");
                    }
                }
                catch (Exception ex)
                {
                    callback(RestoreActionType.Error, $"Failed to apply patch {Path.GetFileName(patchFile)}: {ex.Message}");
                }
            }

            callback(RestoreActionType.Complete, "All patches applied successfully.");
        }
    }
}
