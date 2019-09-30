using Common;
using Common.Enums;
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
        Task<List<BookSelectDto>> GetAllBookAsync(Pagable pagable,CancellationToken cancellationToken);
        Task<List<BookSelectDto>> GetAllBookAsync(CancellationToken cancellationToken,BookStatus bookStatus,Language language,int Field, string search = "");
        Task<bool> BookExists(BookDto bookDto, CancellationToken cancellationToken);
        Task<bool> BookExists(int bookId, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<bool> DeleteOneBookAsync(string isbn, CancellationToken cancellationToken);
        Task<BookSelectDto> FindBookByIdAsync(int id, CancellationToken cancellationToken);
        Task<BookDto> FindBookById4EditAsync(int id, CancellationToken cancellationToken);
        Task<bool> EditAsync( BookDto bookDto, CancellationToken cancellationToken);
        Task<List<BookSelectDto>> GetAllBook4AnyField(CancellationToken cancellationToken, Pagable pagable, int FieldId);
    }
}
