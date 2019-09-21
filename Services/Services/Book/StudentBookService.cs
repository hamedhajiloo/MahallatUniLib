using Common.Exceptions;
using Data.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public interface IStudentBookService
    {
        Task<bool> Reserve(string userId, int bookId, CancellationToken cancellationToken);
    }
    public class StudentBookService : IStudentBookService
    {
        private readonly IRepository<ReserveBook> _sbRepository;
        private readonly IRepository<User> _uRepository;
        private readonly IRepository<Book> _bRepository;
        private readonly IRepository<Setting> _seRepository;
        private readonly IRepository<Student> _sRepository;

        public StudentBookService(IRepository<ReserveBook> sbRepository,
                                  IRepository<User> uRepository,
                                  IRepository<Book> bRepository,
                                  IRepository<Setting> seRepository,
                                  IRepository<Student> sRepository)
        {
            this._sbRepository = sbRepository ?? throw new ArgumentNullException(nameof(sbRepository));
            this._uRepository = uRepository ?? throw new ArgumentNullException(nameof(uRepository));
            this._bRepository = bRepository ?? throw new ArgumentNullException(nameof(bRepository));
            this._seRepository = seRepository ?? throw new ArgumentNullException(nameof(seRepository));
            this._sRepository = sRepository ?? throw new ArgumentNullException(nameof(sRepository));
        }
        public async Task<bool> Reserve(string userId, int bookId, CancellationToken cancellationToken)
        {
            var setting = await _seRepository.GetByIdAsync(cancellationToken,1);
            var book = await _bRepository.Table.Where(c => c.Id == bookId).SingleOrDefaultAsync(cancellationToken);
            if (book==null)
                throw new BadRequestException("کتاب یافت نشد");

            var user = await _uRepository.Table.Where(c => c.Id == userId).SingleOrDefaultAsync(cancellationToken);
            if (user == null)
                throw new BadRequestException("کاربر یافت نشد");

            if (user.ReserveBooks.Count>=setting.ReservCount)
                return false;


            var reserveBook = new ReserveBook
            {
                BookId = bookId,
                UserId = userId,
                BookStatus = Common.Enums.BookStatus.Reserved,
                ReserveDate=DateTime.Now
            };
            await _sbRepository.AddAsync(reserveBook, cancellationToken);


            return true;
        }
    }
}
