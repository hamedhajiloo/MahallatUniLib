using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Dto;
using WebFramework.Api;
using WebFramework.Filters;

namespace Web.Controllers
{
   [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IBookService _bookService;

        public HomeController(IBookService bookService)
        {
            _bookService = bookService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Alaki(CancellationToken cancellationToken,Language lan,CourseType ctype,int field,BookStatus bstatus,Pagable pagable)
        {
            var model = await _bookService.GetAllBookAsync(cancellationToken,ctype,bstatus,lan,field,pagable.Search);
            return Ok(model);
        }

    }
}