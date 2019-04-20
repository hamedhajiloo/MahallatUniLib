using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Data.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using Services.Dto;
using System.Linq.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using System.Linq.Expressions;

namespace Services
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _repository;
        private readonly IBaseService<Book> _baseService;

        public BookService(IRepository<Book> repository,IBaseService<Book> baseService)
        {
            _repository = repository;
            _baseService = baseService;
        }

        public async Task<BookSelectDto>  AddBookAsync(BookDto bookDto,CancellationToken cancellationToken)
        {
            var existsBook = await BookExists(bookDto,cancellationToken);
            if (existsBook) throw new BadRequestException("این کتاب قبلا ثبت شده است");
           var book= bookDto.ToEntity();
            await _repository.AddAsync(book,cancellationToken);
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
            var result =await _repository.TableNoTracking.ProjectTo<BookSelectDto>().ToListAsync(cancellationToken);
            return result;

        }
        public async Task<List<BookSelectDto>> GetAllBook4AnyField(CancellationToken cancellationToken, Pagable pagable, int FieldId)
        {
            Expression<Func<Book, bool>> expression = p =>
              p.AuthorName.Contains(pagable.Search.Trim()) &&
                                p.Edition.ToString().Contains(pagable.Search.Trim()) &&
                                p.Publisher.Contains(pagable.Search.Trim()) &&
                                p.PublishDate.ToString().Contains(pagable.Search.Trim()) &&
                                p.Name.Contains(pagable.Search.Trim()) &&
                                p.Language.ToString().Contains(pagable.Search.Trim()) &&
                                p.ISBN.Contains(pagable.Search.Trim())&&
                                p.FieldBookList.Select(c=>c.FieldId).Equals(FieldId);

            var model = await _baseService.GetAllAsync<BookSelectDto>(cancellationToken, pagable, expression);
            return model;
        }

        protected async Task<bool> BookExists(BookDto bookDto, CancellationToken cancellationToken)
        {
            return await _repository.TableNoTracking
                .Where(c => (c.AuthorName == bookDto.AuthorName &&
                            c.Edition == bookDto.Edition &&
                            c.ISBN == bookDto.ISBN &&
                            c.Language == bookDto.Language &&
                            c.Name == bookDto.Name &&
                            c.Publisher == bookDto.Publisher) ||c.ISBN==bookDto.ISBN ).AnyAsync(cancellationToken);
           
        }
    }
}
