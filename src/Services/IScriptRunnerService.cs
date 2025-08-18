using RcloneQBController.Models;
using System;
using System.Threading.Tasks;

namespace RcloneQBController.Services
{
    public interface IScriptRunnerService
    {
        Task RunRcloneJobAsync(RcloneJobConfig job, Action<string> onOutput);
        void StopJob(string jobName);
        Task RunCleanupScriptAsync(bool isDryRun, Action<string> onOutput);
    }
}