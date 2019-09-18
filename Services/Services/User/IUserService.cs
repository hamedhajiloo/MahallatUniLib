using Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService
    {
        Task<AccessToken> TokenAsync(string username , string password,CancellationToken cancellationToken);

        Task DeleteAsync(string id, CancellationToken cancellationToken);
    }
}
