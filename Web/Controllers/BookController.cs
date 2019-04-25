using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Dto;
using WebFramework.Filters;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiResultFilter]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpGet]
        public async Task<ActionResult<List<BookSelectDto>>> Get([FromQuery]CancellationToken cancellationToken)
        {

            var result =await _bookService.GetAllBookAsync(cancellationToken);
            return Ok(result);

        }

        //[HttpPost]
        //public async Task<ActionResult> Create(BookDto bookDto, [FromHeader]CancellationToken cancellationToken)
        //{
        // var result= await _bookService.AddBookAsync(bookDto, cancellationToken);
        //    return Ok(result);
        //}
    }
}