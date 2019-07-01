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
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _repository;
        private readonly IRepository<BookList> _bookList;

        public BookService(IRepository<Book> repository, IRepository<BookList> bookList)
        {
            _repository = repository;
            _bookList = bookList;
        }

        public async Task<BookSelectDto> AddBookAsync(BookDto bookDto, CancellationToken cancellationToken)
        {
            var bookList = bookDto.ToEntity();
            await _bookList.AddAsync(bookList, cancellationToken, saveNow: false);

            foreach (var item in bookDto.BooksISBN)
            {
                var book = new Book()
                {
                    BookListId = bookList.Id,
                    ISBN = item
                };
                await _repository.AddAsync(book, cancellationToken);
            }


            var resultDto = await _repository.TableNoTracking.ProjectTo<BookSelectDto>().SingleOrDefaultAsync(p => p.Id.Equals(bookList.Id), cancellationToken);

            return resultDto;
        }

        public async Task<List<BookSelectDto>> GetAllBookAsync(CancellationToken cancellationToken)
        {
            var result = await _bookList.TableNoTracking.Where(c => c.BookListIsDeleted == false).OrderByDescending(c => c.Id).ProjectTo<BookSelectDto>().ToListAsync(cancellationToken);
            return result;
        }

        public async Task<List<BookSelectDto>> GetAllBook4AnyField(CancellationToken cancellationToken, Pagable pagable, int FieldId)
        {
            var model = await _bookList.TableNoTracking
                              .Where(p => p.BookListIsDeleted == false &&
                                p.AuthorName.Contains(pagable.Search.Trim()) &&
                                p.Edition.ToString().Contains(pagable.Search.Trim()) &&
                                p.Publisher.Contains(pagable.Search.Trim()) &&
                                p.PublishYear.ToString().Contains(pagable.Search.Trim()) &&
                                p.Name.Contains(pagable.Search.Trim()) &&
                                p.Language.ToString().Contains(pagable.Search.Trim()) &&
                                p.FieldBookList.Select(c => c.FieldId).Contains(FieldId)).ProjectTo<BookSelectDto>().ToListAsync(cancellationToken);
            return model;
        }

        public async Task<bool> BookExists(BookDto bookDto, CancellationToken cancellationToken)
        {
            var res1 = await _bookList.TableNoTracking
                .Where(c => c.BookListIsDeleted == false &&
                           (c.AuthorName == bookDto.AuthorName &&
                            c.Edition == bookDto.Edition &&
                            c.Language == bookDto.Language &&
                            c.Name == bookDto.Name &&
                            c.Publisher == bookDto.Publisher
                           )).AnyAsync(cancellationToken);
            if (res1 == true)
                return true;

            foreach (var item in bookDto.BooksISBN)
            {
                foreach (var item1 in _repository.TableNoTracking)
                {
                    if (item1.ISBN == item)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var model = await _bookList.Table.Where(c => c.Id == id).SingleOrDefaultAsync(cancellationToken);
            if (model == null) return false;
            model.BookListIsDeleted = true;
            await _bookList.UpdateAsync(model, cancellationToken);
            return true;
        }

        public async Task<BookSelectDto> FindBookByIdAsync(int id, CancellationToken cancellationToken)
        {
            var model = await _bookList.TableNoTracking.Where(c => c.Id == id && c.BookListIsDeleted == false).ProjectTo<BookSelectDto>().SingleOrDefaultAsync(cancellationToken);
            return model;
        }

        public async Task<bool> EditAsync(BookDto bookDto, CancellationToken cancellationToken)
        {
            var exists = await _bookList.Table.Where(c => c.Id == bookDto.Id).AnyAsync(cancellationToken);
            if (exists != true) return false;
            var bookList = bookDto.ToEntity();
            await _bookList.UpdateAsync(bookList, cancellationToken);
            return true;
        }

        public async Task<BookDto> FindBookById4EditAsync(int id, CancellationToken cancellationToken)
        {
            var model = await _bookList.Table.Where(c => c.Id == id && c.BookListIsDeleted == false).ProjectTo<BookDto>().SingleOrDefaultAsync(cancellationToken);
            var books = await _repository.Table.Where(c=>c.BookListId==id).ToListAsync(cancellationToken);
            var isbns = books.Select(c => c.ISBN);
            try
            {
                //IList<string> isbnList = new List<string>();
                //foreach (var item in isbns)
                //{
                //    isbnList.Add(item);
                //}
                //foreach (var item in isbns)
                //{
                //    model.BooksISBN.Add(item);
                //}
                model.BooksISBN = isbns.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
            return model;
        }

        public async Task<List<BookSelectDto>> GetAllBookAsync(CancellationToken cancellationToken, CourseType courseType, BookStatus bookStatus, Language language, int Field, string search)
        {
            var models = _bookList.TableNoTracking;


            if (search != null) { 
                search = search.Trim();
                models = models.Where(p => p.BookListIsDeleted == false && (p.AuthorName.Contains(search) ||
                                  p.Edition.ToString().Contains(search) ||
                                  p.Publisher.Contains(search) ||
                                  p.Name.Contains(search)) &&
                                  (courseType != CourseType.None ? p.CourseType == courseType : true) &&
                                  (language != Language.None ? p.Language == language : true) &&
                                  (bookStatus != BookStatus.None ? p.BookStatus == bookStatus : true));
            }
            else if (search == null)
                models = models.Where(p => p.BookListIsDeleted == false && (courseType != CourseType.None ? p.CourseType == courseType : true) &&
                                  (language != Language.None ? p.Language == language : true) &&
                                  (bookStatus != BookStatus.None ? p.BookStatus == bookStatus : true));

            return await models.OrderByDescending(c => c.Id).ProjectTo<BookSelectDto>().ToListAsync(cancellationToken);


        }
    }
}