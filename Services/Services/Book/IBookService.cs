using Common;
using Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public interface IBookService
    {
        Task<BookSelectDto> AddBookAsync(BookDto bookDto,CancellationToken cancellationToken);
        Task<List<BookSelectDto>> GetAllBookAsync(CancellationToken cancellationToken);
    }
}
