using Common;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize(Roles = "Student")]
    [Route("Book/[action]")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IRepository<Field> _fieldRepository;
        private readonly IRepository<ReserveBook> _rbRepository;
        private readonly IRepository<Book> _bookRepository;

        public BookController(IBookService bookService,
                              IRepository<Field> fieldRepository,
                              IRepository<ReserveBook> rbRepository,
                              IRepository<Book> bookRepository)
        {
            _bookService = bookService;
            _fieldRepository = fieldRepository;
            this._rbRepository = rbRepository ?? throw new System.ArgumentNullException(nameof(rbRepository));
            _bookRepository = bookRepository;
        }


        public async Task<ActionResult> Index(int id, CancellationToken cancellationToken)
        {
            Pagable pagable = new Pagable
            {
                Desc = true,
                Page = 1,
                PageSize = 10
            };

            List<Book> books = await _bookRepository.TableNoTracking.Where(c => c.FieldId == id && c.BookIsDeleted == false).Take(10).ToListAsync(cancellationToken);
         
            return View(books);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Details(int id, [FromHeader]CancellationToken cancellationToken)
        {
            Services.Dto.BookSelectDto model = await _bookService.FindBookByIdAsync(id, cancellationToken);
            return View(model);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Reserve(int id, [FromHeader]CancellationToken cancellationToken)
        {
            var userId = User.Identity.GetUserId();
            var book = await _bookRepository.TableNoTracking
                .Where(c => c.Id == id && c.BookIsDeleted == false).SingleOrDefaultAsync(cancellationToken);

            var rb = await _rbRepository.TableNoTracking.Where(c => c.UserId == userId && c.BookId == id).SingleOrDefaultAsync(cancellationToken);
            var message = "قبلا این کتاب را رزرو کرده اید";
            if (rb!=null)
            {
                return this.Json(new { message = message });
            }

            var newrb = new ReserveBook
            {
                BookId = id,
                BookStatus = Common.Enums.BookStatus.Reserved,
                ReserveDate = DateTime.Now,
                UserId = userId
            };

            await _rbRepository.AddAsync(newrb, cancellationToken);
            message = "با موفقیت رزرو شد";
            return this.Json(new { message=message });
        }


    }
}