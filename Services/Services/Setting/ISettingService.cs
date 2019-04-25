using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Entities;
using System.Threading.Tasks;
using System.Threading;

namespace Services
{
    public interface ISettingService
    {
        Task<Setting> GetAsync(CancellationToken cancellationToken);
        Task<bool> EditAsync(CancellationToken cancellationToken,Entities.Setting setting);
    }
}
