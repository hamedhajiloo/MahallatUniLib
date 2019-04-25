using AutoMapper.QueryableExtensions;
using Common;
using Common.Enums;
using Data.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _repository;
        private readonly IBaseService<Book> _baseService;

        public BookService(IRepository<Book> repository, IBaseService<Book> baseService)
        {
            _repository = repository;
            _baseService = baseService;
        }

        public async Task<BookSelectDto> AddBookAsync(BookDto bookDto, CancellationToken cancellationToken)
        {
            //var existsBook = await BookExists(bookDto,cancellationToken);
            //if (existsBook) throw new BadRequestException("این کتاب قبلا ثبت شده است");
            var book = bookDto.ToEntity();
            await _repository.AddAsync(book, cancellationToken);
            var resultDto = await _repository.TableNoTracking.ProjectTo<BookSelectDto>().SingleOrDefaultAsync(p => p.Id.Equals(book.Id), cancellationToken);

            return resultDto;
        }

        public async Task<List<BookSelectDto>> GetAllBookAsync(CancellationToken cancellationToken)
        {
            //Expression<Func<Book, bool>> expression = p =>
            //   p.AuthorName.Contains(pagable.Search) &&
            //                     p.Edition.ToString().Contains(pagable.Search) &&
            //                     p.Publisher.Contains(pagable.Search) &&
            //                     p.Name.Contains(pagable.Search) &&
            //                     p.ISBN.Contains(pagable.Search);

            //var model = await _baseService.GetAllAsync<BookSelectDto>(cancellationToken,pagable,expression);
            var result = await _repository.TableNoTracking.OrderByDescending(c => c.Id).ProjectTo<BookSelectDto>().ToListAsync(cancellationToken);
            return result;
        }

        public async Task<List<BookSelectDto>> GetAllBook4AnyField(CancellationToken cancellationToken, Pagable pagable, int FieldId)
        {
            Expression<Func<Book, bool>> expression = p =>
              p.AuthorName.Contains(pagable.Search.Trim()) &&
                                p.Edition.ToString().Contains(pagable.Search.Trim()) &&
                                p.Publisher.Contains(pagable.Search.Trim()) &&
                                p.PublishYear.ToString().Contains(pagable.Search.Trim()) &&
                                p.Name.Contains(pagable.Search.Trim()) &&
                                p.Language.ToString().Contains(pagable.Search.Trim()) &&
                                p.ISBN.Contains(pagable.Search.Trim()) &&
                                p.FieldBookList.Select(c => c.FieldId).Equals(FieldId);

            var model = await _baseService.GetAllAsync<BookSelectDto>(cancellationToken, pagable, expression);
            return model;
        }

        public async Task<bool> BookExists(BookDto bookDto, CancellationToken cancellationToken)
        {
            return await _repository.TableNoTracking
                .Where(c => (c.AuthorName == bookDto.AuthorName &&
                            c.Edition == bookDto.Edition &&
                            c.ISBN == bookDto.ISBN &&
                            c.Language == bookDto.Language &&
                            c.Name == bookDto.Name &&
                            c.Publisher == bookDto.Publisher) || c.ISBN == bookDto.ISBN).AnyAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var model = await _repository.TableNoTracking.Where(c => c.Id == id).SingleOrDefaultAsync(cancellationToken);
            if (model == null) return false;

            await _repository.DeleteAsync(model, cancellationToken);
            return true;
        }

        public async Task<BookSelectDto> FindBookByIdAsync(int id, CancellationToken cancellationToken)
        {
            var model = await _repository.TableNoTracking.Where(c => c.Id == id).ProjectTo<BookSelectDto>().SingleOrDefaultAsync(cancellationToken);
            return model;
        }

        public async Task<bool> EditAsync(BookDto bookDto, CancellationToken cancellationToken)
        {
            var exists = await BookExists(bookDto, cancellationToken);
            if (exists == false) return false;
            var book = bookDto.ToEntity();
            await _repository.UpdateAsync(book, cancellationToken);
            return true;
        }

        public async Task<BookDto> FindBookById4EditAsync(int id, CancellationToken cancellationToken)
        {
            var model = await _repository.Table.Where(c => c.Id == id).ProjectTo<BookDto>().SingleOrDefaultAsync(cancellationToken);
            return model;
        }

        public async Task<List<BookSelectDto>> GetAllBookAsync(CancellationToken cancellationToken, CourseType courseType, BookStatus bookStatus, Language language, int Field, string search)
        {
            var models = _repository.TableNoTracking;

        
            if (search != null)
                models = models.Where(p => (p.AuthorName.Contains(search) ||
                                  p.Edition.ToString().Contains(search) ||
                                  p.Publisher.Contains(search) ||
                                  p.Name.Contains(search) ||
                                  p.ISBN.Contains(search)) &&
                                  (courseType != CourseType.None ? p.CourseType == courseType : true) &&
                                  (language != Language.None ? p.Language == language : true) &&
                                  (bookStatus != BookStatus.None ? p.BookStatus == bookStatus : true));
            else if (search == null)
                models = models.Where(p => (courseType != CourseType.None ? p.CourseType == courseType : true) &&
                                  (language != Language.None ? p.Language == language : true) &&
                                  (bookStatus != BookStatus.None ? p.BookStatus == bookStatus : true));

            return await models.OrderByDescending(c => c.Id).ProjectTo<BookSelectDto>().ToListAsync(cancellationToken);


        }
    }
}