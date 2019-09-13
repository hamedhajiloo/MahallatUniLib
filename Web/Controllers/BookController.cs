using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using Web.Model;

namespace Web.Controllers
{
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
            var pagable = new Pagable
            {
                Desc = true,
                Page = 1,
                PageSize = 10
            };

            var books = await _bookRepository.TableNoTracking.Where(c => c.FieldId == id && c.BookIsDeleted == false).Take(10).ToListAsync(cancellationToken);


            return View(books);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Details(int id, [FromHeader]CancellationToken cancellationToken)
        {
            var model = await _bookService.FindBookByIdAsync(id, cancellationToken);
            return View(model);
        }


    }
}