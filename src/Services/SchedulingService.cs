using RcloneQBController.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RcloneQBController.Services
{
    public class SchedulingService
    {
        private readonly Dictionary<string, Timer> _timers = new();
        private readonly ScriptRunnerService _scriptRunnerService;

        public SchedulingService(ScriptRunnerService scriptRunnerService)
        {
            _scriptRunnerService = scriptRunnerService;
        }

        public void Start(RcloneJobConfig job, TimeSpan interval)
        {
            if (job.Name == null || _timers.ContainsKey(job.Name)) return;

            var timer = new Timer(async _ =>
            {
                await _scriptRunnerService.RunRcloneJobAsync(job, _ => { });
            }, null, TimeSpan.Zero, interval);

            _timers[job.Name] = timer;
        }

        public void Stop(RcloneJobConfig job)
        {
            if (job.Name == null) return;

            if (_timers.TryGetValue(job.Name, out var timer))
            {
                timer.Dispose();
                _timers.Remove(job.Name);
            }
        }
    }
}