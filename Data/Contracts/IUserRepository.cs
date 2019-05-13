using Data.Repositories;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task UpdateLastLoginDateAsync(User user, CancellationToken requestAborted);
    }
}
