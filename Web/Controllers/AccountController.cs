using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Model;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Entities.User> _signInManager;

        public AccountController(SignInManager<Entities.User> signInManager)
        {
            this._signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("Login")]
        [HttpGet]
        public ActionResult Login(string returnUrl=null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return LocalRedirect("/");
            }
            returnUrl = returnUrl ?? Url.Content("~/");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult> Login(LoginVM loginVM,CancellationToken cancellationToken)
        {
            loginVM.ReturnUrl = loginVM.ReturnUrl ?? Url.Content("~/");
            ViewBag.ReturnUrl = loginVM.ReturnUrl;
            var result = await _signInManager.PasswordSignInAsync(loginVM.UserName,loginVM.Password,true, lockoutOnFailure: true);
            if (result.Succeeded)
            {
               // _logger.LogInformation("User logged in.");
                return LocalRedirect(loginVM.ReturnUrl);
            }
            
            else
            {
                TempData["Error"] = "مشکلی در ورود به سایت رخ داده است";
                return View();
            }
        }

        [HttpPost]
        public ActionResult AccessDenied(CancellationToken cancellationToken)
        {


            return View();
        }


    }
}