using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using RcloneQBController.Models;
using System.IO;

namespace RcloneQBController.Services
{
    public class ScriptRunnerService
    {
        private static Mutex _rcloneMutex = new Mutex(false, "RcloneQBController_Rclone");
        private static Mutex _cleanupMutex = new Mutex(false, "RcloneQBController_Cleanup");

        public async Task RunRcloneJobAsync(RcloneJobConfig job, Action<string> onOutput)
        {
            if (!_rcloneMutex.WaitOne(TimeSpan.Zero, true))
            {
                onOutput("An rclone job is already running.");
                return;
            }

            try
            {
                var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "scripts", $"rclone_pull_{job.Name}.bat");
                if (!File.Exists(scriptPath))
                {
                    onOutput($"Error: Script not found at {scriptPath}");
                    return;
                }

                await ExecuteProcessAsync(scriptPath, "", onOutput, job.MaxRuntimeMinutes);
            }
            finally
            {
                _rcloneMutex.ReleaseMutex();
            }
        }

        public async Task RunCleanupScriptAsync(Action<string> onOutput)
        {
            if (!_cleanupMutex.WaitOne(TimeSpan.Zero, true))
            {
                onOutput("Cleanup script is already running.");
                return;
            }

            try
            {
                var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "scripts", "qb_cleanup_ratio.ps1");
                if (!File.Exists(scriptPath))
                {
                    onOutput($"Error: Script not found at {scriptPath}");
                    return;
                }
                
                await ExecuteProcessAsync("powershell.exe", $"-ExecutionPolicy Bypass -File \"{scriptPath}\"", onOutput, 0);
            }
            finally
            {
                _cleanupMutex.ReleaseMutex();
            }
        }

        private async Task ExecuteProcessAsync(string fileName, string arguments, Action<string> onOutput, int timeoutMinutes)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.OutputDataReceived += (sender, args) => {
                if (args.Data != null) onOutput(args.Data);
            };
            process.ErrorDataReceived += (sender, args) => {
                if (args.Data != null) onOutput($"ERROR: {args.Data}");
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            if (timeoutMinutes > 0)
            {
                using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(timeoutMinutes)))
                {
                    try
                    {
                        await process.WaitForExitAsync(cts.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        process.Kill();
                        onOutput($"ERROR: Process timed out after {timeoutMinutes} minutes and was terminated.");
                    }
                }
            }
            else
            {
                await process.WaitForExitAsync();
            }
        }
    }
}