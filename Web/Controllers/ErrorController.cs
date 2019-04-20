using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("[controller]/Index")]
    public class ErrorController : Controller
    {
        [HttpGet("{httpStatusCode:int}")]
        public IActionResult Index(int httpStatusCode)
        {
            ViewBag.Error = httpStatusCode;
            return View();
        }


    }
}