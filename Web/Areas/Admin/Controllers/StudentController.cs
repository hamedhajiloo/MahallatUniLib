using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Dto;
using Services.Enum.ServicesResult.Student;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using Common;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Data.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
    [Authorize(Roles = "Admin,Personel")]

    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<User> _uRepository;

        public StudentController(IStudentService studentService,UserManager<User> userManager,IRepository<User> uRepository)
        {
            _studentService = studentService;
            this._userManager = userManager;
            this._uRepository = uRepository;
        }
        public async Task<IActionResult> Index([FromHeader]CancellationToken cancellationToken)
        {
            var model = await _studentService.GetAllAsync(cancellationToken);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string gstudentid, [FromHeader] CancellationToken cancellationToken)
        {
            var res = await _studentService.DeleteAsync(gstudentid, cancellationToken);
            if (res == DeleteAsyncStatus.Ok)
            {
                TempData["War"] = "عملیات با موفقیت انجام شد";
                return RedirectToAction(nameof(Index));
            }
            else if (res == DeleteAsyncStatus.NonExists)
            {
                TempData["Error"] = "دانشجوی مورد نظر یافت نشد";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "مشکلی در حذف به وجود آمد";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, [FromHeader]CancellationToken cancellationToken)
        {
            var model = await _studentService.FindByIdAsync(id, cancellationToken);
            var res = model.Data;
            if (res == null)
            {
                TempData["Error"] = "دانشجو مورد نظر یافت نشد";
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
            try
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

                var existsStudentUserName = await _uRepository.TableNoTracking.Where(c => c.UserName == studentDto.UserName).SingleOrDefaultAsync(cancellationToken);
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

                var result = await _userManager.CreateAsync(user, studentDto.UserName);
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
            catch
            {
                TempData["Error"] = "لطفا در وارد کردن اطلاعات دقت نمایید";
                return View(studentDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(CancellationToken cancellationToken, Pagable pagable)
        {
            var model = await _studentService.GetAllAsync(cancellationToken, pagable.Search);

            return this.Json(new { data = model });
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id, [FromHeader]CancellationToken cancellationToken)
        {
            ViewBag.ViewTitle = "فرم ویرایش اطلاعات دانشجو"; 
            var model = await _studentService.GetByIdAsync( cancellationToken,id);
            if (model == null)
            {
                TempData["Error"] = "این دانشجو در سیستم وجود ندارد";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromHeader]CancellationToken cancellationToken, StudentSelectDto studentDto)
        {
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    TempData["Error"] = error.ErrorMessage;
                }
            }
            if (ModelState.IsValid)
            {
                await _studentService.EditAsync(studentDto, cancellationToken);

                TempData["Success"] = "تبریک . ویرایش با موفقیت انجام شد";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"]="مشکلی در ثبت اطلاعات رخ داد";
            return LocalRedirect("/Admin/Student/Index");
        }

    }
}