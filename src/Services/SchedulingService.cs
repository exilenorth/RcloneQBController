using RcloneQBController.Models;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace RcloneQBController.Services
{
    public class SchedulingService
    {
        private readonly ConcurrentDictionary<string, Timer> _timers = new();
        private readonly ConcurrentDictionary<string, DateTime> _lastRunTimes = new();
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
                var jobName = job.Name;

                if (_lastRunTimes.TryGetValue(jobName, out var lastRun))
                {
                    var elapsed = DateTime.UtcNow - lastRun;
                    if (elapsed > TimeSpan.FromMilliseconds(interval.TotalMilliseconds * 1.5))
                    {
                    }
                }
                await _scriptRunnerService.RunRcloneJobAsync(job, _ => { });
                _lastRunTimes[jobName] = DateTime.UtcNow;

            }, null, TimeSpan.Zero, interval);

            _timers.TryAdd(job.Name, timer);
        }

        public void Stop(RcloneJobConfig job)
        {
            if (job.Name == null) return;

            if (_timers.TryRemove(job.Name, out var timer))
            {
                timer.Dispose();
                _lastRunTimes.TryRemove(job.Name, out _);
            }
        }
    }
}