using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;



namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        public async Task<IActionResult> Index([FromHeader]CancellationToken cancellationToken)
        {
            var model = await _studentService.GetAllAsync(cancellationToken); 
            return View(model);
        }
    }
}