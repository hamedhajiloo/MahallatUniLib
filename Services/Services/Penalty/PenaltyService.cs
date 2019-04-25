using AutoMapper.QueryableExtensions;
using Common.Enums;
using Data.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using Services.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class PenaltyService : IPenaltyService
    {
        private readonly IRepository<Penalty> _repository;

        public PenaltyService(IRepository<Penalty> repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddAsync(CancellationToken cancellationToken, PenaltyDto penaltyDto)
        {
            try
            {
                var penalty = penaltyDto.ToEntity();
                await _repository.AddAsync(penalty, cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> EditAsync(CancellationToken cancellationToken, int penaltyId, PenaltyDto penaltyDto)
        {
            try
            {
                var model = await _repository.Table.Where(c => c.Id == penaltyId).SingleOrDefaultAsync(cancellationToken);
                if (model == null)
                    return "NotFound";
                var res = penaltyDto.ToEntity(model);
                await _repository.UpdateAsync(res, cancellationToken);
                return "Ok";
            }
            catch
            {
                return "Bad";
            }
        }

        public async Task<List<PenaltySelectDto>> GetAll4AnyUserAsync(CancellationToken cancellationToken, string userId, PenaltyType penaltyType)
        {
            var model = await _repository.TableNoTracking.Where(c => c.UserId == userId).ProjectTo<PenaltySelectDto>().ToListAsync(cancellationToken);
            return model;
        }

        public async Task<string> RemoveAsync(CancellationToken cancellationToken, int penaltyId)
        {
            try
            {
                var model = await _repository.Table.Where(c => c.Id == penaltyId).SingleOrDefaultAsync(cancellationToken);
                if (model == null)
                    return "NotFound";

                await _repository.DeleteAsync(model, cancellationToken);
                return "Ok";
            }
            catch
            {
                return "Bad";
            }
        }
    }
}