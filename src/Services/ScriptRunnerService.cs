using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using RcloneQBController.Models;

namespace RcloneQBController.Services
{
    public class ScriptRunnerService
    {
        private static Mutex _mutex = new Mutex(false, "RcloneQBController");

        public async Task RunScriptAsync(RcloneJobConfig job, Action<string> onOutput)
        {
            if (!_mutex.WaitOne(TimeSpan.Zero, true))
            {
                onOutput("Job is already running.");
                return;
            }

            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                    FileName = "cmd.exe",
                    Arguments = $"/c rclone copy \"{job.SourcePath}\" \"{job.DestPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.OutputDataReceived += (sender, args) => {
                if (args.Data != null)
                {
                    onOutput(args.Data);
                }
            };
            process.ErrorDataReceived += (sender, args) => {
                if (args.Data != null)
                {
                    onOutput(args.Data);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }
    }
}