using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using RcloneQBController.Models;
using System.IO;
using System.Text;

namespace RcloneQBController.Services
{
    public class ScriptRunnerService
    {
        private static readonly Mutex RcloneMutex = new Mutex(false, "RcloneQBController_Rclone");
        private static readonly Mutex CleanupMutex = new Mutex(false, "RcloneQBController_Cleanup");
        private readonly Dictionary<string, Process> _runningProcesses = new();

        public async Task RunRcloneJobAsync(RcloneJobConfig job, Action<string> onOutput)
        {
            if (!RcloneMutex.WaitOne(TimeSpan.Zero, true))
            {
                onOutput("An rclone job is already running.");
                return;
            }

                            try
                            {
                                var scriptPath = Path.Combine(System.AppContext.BaseDirectory, "scripts", $"rclone_pull_{job.Name}.bat");
                                if (!File.Exists(scriptPath))
                                {
                                    onOutput($"Error: Script not found at {scriptPath}");
                                    return;
                                }
            
                                await ExecuteProcessAsync(job.Name, scriptPath, "", onOutput, job.MaxRuntimeMinutes);
                            }
                            finally
                            {
                                RcloneMutex.ReleaseMutex();
                            }
        }

        public void StopJob(string jobName)
        {
            if (_runningProcesses.TryGetValue(jobName, out var process))
            {
                if (!process.HasExited)
                {
                    process.Kill(entireProcessTree: true);
                    process.WaitForExit();
                }
                _runningProcesses.Remove(jobName);
            }
        }

        public async Task RunCleanupScriptAsync(bool isDryRun, Action<string> onOutput)
        {
            if (!CleanupMutex.WaitOne(TimeSpan.Zero, true))
            {
                onOutput("Cleanup script is already running.");
                return;
            }

                            try
                            {
                                var scriptPath = Path.Combine(System.AppContext.BaseDirectory, "scripts", "qb_cleanup_ratio.ps1");
                                if (!File.Exists(scriptPath))
                                {
                                    onOutput($"Error: Script not found at {scriptPath}");
                                    return;
                                }
            
                                var arguments = new StringBuilder($"-ExecutionPolicy Bypass -File \"{scriptPath}\"");
                                if (isDryRun)
                                {
                                    arguments.Append(" -DryRun");
                                }
            
                                await ExecuteProcessAsync("cleanup", "powershell.exe", arguments.ToString(), onOutput, 0);
                            }
                            finally
                            {
                                CleanupMutex.ReleaseMutex();
                            }
        }

        private async Task ExecuteProcessAsync(string jobName, string fileName, string arguments, Action<string> onOutput, int timeoutMinutes)
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

            _runningProcesses[jobName] = process;

            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null) onOutput(args.Data);
            };
            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data != null) onOutput($"ERROR: {args.Data}");
            };

            try
            {
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
                            if (!process.HasExited)
                            {
                                process.Kill();
                            }
                            onOutput($"ERROR: Process timed out after {timeoutMinutes} minutes and was terminated.");
                        }
                    }
                }
                else
                {
                    await process.WaitForExitAsync();
                }
            }
            finally
            {
                _runningProcesses.Remove(jobName);
            }
        }
    }
}