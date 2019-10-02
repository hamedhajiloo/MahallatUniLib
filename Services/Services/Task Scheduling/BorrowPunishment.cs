using Common.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Task_Scheduling
{
    public class BorrowPunishment : ScheduledScopedBackgroundService
    {
        private readonly ILogger<BorrowPunishment> _logger;

        public BorrowPunishment(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<BorrowPunishment> logger) : base(serviceScopeFactory)
        {
            _logger = logger;
        }

        protected override string Schedule => "1 1 23 * * *"; //Runs every day at 23:1':1''

        public override Task ScheduledExecuteInScope(IServiceProvider serviceProvider, CancellationToken stoppingToken)
        {
            var orderService = serviceProvider.GetRequiredService<IPenaltyService>();
            orderService.AddAsyncTask(stoppingToken, PenaltyType.Return);
            _logger.LogInformation("BorrowPunishment executing - {0}", DateTime.Now);
            return Task.CompletedTask;
        }
    }
}