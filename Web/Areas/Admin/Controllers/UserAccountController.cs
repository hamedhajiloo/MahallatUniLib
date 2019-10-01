using Common;
using Common.Enums;
using Common.Utilities;
using Data.Repositories;
using Entities;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Dto;
using Services.Services.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Filters;
using Microsoft.AspNetCore.Identity;

namespace Web.Areas.Admin.Controllers
{
    [Route("[area]/[controller]/[action]")]
    [Area("Admin")]
    [Authorize(Roles = "Admin,Personel")]
    [Controller]
    public class UserAccountController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UserAccountController(UserManager<User> userManager)
        {
            this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        public ActionResult ChangeUserPassword(string id)
        {
            ViewBag.UserId = id;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangeUserPassword(ChangePasswordVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var userId = vm.UserId;

            var user = await _userManager.FindByIdAsync(userId);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, vm.Password);
            return RedirectToAction("Index", controllerName: "Home");
        }
    }
}