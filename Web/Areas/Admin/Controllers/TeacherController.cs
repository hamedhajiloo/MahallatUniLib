using Common;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Dto;
using Services.Enum.ServicesResult.Student;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
    [Authorize(Roles = "Admin,Personel")]

    public class TeacherController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IRepository<Teacher> _repository;
        private readonly IRepository<User> _uRepository;

        public TeacherController(UserManager<User> userManager,IUserService userService, IRepository<Teacher> repository,IRepository<User> uRepository)
        {
            _userManager = userManager;
            this._userService = userService;
            this._repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
            this._uRepository = uRepository;
        }
        public async Task<IActionResult> Index([FromHeader]CancellationToken cancellationToken)
        {
            var model = await _repository.TableNoTracking.Include(c=>c.User).Where(c=>c.User.Deleted==false).ToListAsync(cancellationToken);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string gTeacherId, [FromHeader] CancellationToken cancellationToken)
        {
            var teacher = await _repository.Table.Include(c => c.User).Where(c => c.Id == gTeacherId).SingleOrDefaultAsync(cancellationToken);
            await _userService.DeleteAsync(teacher.User.Id, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, [FromHeader]CancellationToken cancellationToken)
        {
            var teacher = await _repository.TableNoTracking.Include(c => c.User).Where(c => c.Id == id).SingleOrDefaultAsync(cancellationToken);
            return View(teacher);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeacherDto teacherDto, [FromHeader]CancellationToken cancellationToken)
        {
            var Teacher1 = await _repository.TableNoTracking.Include(c => c.User).Where(c => c.User.UserName == teacherDto.NationalCode).SingleOrDefaultAsync(cancellationToken);
            if (Teacher1 != null)
            {
                TempData["Error"] = "استادی با این کد ملی قبلا ذخیره شده است";
                return View(teacherDto);
            }

            //User existsStudentId = await _userManager.FindByIdAsync(teacherDto.UserId);
            //if (existsStudentId != null)
            //{
            //    TempData["Error"] = "این استاد از قبل موجود است";
            //    return View(teacherDto);
            //}



            User user = new User
            {
                FullName = teacherDto.UserFullName,
                UserName=teacherDto.NationalCode
            };

            IdentityResult result = await _userManager.CreateAsync(user, teacherDto.NationalCode);
            if (result != IdentityResult.Success)
            {
                TempData["Error"] = "مشکلی در ثبت نام به وجود آمده است";
                return View(teacherDto);
            }


            var teacher = new Teacher
            {
                UserId = user.Id
            };

            await _repository.AddAsync(teacher, cancellationToken);

          

            IdentityResult roleResult = await _userManager.AddToRoleAsync(user, "Teacher");
            if (roleResult != IdentityResult.Success)
            {
                TempData["Error"] = "مشکلی در ثبت نام به وجود آمده است";
                return View(teacherDto);
            }
            TempData["Success"] = $"ثبت نام {user.FullName} با موفیت انجام شد";
            return LocalRedirect("/Admin/Teacher/Index");
        }

        [HttpPost]
        public async Task<IActionResult> Search(CancellationToken cancellationToken, Pagable pagable)
        {
            var search = pagable.Search;
            if (string.IsNullOrEmpty(search))
            {
                var teacher= await _repository.TableNoTracking.Include(c => c.User).Where(c=>c.User.Deleted==false).ToListAsync(cancellationToken);
                return Json(new { data = teacher });
            }
            var model= await _repository.TableNoTracking.Include(c => c.User).Where(c=>c.User.Deleted==false)
                .Where(c =>
                c.User.FullName.Contains(search) ||
                c.User.UserName.Contains(search)).ToListAsync(cancellationToken);
            return Json(new { data = model });
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id, [FromHeader]CancellationToken cancellationToken)
        {
            ViewBag.ViewTitle = "فرم ویرایش اطلاعات استاد";
            var model = await _repository.TableNoTracking.Include(c=>c.User).Where(c => c.Id == id).SingleOrDefaultAsync(cancellationToken);
            if (model == null)
            {
                TempData["Error"] = "این استاد در سیستم وجود ندارد";
                return RedirectToAction(nameof(Index));
            }

            var res = new TeacherDto { UserId = model.User.Id, UserFullName = model.User.FullName,NationalCode=model.User.UserName };
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromHeader]CancellationToken cancellationToken, TeacherDto teacherDto)
        {
            foreach (ModelStateEntry modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    TempData["Error"] = error.ErrorMessage;
                }
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(teacherDto.UserId);

                var otherUser = await _repository.TableNoTracking.Include(c=>c.User).Where(c => c.UserId != user.Id && (c.User.UserName == teacherDto.NationalCode)).FirstOrDefaultAsync(cancellationToken);
                if (otherUser != null)
                {
                    TempData["Error"] = "این استاد در سیستم وجود دارد";
                    return View(teacherDto);
                }

                user.FullName = teacherDto.UserFullName;
                user.UserName = teacherDto.NationalCode;
                await _userManager.UpdateAsync(user);
                TempData["Success"] = "تبریک . ویرایش با موفقیت انجام شد";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "مشکلی در ثبت اطلاعات رخ داد";
            return LocalRedirect("/Admin/Teacher/Index");
        }

    }
}