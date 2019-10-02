using Common.Enums;
using Entities;
using Services.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public interface IPenaltyService
    {
        Task<List<PenaltySelectDto>> GetAll4AnyUserAsync(CancellationToken cancellationToken, string userId, PenaltyType penaltyType);
        Task<bool> AddAsync(CancellationToken cancellationToken, PenaltyDto penaltyDto);

        Task<string> EditAsync(CancellationToken cancellationToken, int penaltyId, PenaltyDto penaltyDto);

        Task<string> RemoveAsync(CancellationToken cancellationToken, int PenaltyId);
        Task AddAsyncTask(CancellationToken stoppingToken, PenaltyType penaltyType);
    }
}