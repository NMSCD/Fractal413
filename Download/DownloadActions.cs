using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Download
{
    public static class DownloadActions
    {
        private static Process _process;
        public static int _retryCount = 0;
        public static string _args = "";
        public static Action<DownloadActionType, string, float> _callback = null;

        public enum DownloadActionType
        {
            Status,
            Input,
            Error,
            Complete
        }

        public enum ConsoleSourceType
        {
            Normal,
            Error
        }


        public static void DoDownload(string downloadPath, string username, string password, Action<DownloadActionType, string, float> callback)
        {
            _retryCount = 0;
            _args = $" -app 275850 -depot 275851 -manifest 3961348687487426672 -dir \"{downloadPath}\" -username {username} -password {password} -remember-password";
            _callback = callback;
            DepotDownloader();
        }

        public static void DepotDownloader()
        {
            string depotDownloader = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "3rdParty\\DepotDownloader.exe");

            Thread downloadThread = new Thread(() =>
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = depotDownloader,
                        Arguments = _args,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    _process = new Process { StartInfo = startInfo };

                    _process.OutputDataReceived += (sender, e) => ProcessOutput(e.Data, _callback, ConsoleSourceType.Normal);
                    _process.ErrorDataReceived += (sender, e) => ProcessOutput(e.Data, _callback, ConsoleSourceType.Error);

                    _process.Start();

                    Thread outputThread = new Thread(() => ReadStream(_process.StandardOutput, _callback, ConsoleSourceType.Normal));
                    Thread errorThread = new Thread(() => ReadStream(_process.StandardError, _callback, ConsoleSourceType.Error));

                    outputThread.Start();
                    errorThread.Start();

                    _process.WaitForExit();
                    outputThread.Join();
                    errorThread.Join();

                    _callback(DownloadActionType.Complete, "Process completed.", 0);
                }
                catch (Exception ex)
                {
                    _callback(DownloadActionType.Error, ex.Message, 0);
                    TerminateProcess();
                }
            });

            downloadThread.Start();
        }

        private static bool IsEnterPrompt(string line)
        {
            if (line.ToString().ToLower().Contains("please enter") || line.ToString().ToLower().StartsWith("enter"))
                return true;
            return false;
        }

        private static void ReadStream(StreamReader stream, Action<DownloadActionType, string, float> callback, ConsoleSourceType sourceType)
        {
            StringBuilder lineBuffer = new StringBuilder();

            while (!stream.EndOfStream)
            {
                char[] buffer = new char[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                for (int i = 0; i < bytesRead; i++)
                {
                    char c = buffer[i];
                    lineBuffer.Append(c);

                    if (c == '\n' || (c == ':' && IsEnterPrompt(lineBuffer.ToString())))
                    {
                        string line = lineBuffer.ToString();
                        ProcessOutput(line, callback, sourceType);
                        lineBuffer.Clear();
                    }
                }

                // Process any remaining data in the buffer
                if (lineBuffer.Length > 0)
                {
                    string line = lineBuffer.ToString();
                    if (!line.EndsWith("\n"))
                    {
                        ProcessOutput(line, callback, sourceType);
                        lineBuffer.Clear();
                    }
                }
            }
        }

        private static void ProcessOutput(string data, Action<DownloadActionType, string, float> callback, ConsoleSourceType sourceType)
        {
            if (string.IsNullOrEmpty(data)) return;

            if (data.Contains(":") && IsEnterPrompt(data))
            {
                callback(DownloadActionType.Input, data, 0);
            }
            else if (data.Contains("Error"))
            {                
                if(data.ToLower().Contains("password") || _retryCount >= 3)
                {
                    callback(DownloadActionType.Error, data, 0);
                } else {
                    // Retry
                    _retryCount++;
                    TerminateProcess();
                    DepotDownloader();
                    return;
                }
                TerminateProcess();
            }
            else if (data.Contains("%"))
            {
                string[] parts = data.Split(' ');
                foreach (string part in parts)
                {
                    if (part.Contains("%") && float.TryParse(part.Replace("%", ""), out float percentage))
                    {
                        callback(DownloadActionType.Status, data, percentage);
                        return;
                    }
                }
            }
            else
            {
                callback(DownloadActionType.Status, data, 0);
            }
        }

        public static void SendResponse(string response)
        {
            try
            {
                if (_process != null && !_process.HasExited)
                {
                    using (StreamWriter writer = _process.StandardInput)
                    {
                        writer.WriteLine(response);
                        writer.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendResponse: {ex.Message}");
            }
        }

        private static void TerminateProcess()
        {
            try
            {
                if (_process != null && !_process.HasExited)
                {
                    _process.Kill();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error terminating process: {ex.Message}");
            }
        }
    }
}
