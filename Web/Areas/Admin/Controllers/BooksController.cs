using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Dto;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var model = await _bookService.GetAllBookAsync(cancellationToken);
            return View(model);
        }   

        public async Task<IActionResult> Create([FromHeader]CancellationToken cancellationToken,BookDto bookDto)
        {
            var res = await _bookService.AddBookAsync(bookDto, cancellationToken);
            return Ok(res);
        }


    }
}