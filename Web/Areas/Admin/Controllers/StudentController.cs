using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Enum.ServicesResult.Student;
using System.Threading;
using System.Threading.Tasks;



namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
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

        [HttpPost]
        public async Task<IActionResult> Delete(string studentId, [FromHeader] CancellationToken cancellationToken)
        {
            var res = await _studentService.DeleteAsync(studentId, cancellationToken);
            if (res == DeleteAsyncStatus.Ok)
            {
                TempData["War"] = "کتاب مورد نظر حذف شد";
                return RedirectToAction(nameof(Index));
            }
            else if (res == DeleteAsyncStatus.NonExists)
            {
                TempData["Error"] = "کتاب مورد نظر یافت نشد";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "مشکلی در حذف کتاب به وجود آمد";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, [FromHeader]CancellationToken cancellationToken)
        {
            var model = await _studentService.FindByIdAsync(id, cancellationToken);
            var res = model.Data;
            if (res == null)
            {
                TempData["Error"] = "کتاب مورد نظر یافت نشد";
                return RedirectToAction(nameof(Index));
            }
            return View(res);
        }

     

    }
}