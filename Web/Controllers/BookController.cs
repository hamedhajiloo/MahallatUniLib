using Common;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize(Roles ="Student")]
    [Route("Book/[action]")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IRepository<Field> _fieldRepository;
        private readonly IRepository<Book> _bookRepository;

        public BookController(IBookService bookService, IRepository<Field> fieldRepository, IRepository<Book> bookRepository)
        {
            _bookService = bookService;
            _fieldRepository = fieldRepository;
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


    }
}