using System;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;

namespace Install
{
    // Ensures the .NET 9.0 runtime (needed by the bundled DepotDownloader 3.4.0) is present.
    // Ported from the NMS Legacy Version Installer's ExtractTemporaryFiles check.
    public static class DotNetCheck
    {
        // Fallback URL if we can't scrape the direct download link.
        private const string DotNet9DownloadPageUrl = "https://dotnet.microsoft.com/en-us/download/dotnet/9.0";

        // Returns true if .NET 9 is available (or the user is choosing to continue anyway); false if it is
        // missing and the caller should abort. Prompts the user to download the runtime when missing.
        public static bool EnsureDotNet9()
        {
            string dnOutput = string.Empty;

            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "dotnet";
                    process.StartInfo.Arguments = "--info";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;

                    process.OutputDataReceived += (s, p) => dnOutput += p.Data + Environment.NewLine;
                    process.ErrorDataReceived += (s, p) => dnOutput += p.Data + Environment.NewLine;

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                }
            }
            catch
            {
                // 'dotnet' not on PATH at all - treat as missing (dnOutput stays empty).
            }

            // DepotDownloader 3.4.0 is a net9.0 console app: its runtimeconfig requires Microsoft.NETCore.App
            // (the base .NET Runtime), which is exactly what the download link below installs. The Desktop
            // Runtime also works (it's a superset) but is not required.
            if (dnOutput.Contains("Microsoft.NETCore.App 9"))
                return true;

            string downloadUrl = TryGetDotNet9RuntimeDownloadUrl();

            string message = "The No Man's Sky Fractal 4.13 Installer requires the .NET 9.0 Runtime to download from Steam, but it was not found on your system." + Environment.NewLine;

            if (!string.IsNullOrEmpty(downloadUrl))
            {
                message += "-------------" + Environment.NewLine;
                message += "Do you wish to download and install the .NET 9.0 Runtime (Windows x64) now?" + Environment.NewLine;
                message += Environment.NewLine + "Download link:" + Environment.NewLine + downloadUrl;

                if (MessageBox.Show(message, ".NET 9.0 Runtime Required", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    Process.Start(downloadUrl);
            }
            else
            {
                // Fallback: send the user to the download page to find the runtime manually.
                message += "-------------" + Environment.NewLine;
                message += "Do you wish to open the .NET 9.0 download page in your browser?" + Environment.NewLine;
                message += Environment.NewLine + "Page: " + DotNet9DownloadPageUrl + Environment.NewLine;
                message += "Look for '.NET Runtime' -> Windows -> x64 Installer.";

                if (MessageBox.Show(message, ".NET 9.0 Runtime Required", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    Process.Start(DotNet9DownloadPageUrl);
            }

            return false;
        }

        private static string TryGetDotNet9RuntimeDownloadUrl()
        {
            try
            {
                // Step 1: Scrape the download page to find the latest runtime x64 installer thank-you page URL.
                string downloadPageHtml = DownloadString(DotNet9DownloadPageUrl);
                if (string.IsNullOrEmpty(downloadPageHtml))
                    return null;

                string thankYouUrl = ExtractFirstRuntimeThankYouUrl(downloadPageHtml);
                if (string.IsNullOrEmpty(thankYouUrl))
                    return null;

                // Step 2: Scrape the thank-you page to find the direct .exe download link.
                string thankYouPageHtml = DownloadString(thankYouUrl);
                if (string.IsNullOrEmpty(thankYouPageHtml))
                    return null;

                return ExtractDirectDownloadLink(thankYouPageHtml);
            }
            catch
            {
                return null;
            }
        }

        // Finds the first (latest) .NET Runtime x64 installer thank-you page URL from the download page HTML.
        // Excludes ASP.NET Core and Desktop Runtime links (matches "runtime-VERSION-", not "runtime-desktop-").
        private static string ExtractFirstRuntimeThankYouUrl(string html)
        {
            string pattern = @"thank-you/runtime-(\d+\.\d+\.\d+)-windows-x64-installer";
            var match = Regex.Match(html, pattern, RegexOptions.IgnoreCase);
            if (match.Success)
                return "https://dotnet.microsoft.com/en-us/download/dotnet/" + match.Value;
            return null;
        }

        // Extracts the direct download .exe URL from a thank-you page's HTML.
        private static string ExtractDirectDownloadLink(string html)
        {
            string pattern = @"https?://builds\.dotnet\.microsoft\.com/[^\s""'<>]+\.exe";
            var match = Regex.Match(html, pattern, RegexOptions.IgnoreCase);
            if (match.Success)
                return match.Value;
            return null;
        }

        // Downloads a string from a URL using WebClient. Returns null on failure. Uses TLS 1.2.
        private static string DownloadString(string url)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
                    return client.DownloadString(url);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
