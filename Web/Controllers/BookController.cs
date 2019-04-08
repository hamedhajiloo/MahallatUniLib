using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFramework.Filters;

namespace Web.Controller
{
    [Route("api/[controller]")]
    [ApiResultFilter]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IRepository<Book> repository;

        public BookController(IRepository<Book> repository )
        {
            this.repository = repository;
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result =await repository.TableNoTracking.ToListAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Book book)
        {
            var cancellationToken = HttpContext.RequestAborted;
          await repository.AddAsync(book, cancellationToken);
            return Ok();
        }
    }
}