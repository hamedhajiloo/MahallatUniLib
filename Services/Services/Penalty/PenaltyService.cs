using AutoMapper.QueryableExtensions;
using Common.Enums;
using Data.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class PenaltyService : IPenaltyService
    {
        private readonly IRepository<Penalty> _repository;
        private readonly IRepository<ReserveBook> _rbRepo;
        private readonly IRepository<Setting> _sRepo;
        private readonly IRepository<Book> _bookRepo;

        public PenaltyService(IRepository<Penalty> repository, IRepository<ReserveBook> rbRepo, IRepository<Setting> sRepo,
                              IRepository<Book> bookRepo)
        {
            _repository = repository;
            this._rbRepo = rbRepo ?? throw new System.ArgumentNullException(nameof(rbRepo));
            this._sRepo = sRepo ?? throw new System.ArgumentNullException(nameof(sRepo));
            this._bookRepo = bookRepo ?? throw new System.ArgumentNullException(nameof(bookRepo));
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

        public async Task AddAsyncTask(CancellationToken stoppingToken, PenaltyType penaltyType)
        {
            var setting = await _sRepo.GetByIdAsync(stoppingToken, 1);
            var dtNow = DateTime.Now;
            if (penaltyType == PenaltyType.Return)
            {
                var rb = await _rbRepo.TableNoTracking.Where(c => c.BorrowDate != null
                && c.BorrowDate.Value.AddDays(setting.BorrowDay) < dtNow).ToListAsync(cancellationToken: stoppingToken);

                foreach (var item in rb)
                {
                    var penalty = await _repository.Table.Where(c => c.UserId == item.UserId && c.BookId == item.BookId
                    && c.PenaltyType == PenaltyType.Return).FirstOrDefaultAsync(stoppingToken);

                    if (penalty != null)
                    {
                        penalty.Amount += setting.Amount_Of_Punishment_For_Returning_The_Book;
                        await _repository.UpdateAsync(penalty, stoppingToken);
                    }
                    else
                    {
                        penalty = new Penalty
                        {
                            Amount = setting.Amount_Of_Punishment_For_Returning_The_Book,
                            BookId = item.BookId,
                            PenaltyType = PenaltyType.Return,
                            UserId = item.UserId
                        };
                        await _repository.AddAsync(penalty, cancellationToken: stoppingToken);
                    }
                }
            }
            else
            {
                var rb2 = await _rbRepo.TableNoTracking.Where(c => c.ReserveDate != null
                && c.ReserveDate.Value.AddDays(setting.ReservDay) < dtNow).ToListAsync(cancellationToken: stoppingToken);

                foreach (var item in rb2)
                {
                    var penalty2 = await _repository.Table.Where(c => c.UserId == item.UserId && c.BookId == item.BookId
                    && c.PenaltyType == PenaltyType.Reserve).FirstOrDefaultAsync(stoppingToken);

                    if (penalty2 == null)

                    {
                        penalty2 = new Penalty
                        {
                            Amount = setting.Amount_Of_Punishment_For_Reserving_The_Book,
                            BookId = item.BookId,
                            PenaltyType = PenaltyType.Reserve,
                            UserId = item.UserId
                        };
                        await _repository.AddAsync(penalty2, cancellationToken: stoppingToken);
                    }
                    await _rbRepo.DeleteAsync(item, cancellationToken: stoppingToken);
                }
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