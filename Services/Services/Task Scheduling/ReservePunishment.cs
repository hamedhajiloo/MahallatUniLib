using Common.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Task_Scheduling
{
    public class ReservePunishment : ScheduledScopedBackgroundService
    {
        private readonly ILogger<ReservePunishment> _logger;

        public ReservePunishment(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<ReservePunishment> logger) : base(serviceScopeFactory)
        {
            _logger = logger;
        }

        protected override string Schedule => "1 1 22 * * *"; //Runs every day at 22:1':1''

        public override Task ScheduledExecuteInScope(IServiceProvider serviceProvider, CancellationToken stoppingToken)
        {
            var orderService = serviceProvider.GetRequiredService<IPenaltyService>();
            orderService.AddAsyncTask(stoppingToken, PenaltyType.Reserve);
            _logger.LogInformation("ReservePunishment executing - {0}", DateTime.Now);
            return Task.CompletedTask;
        }
    }
}