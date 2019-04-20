using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using newsSite90tv.Models;
using newsSite90tv.Models.UnitOfWork;
using newsSite90tv.Models.ViewModels;

namespace newsSite90tv.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _context;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly UserManager<ApplicationUsers> _userManager;

        public UserController(IUnitOfWork context,
            IHostingEnvironment appEnvironment,
            UserManager<ApplicationUsers> userManager)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewBag.ViewTitle = "لیست کاربران";
            var model = _context.userManagerUW.Get();
            return View(model);
        }

        public async Task<IActionResult> UploadFile(IEnumerable<IFormFile> files)
        {

            var upload = Path.Combine(_appEnvironment.WebRootPath, "upload\\userimage\\normalimage\\");
            var filename = "";
            foreach (var file in files)
            {
                filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                using (var fs = new FileStream(Path.Combine(upload, filename), FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }
            }

            /////////تغییر سایز عکس و ذخیره
            InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer();
            img.Resize(upload + filename, _appEnvironment.WebRootPath + "\\upload\\userimage\\thumbnailimage\\" + filename);

            return Json(new { status = "success", message = "تصویر با موفقیت آپلود شد.", imagename = filename });

        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.ViewTitle = "فرم افزودن کاربر";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model, string imagename)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (imagename == null)
                    {
                        model.UserImage = "defaultuserImage.png";
                    }
                    else
                    {
                        model.UserImage = imagename;
                    }

                    ApplicationUsers user = new ApplicationUsers
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        UserName = model.UserName,
                        Email = model.Email,
                        gender = model.gender,
                        BirthDayDate = model.BirthDayDate,
                        UserImagePath = model.UserImage
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                }
                catch
                {
                    throw;
                }
            }
            ViewBag.ViewTitle = "فرم ایجاد کاربر";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {

            EditUserViewModel model = new EditUserViewModel();
            ApplicationUsers user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Email = user.Email;
                model.PhoneNumber = user.PhoneNumber;
                model.BirthDayDate = user.BirthDayDate;
                model.gender = user.gender;
                model.UserImage = user.UserImagePath;
            }


            ViewBag.ViewTitle = "فرم ویرایش کاربر";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model, string id,
            string imagename,string chkinput)
        {
            if (ModelState.IsValid)
            {
                //Update
                ApplicationUsers user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Email = model.Email;
                    user.gender = model.gender;
                    user.BirthDayDate = model.BirthDayDate;
              //      if (imagename == null) user.UserImagePath = "defaultuserImage.png";
                    if (imagename != null) user.UserImagePath = imagename;
                    
                    if (chkinput == "on")
                    {
                        //Reset Password
                        //123d@F
                        user.PasswordHash = "AQAAAAEAACcQAAAAEAabKLaDOcVF55N+pqYxMKEsctUlZmrmudfUurx8DtbxZcPv0wXbujbbfg3g2LrYrg==";
                    }

                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                }
            }

            ViewBag.ViewTitle = "فرم ویرایش کاربر";
            return View(model);
        }

    }
}