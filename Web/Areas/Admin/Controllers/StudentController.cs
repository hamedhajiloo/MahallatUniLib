using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Dto;
using Services.Enum.ServicesResult.Student;
using System.Threading;
using System.Threading.Tasks;
using Entities;



namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly UserManager<User> _userManager;

        public StudentController(IStudentService studentService,UserManager<User> userManager)
        {
            _studentService = studentService;
            this._userManager = userManager;
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentDto studentDto, [FromHeader]CancellationToken cancellationToken)
        {
            var existsStudentCode = await _studentService.GetByStudentNumberAsync(cancellationToken, studentDto.UserName);
            if (existsStudentCode != null)
            {
                TempData["Error"] = "شماره دانشجویی تکراری است";
                return View(studentDto);
            }

            var existsStudentId = await _userManager.FindByIdAsync(studentDto.Id);
            if (existsStudentId != null)
            {
                TempData["Error"] = "این دانشجو از قبل موجود است";
                return View(studentDto);
            }

            var existsStudentUserName = await _userManager.FindByNameAsync(studentDto.UserName);
            if (existsStudentUserName != null)
            {
                TempData["Error"] = "این شماره دانشجویی قبلا ثبت شده است";
                return View(studentDto);
            }
            var stStatus = studentDto.UserName.Substring(6, 2);
          
            var user = new User
            {
                UserName = studentDto.UserName,
                FullName = studentDto.FullName
            };

            var result = await _userManager.CreateAsync(user, "111111");
            if (result != IdentityResult.Success)
            {
                TempData["Error"] = "مشکلی در ثبت نام به وجود آمده است";
                return View(studentDto);
            }

            var student = studentDto.ToEntity();

            studentDto.UserId = user.Id;
            studentDto.EntryYear = int.Parse(studentDto.UserName.Substring(0, 2));
           

            if (stStatus == "11")
                studentDto.StudentStatus = Common.Enums.StudentStatus.Daily;
            else
                studentDto.StudentStatus = Common.Enums.StudentStatus.Nightly;

            var studentResult = await _studentService.AddAsync(cancellationToken, studentDto);
            if (studentResult == AddAsyncStatus.Exists)
            {
                TempData["Error"] = "این دانشجو از قبل موجود بوده است";
                return View(studentDto);
            }

            if (studentResult == AddAsyncStatus.Bad)
            {
                TempData["Error"] = "مشکلی در ثبت نام به وجود آمده است";
                return View(studentDto);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Student");
            if (roleResult != IdentityResult.Success)
            {
                TempData["Error"] = "مشکلی در ثبت نام به وجود آمده است";
                return View(studentDto);
            }
            TempData["Success"] = $"ثبت نام {user.FullName} با موفیت انجام شد";
            return LocalRedirect("/Admin/Student/Index");
        }


    }
}