using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace SmartPOS.WinForms.UI.Helpers
{
    internal static class PhoneBridgeFirewallHelper
    {
        private const int BridgePort = 5055;
        private const string PortRuleName = "SmartPOS Phone Bridge TCP 5055";
        private const string AppRuleName = "SmartPOS Phone Bridge App 5055";

        private static int _started;
        private static int _elevationAttempted;

        public static void EnsureAsync()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                return;
            }

            if (Interlocked.Exchange(ref _started, 1) == 1)
            {
                return;
            }

            ThreadPool.QueueUserWorkItem(_ => Ensure());
        }

        public static void EnsureWhenBridgeStarts()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                return;
            }

            Ensure();
        }

        private static void Ensure()
        {
            try
            {
                string exePath = Process.GetCurrentProcess().MainModule.FileName;
                if (IsFirewallReady(exePath))
                {
                    return;
                }

                string script = BuildEnsureScript(exePath);
                if (IsAdministrator())
                {
                    RunPowerShell(script, false, true);
                    return;
                }

                if (Interlocked.Exchange(ref _elevationAttempted, 1) == 1)
                {
                    return;
                }

                RunPowerShell(script, true, false);
            }
            catch
            {
                // Firewall setup is best-effort. The scanner dialog still shows the bridge URL and manual checks.
            }
        }

        private static bool IsFirewallReady(string exePath)
        {
            string script =
                "$ErrorActionPreference='SilentlyContinue';" +
                "$exe=" + PsString(exePath) + ";" +
                "$port='" + BridgePort + "';" +
                "$portRule=" + PsString(PortRuleName) + ";" +
                "$appRule=" + PsString(AppRuleName) + ";" +
                "$allow=$false;" +
                "Get-NetFirewallRule -Direction Inbound -Action Allow -Enabled True | " +
                "Where-Object { $_.DisplayName -eq $portRule -or $_.DisplayName -eq $appRule } | ForEach-Object {" +
                "  $pf=$_ | Get-NetFirewallPortFilter;" +
                "  if($pf -and $pf.Protocol -eq 'TCP' -and ($pf.LocalPort -eq $port -or $pf.LocalPort -eq 'Any')){ $allow=$true }" +
                "};" +
                "$block=$false;" +
                "Get-NetFirewallRule -Direction Inbound -Action Block -Enabled True | ForEach-Object {" +
                "  $af=$_ | Get-NetFirewallApplicationFilter;" +
                "  if($af -and $af.Program){" +
                "    try {" +
                "      $program=[System.IO.Path]::GetFullPath([Environment]::ExpandEnvironmentVariables($af.Program));" +
                "      $current=[System.IO.Path]::GetFullPath($exe);" +
                "      if([string]::Equals($program,$current,[System.StringComparison]::OrdinalIgnoreCase)){ $block=$true }" +
                "    } catch {}" +
                "  }" +
                "};" +
                "if($allow -and -not $block){ 'READY' } else { 'NEEDS_FIX' }";

            string output = RunPowerShellCapture(script);
            return output.IndexOf("READY", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string BuildEnsureScript(string exePath)
        {
            return
                "$ErrorActionPreference='SilentlyContinue';" +
                "$exe=" + PsString(exePath) + ";" +
                "$port=" + BridgePort + ";" +
                "$portRule=" + PsString(PortRuleName) + ";" +
                "$appRule=" + PsString(AppRuleName) + ";" +
                "Get-NetFirewallRule -Direction Inbound -Action Block -Enabled True | ForEach-Object {" +
                "  $rule=$_;" +
                "  $af=$rule | Get-NetFirewallApplicationFilter;" +
                "  if($af -and $af.Program){" +
                "    try {" +
                "      $program=[System.IO.Path]::GetFullPath([Environment]::ExpandEnvironmentVariables($af.Program));" +
                "      $current=[System.IO.Path]::GetFullPath($exe);" +
                "      if([string]::Equals($program,$current,[System.StringComparison]::OrdinalIgnoreCase)){" +
                "        $rule | Disable-NetFirewallRule | Out-Null" +
                "      }" +
                "    } catch {}" +
                "  }" +
                "};" +
                "Get-NetFirewallRule -DisplayName $portRule | Remove-NetFirewallRule;" +
                "Get-NetFirewallRule -DisplayName $appRule | Remove-NetFirewallRule;" +
                "New-NetFirewallRule -DisplayName $portRule -Direction Inbound -Action Allow -Protocol TCP -LocalPort $port -Profile Any -Description 'Allow SmartPOS phone scanner bridge on TCP 5055.' | Out-Null;" +
                "New-NetFirewallRule -DisplayName $appRule -Direction Inbound -Action Allow -Program $exe -Protocol TCP -LocalPort $port -Profile Any -Description 'Allow SmartPOS phone scanner bridge executable.' | Out-Null;";
        }

        private static bool IsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private static string RunPowerShellCapture(string script)
        {
            using (Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = BuildPowerShellArguments(script),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }))
            {
                if (process == null)
                {
                    return string.Empty;
                }

                StringBuilder output = new StringBuilder();
                process.OutputDataReceived += (sender, args) =>
                {
                    if (args.Data != null)
                    {
                        output.AppendLine(args.Data);
                    }
                };
                process.ErrorDataReceived += (sender, args) => { };
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                if (!process.WaitForExit(8000))
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                    }
                }

                return output.ToString();
            }
        }

        private static void RunPowerShell(string script, bool elevate, bool waitForExit)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = BuildPowerShellArguments(script),
                UseShellExecute = elevate,
                CreateNoWindow = !elevate,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            if (elevate)
            {
                startInfo.Verb = "runas";
            }

            using (Process process = Process.Start(startInfo))
            {
                if (waitForExit && process != null)
                {
                    process.WaitForExit(15000);
                }
            }
        }

        private static string BuildPowerShellArguments(string script)
        {
            string encodedScript = Convert.ToBase64String(Encoding.Unicode.GetBytes(script));
            return "-NoProfile -ExecutionPolicy Bypass -EncodedCommand " + encodedScript;
        }

        private static string PsString(string value)
        {
            return "'" + (value ?? string.Empty).Replace("'", "''") + "'";
        }
    }
}
