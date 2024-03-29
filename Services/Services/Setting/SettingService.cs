﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Data.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class SettingService : ISettingService
    {
        private readonly IRepository<Setting> _repository;

        public SettingService(IRepository<Entities.Setting> repository)
        {
            _repository = repository;
        }
        public async Task<bool> EditAsync(CancellationToken cancellationToken, Setting setting)
        {
            try
            {
                var model = await _repository.Table.Where(c => c.Id == 1).SingleOrDefaultAsync(cancellationToken);
                model.Amount_Of_Punishment_For_Reserving_The_Book = setting.Amount_Of_Punishment_For_Reserving_The_Book;
                model.Amount_Of_Punishment_For_Returning_The_Book = setting.Amount_Of_Punishment_For_Returning_The_Book;
                model.BorrowDay = setting.BorrowDay;
                model.ReservCount = setting.ReservCount;
                model.BorrowCount = setting.BorrowCount;
                model.ReservDay = setting.ReservDay;
                model.BDay4Reserve = setting.BDay4Reserve;
                await _repository.UpdateAsync(model, cancellationToken);
                return true;
            }
            catch
            {

                return false;
            }

        }

        public async Task<Setting> GetAsync(CancellationToken cancellationToken)
        {
            return await _repository.TableNoTracking.Where(c => c.Id == 1).SingleOrDefaultAsync(cancellationToken);
        }
    }
}
