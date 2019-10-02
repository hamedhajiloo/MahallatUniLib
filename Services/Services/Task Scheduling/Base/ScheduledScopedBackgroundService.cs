using Microsoft.Extensions.DependencyInjection;
using NCrontab;
using System;
using System.Threading;
using System.Threading.Tasks;
using static NCrontab.CrontabSchedule;

namespace Services.Task_Scheduling
{
    public abstract class ScheduledScopedBackgroundService : ScopedBackgroundService
    {
        private CrontabSchedule _schedule;
        private DateTime _nextRun;

        protected abstract string Schedule { get; }

        public ScheduledScopedBackgroundService(IServiceScopeFactory serviceScopeFactory)
         : base(serviceScopeFactory)
        {
            _schedule = CrontabSchedule.Parse(Schedule, new ParseOptions { IncludingSeconds = true });
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
        }

        public override async Task ExecuteInScope(IServiceProvider serviceProvider, CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.Now;
                if (now > _nextRun)
                {
                    await ScheduledExecuteInScope(serviceProvider, stoppingToken);
                    _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                }
                await Task.Delay(1000, stoppingToken); //1 second delay
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        public abstract Task ScheduledExecuteInScope(IServiceProvider serviceProvider, CancellationToken stoppingToken);
    }
}