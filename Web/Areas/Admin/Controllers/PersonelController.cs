using Common;
using Data.Repositories;
using Entities;
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
    public class PersonelController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<User> _uRepository;

        public PersonelController(UserManager<User> userManager,
                                  IRepository<User> uRepository)
        {
            _userManager = userManager;
            this._uRepository = uRepository;
        }
        public async Task<IActionResult> Index([FromHeader]CancellationToken cancellationToken)
        {
            var model = await _userManager.GetUsersInRoleAsync("Personel");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string gPersonelId, [FromHeader] CancellationToken cancellationToken)
        {
            var user = await _uRepository.Table.Where(c => c.Id == gPersonelId).SingleOrDefaultAsync(cancellationToken);
            await _uRepository.DeleteAsync(user,cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, [FromHeader]CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id);
            return View(user);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeacherDto teacherDto, [FromHeader]CancellationToken cancellationToken)
        {
            var user1 = await _uRepository.TableNoTracking.Where(c => c.UserName == teacherDto.NationalCode).SingleOrDefaultAsync(cancellationToken);
            if (user1 != null)
            {
                TempData["Error"] = "پرسنلی با این کد ملی قبلا ذخیره شده است";
                return View(teacherDto);
            }



            User user = new User
            {
                FullName = teacherDto.UserFullName,
                UserName=teacherDto.NationalCode
            };

            IdentityResult result = await _userManager.CreateAsync(user, "111111");
            if (result != IdentityResult.Success)
            {
                TempData["Error"] = "مشکلی در ثبت نام به وجود آمده است";
                return View(teacherDto);
            }


          

            IdentityResult roleResult = await _userManager.AddToRoleAsync(user, "Personel");
            if (roleResult != IdentityResult.Success)
            {
                TempData["Error"] = "مشکلی در ثبت نام به وجود آمده است";
                return View(teacherDto);
            }
            TempData["Success"] = $"ثبت نام {user.FullName} با موفیت انجام شد";
            return LocalRedirect("/Admin/Personel/Index");
        }

        [HttpPost]
        public async Task<IActionResult> Search(CancellationToken cancellationToken, Pagable pagable)
        {
            var search = pagable.Search;
            if (string.IsNullOrEmpty(search))
            {
                var teacher= await _uRepository.TableNoTracking.ToListAsync(cancellationToken);
                return Json(new { data = teacher });
            }
            var model= await _uRepository.TableNoTracking
                .Where(c =>
                c.FullName.Contains(search) ||
                c.UserName.Contains(search)).ToListAsync(cancellationToken);
            return Json(new { data = model });
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id, [FromHeader]CancellationToken cancellationToken)
        {
            ViewBag.ViewTitle = "فرم ویرایش اطلاعات پرسنل";
            var model = await _userManager.FindByIdAsync(id);
            if (model == null)
            {
                TempData["Error"] = "این پرسنل در سیستم وجود ندارد";
                return RedirectToAction(nameof(Index));
            }

            var res = new TeacherDto { UserId = model.Id, UserFullName = model.FullName,NationalCode=model.UserName };
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

                var otherUser = await _uRepository.TableNoTracking.Where(c => c.Id != user.Id && ( c.UserName == teacherDto.NationalCode)).SingleOrDefaultAsync(cancellationToken);
                if (otherUser != null)
                {
                    TempData["Error"] = "این پرسنل در سیستم وجود دارد";
                    return View(teacherDto);
                }

                user.FullName = teacherDto.UserFullName;
                user.UserName = teacherDto.NationalCode;
                await _userManager.UpdateAsync(user);
                TempData["Success"] = "تبریک . ویرایش با موفقیت انجام شد";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "مشکلی در ثبت اطلاعات رخ داد";
            return LocalRedirect("/Admin/Personel/Index");
        }

    }
}