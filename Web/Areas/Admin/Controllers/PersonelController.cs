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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
    [Authorize(Roles = "Admin")]

    public class PersonelController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IRepository<User> _uRepository;

        public PersonelController(UserManager<User> userManager,
            IUserService userService,
                                  IRepository<User> uRepository)
        {
            _userManager = userManager;
            this._userService = userService;
            this._uRepository = uRepository;
        }
        public async Task<IActionResult> Index([FromHeader]CancellationToken cancellationToken)
        {
            var model = await _userManager.GetUsersInRoleAsync("Personel");
            IList<User> result = new List<User>();
            foreach (var user in model)
            {
                if (user.Deleted == false)
                {
                    result.Add(user);
                }
            }
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string gPersonelId, [FromHeader] CancellationToken cancellationToken)
        {
            await _userService.DeleteAsync(gPersonelId, cancellationToken);
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
                UserName = teacherDto.NationalCode
            };

            IdentityResult result = await _userManager.CreateAsync(user, teacherDto.NationalCode);
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
                var personel = await _userManager.GetUsersInRoleAsync("Personel");
                var result = new List<User>();
                foreach (var user in personel)
                {
                    if (user.Deleted == false)
                    {
                        result.Add(user);
                    }
                }

                return Json(new { data = result });
            }
            var model = await _userManager.GetUsersInRoleAsync("Personel");

            var result2 = new List<User>();
            foreach (var user in model.Where(c => (
               c.FullName.Contains(search) ||
               c.UserName.Contains(search)) && c.Deleted == false).ToList())
            {
                if (user.Deleted == false)
                {
                    result2.Add(user);
                }
            }
            return Json(new { data = result2 });
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

            var res = new TeacherDto { UserId = model.Id, UserFullName = model.FullName, NationalCode = model.UserName };
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

                var otherUser = await _uRepository.TableNoTracking.Where(c => c.Id != user.Id && (c.UserName == teacherDto.NationalCode)).SingleOrDefaultAsync(cancellationToken);
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