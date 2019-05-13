using Common;
using Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Web.Controllers
{

    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        [Route("Error/{code:int}")]
        public IActionResult Error(int code)
        {
            ViewBag.Code =StatusCodes.Status400BadRequest;
            ViewBag.Message = "خطایی رخ داده است";
            ViewBag.Title = $"خطای {400}";

            var errorCode = code;
            if (errorCode==404)
            {
                ViewBag.Code = StatusCodes.Status404NotFound;
                ViewBag.Message = "صفحه مورد نظر یافت نشد";
                ViewBag.Title = $"خطای {404}";
            }

            if (errorCode == 500)
            {
                ViewBag.Code = StatusCodes.Status500InternalServerError;
                ViewBag.Message = "خطای سرور رخ داده است";
                ViewBag.Title = $"خطای {500}";
            }

            return View();
        }

    }
}