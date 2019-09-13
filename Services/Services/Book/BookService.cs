using AutoMapper.QueryableExtensions;
using Common;
using Common.Enums;
using Common.Exceptions;
using Data.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _repository;
        private readonly IRepository<Isbn> _isbnRepository;
        private readonly IRepository<Field> _filedRepository;
        private readonly IRepository<Book> _Book;

        public BookService(IRepository<Book> repository,
             IRepository<Isbn> IsbnRepository,
             IRepository<Field> filedRepository,
            IRepository<Book> Book)
        {
            _repository = repository;
            _isbnRepository = IsbnRepository;
            _filedRepository = filedRepository;
            _Book = Book;
        }


        /// <summary>
        /// افزودن کتاب
        /// </summary>
        /// <param name="bookDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<BookSelectDto> AddBookAsync(BookDto bookDto, CancellationToken cancellationToken)
        {
            Book Book = bookDto.ToEntity();
            int fieldId = 0;
            if (bookDto.FieldId==FieldStatus.N)
                fieldId = 1;

            if (bookDto.FieldId == FieldStatus.C)
                fieldId = 2;

            if (bookDto.FieldId == FieldStatus.PC)
                fieldId = 3;

            if (bookDto.FieldId == FieldStatus.S)
                fieldId = 4;

            if (bookDto.FieldId == FieldStatus.M)
                fieldId = 5;

            if (bookDto.FieldId == FieldStatus.O)
                fieldId = 6;
            
            await _Book.AddAsync(Book, cancellationToken);
            if (bookDto.FieldId != FieldStatus.N)
            {
                var field = await _filedRepository.TableNoTracking.Where(c => c.Id == fieldId).SingleOrDefaultAsync(cancellationToken);
                if (field == null)
                {
                    throw new BadRequestException("رشته مورد نظر یافت نشد");
                }
                Book.FieldId = fieldId;
                await _Book.UpdateAsync(Book, cancellationToken);
            }
            else
            {
                await _Book.UpdateAsync(Book, cancellationToken);
            }
               
            List<Isbn> isbns = new List<Isbn>();
            foreach (string item in bookDto.BooksISBN)
            {
                isbns.Add(new Isbn { BookId = Book.Id, Value = item });
            }

            await _isbnRepository.AddRangeAsync(isbns, cancellationToken);
            BookSelectDto resultDto = await _repository.TableNoTracking.Include(c => c.ISBNs).ProjectTo<BookSelectDto>().SingleOrDefaultAsync(p => p.Id.Equals(Book.Id), cancellationToken);

            return resultDto;
        }


        /// <summary>
        /// لیست تمامی کتاب ها
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<BookSelectDto>> GetAllBookAsync(CancellationToken cancellationToken)
        {
            List<BookSelectDto> result = await _Book.TableNoTracking.Include(c => c.ISBNs).Where(c => c.BookIsDeleted == false && c.ISBNs.Where(d => d.IsDeleted == false).Count() > 0).OrderByDescending(c => c.Id).ProjectTo<BookSelectDto>().ToListAsync(cancellationToken);
            return result;
        }


        /// <summary>
        /// لیست تمامی کتاب های جستجو شده و دسته بندی آن ها
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="pagable"></param>
        /// <param name="FieldId"></param>
        /// <returns></returns>
        public async Task<List<BookSelectDto>> GetAllBook4AnyField(CancellationToken cancellationToken, Pagable pagable, int FieldId)
        {
            List<BookSelectDto> model = await _Book.TableNoTracking
                              .Where(p => p.BookIsDeleted == false &&
                                p.AuthorName.Contains(pagable.Search.Trim()) &&
                                p.Edition.ToString().Contains(pagable.Search.Trim()) &&
                                p.Publisher.Contains(pagable.Search.Trim()) &&
                                p.PublishYear.ToString().Contains(pagable.Search.Trim()) &&
                                p.Name.Contains(pagable.Search.Trim()) &&
                                p.Language.ToString().Contains(pagable.Search.Trim())).ProjectTo<BookSelectDto>().ToListAsync(cancellationToken);
            return model;
        }

        public async Task<bool> BookExists(BookDto bookDto, CancellationToken cancellationToken)
        {
            bool res1 = await _Book.TableNoTracking
                .Where(c => c.BookIsDeleted == false &&
                           (c.AuthorName == bookDto.AuthorName &&
                            c.Edition == bookDto.Edition &&
                            c.Language == bookDto.Language &&
                            c.Name == bookDto.Name &&
                            c.Publisher == bookDto.Publisher
                           )).AnyAsync(cancellationToken);
            if (res1 == true)
            {
                return true;
            }

            foreach (string item in bookDto.BooksISBN)
            {
                foreach (Book item1 in await _repository.TableNoTracking.Include(c => c.ISBNs).ToListAsync(cancellationToken))
                {
                    if (item1.ISBNs.Where(c => c.IsDeleted == false).Select(c => c.Value).Contains(item))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public async Task<bool> BookExists(int bookId, CancellationToken cancellationToken)
        {
            return await _repository.TableNoTracking.Where(c => c.Id == bookId).AnyAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            Book model = await _Book.Table.Where(c => c.Id == id).SingleOrDefaultAsync(cancellationToken);
            if (model == null)
            {
                return false;
            }

            model.BookIsDeleted = true;
            await _Book.UpdateAsync(model, cancellationToken);
            return true;
        }

        public async Task<bool> DeleteOneBookAsync(string isbn, CancellationToken cancellationToken)
        {
            Book model = await _Book.Table.Where(c => c.ISBNs.Where(s => s.Value == isbn) != null).SingleOrDefaultAsync(cancellationToken);
            if (model == null)
            {
                return false;
            }

            if (model.ISBNs.Count <= 1)
            {
                model.BookIsDeleted = true;
                await _Book.UpdateAsync(model, cancellationToken);
                return true;
            }
            Isbn _isbn = await _isbnRepository.Table.Where(c => c.Value == isbn).SingleOrDefaultAsync(cancellationToken);
            if (_isbn == null)
            {
                return false;
            }
            await _isbnRepository.DeleteAsync(_isbn, cancellationToken);
            await _Book.UpdateAsync(model, cancellationToken);
            return true;

        }

        public async Task<BookSelectDto> FindBookByIdAsync(int id, CancellationToken cancellationToken)
        {
            BookSelectDto model = await _Book.TableNoTracking.Where(c => c.Id == id && c.BookIsDeleted == false).ProjectTo<BookSelectDto>().SingleOrDefaultAsync(cancellationToken);
            return model;
        }

        public async Task<bool> EditAsync(BookDto bookDto, CancellationToken cancellationToken)
        {
            //TODO : قابلیت ویرایش شابک را حذف کن
            bool exists = await _Book.Table.Where(c => c.Id == bookDto.Id).AnyAsync(cancellationToken);
            if (exists != true)
            {
                return false;
            }

            Book Book = bookDto.ToEntity();
            await _Book.UpdateAsync(Book, cancellationToken);
            return true;
        }

        public async Task<BookDto> FindBookById4EditAsync(int id, CancellationToken cancellationToken)
        {
            BookDto model = await _Book.Table.Include(c => c.ISBNs).Where(c => c.Id == id && c.BookIsDeleted == false).ProjectTo<BookDto>().SingleOrDefaultAsync(cancellationToken);
            var book= await _Book.Table.Include(c => c.ISBNs).Where(c => c.Id == id && c.BookIsDeleted == false).SingleOrDefaultAsync(cancellationToken);


            FieldStatus fieldId = 0;
            if (book.FieldId ==1)
                fieldId =FieldStatus.N ;

            if (book.FieldId == 2)
                fieldId = FieldStatus.C;

            if (book.FieldId ==3)
                fieldId = FieldStatus.PC;

            if (book.FieldId == 4)
                fieldId = FieldStatus.S;

            if (book.FieldId ==5)
                fieldId = FieldStatus.M;

            if (book.FieldId == 6)
                fieldId = FieldStatus.O;

            model.FieldId = fieldId;
            return model;
        }

        public async Task<List<BookSelectDto>> GetAllBookAsync(CancellationToken cancellationToken,BookStatus bookStatus, Language language, int Field, string search)
        {
            IQueryable<Book> models = _Book.TableNoTracking.Include(c => c.ISBNs);


            if (search != null)
            {
                search = search.Trim();
                models = models.Where(p => p.BookIsDeleted == false && (p.AuthorName.Contains(search) ||
                                  p.Edition.ToString().Contains(search) ||
                                  p.Publisher.Contains(search) ||
                                  p.Name.Contains(search)) &&
                                  (language != Language.None ? p.Language == language : true) &&
                                  (Field != 0 ? p.FieldId == Field : true) &&
                                  (bookStatus != BookStatus.None ? p.BookStatus == bookStatus : true));
            }
            else if (search == null)
            {
                models = models.Where(p => p.BookIsDeleted == false &&
                                  (language != Language.None ? p.Language == language : true) &&
                                  (Field != 0? p.FieldId == Field : true) &&
                                  (bookStatus != BookStatus.None ? p.BookStatus == bookStatus : true));
            }

            return await models.OrderByDescending(c => c.Id).ProjectTo<BookSelectDto>().ToListAsync(cancellationToken);


        }
    }
}