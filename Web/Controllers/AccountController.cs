using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Model;
using Entities;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    //[Authorize("User")]
    public class AccountController : Controller
    {
        private readonly SignInManager<Entities.User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(SignInManager<Entities.User> signInManager, UserManager<User> userManager)
        {
            this._signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("/Account/Login",Name ="Login")]
        [HttpGet]
        public ActionResult Login(string ReturnUrl=null)
        {
            ////
            // var user = new User
            //    {
            //        FullName = "Administrator",
            //        UserName = "Administrator"
            //    };
            //    _userManager.CreateAsync(user, "Admin@12b#");
            //    _userManager.AddToRoleAsync(user, "Admin");
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction(actionName: "Index", controllerName: "Home", new { area = "Admin" });
                }
                return LocalRedirect("/");
            }
            ReturnUrl = ReturnUrl ?? Url.Content("~/");
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [Route("/Account/Login",Name ="Login")]
        [HttpPost]
        public async Task<ActionResult> Login(LoginVM loginVM,CancellationToken cancellationToken)
        {
            
            loginVM.ReturnUrl = loginVM.ReturnUrl ?? Url.Content("~/");
            ViewBag.ReturnUrl = loginVM.ReturnUrl;
            var result = await _signInManager.PasswordSignInAsync(loginVM.UserName,loginVM.Password,true, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(loginVM.UserName);
                
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction(actionName: "Index", controllerName: "Home", new { area = "Admin" });
                }

                if (await _userManager.IsInRoleAsync(user, "Personel"))
                {
                    return RedirectToAction(actionName: "Index", controllerName: "Home", new { area = "Admin" });
                }
                return LocalRedirect(loginVM.ReturnUrl);
            }
            
            else
            {
                TempData["Error"] = "مشکلی در ورود به سایت رخ داده است";
                return View();
            }
        }


        [Route("/Account/AccessDenied", Name = "AccessDenied")]
        [HttpGet]
        public ActionResult AccessDenied( CancellationToken cancellationToken,string returnUrl)
        {


            return View();
        }

        [Route("LogOut")]
        [HttpGet]
        public async Task<ActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToRoute(routeName: "Login");
        }


    }
}