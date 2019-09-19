using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Areas.Admin.Models;

namespace newsSite90tv.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Authorize(Roles = "Admin,Personel")]

    public class HomeController : Controller
    {
        private readonly IRepository<Book> _bRepository;
        private readonly IRepository<User> _uRepository;
        private readonly UserManager<User> _userManager;

        public HomeController(IRepository<Book> bRepository, IRepository<User> uRepository, UserManager<User> userManager)
        {
            this._bRepository = bRepository ?? throw new ArgumentNullException(nameof(bRepository));
            this._uRepository = uRepository ?? throw new ArgumentNullException(nameof(uRepository));
            this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        [Route("")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var personels= await _userManager.GetUsersInRoleAsync("Personel");
            var students= await _userManager.GetUsersInRoleAsync("Student");
            var teachers= await _userManager.GetUsersInRoleAsync("Teacher");
            var model = new IndexModel
            {
                AllBook = await _bRepository.TableNoTracking.Where(c => c.BookIsDeleted == false).CountAsync(cancellationToken),

                BorrowedBook = await _bRepository.TableNoTracking.Where(c => c.BookIsDeleted == false && c.BookStatus == BookStatus.Borrowed).CountAsync(cancellationToken),

                FreeBook = await _bRepository.TableNoTracking.Where(c => c.BookIsDeleted == false && c.BookStatus == BookStatus.Free).CountAsync(cancellationToken),

                ReserveBook = await _bRepository.TableNoTracking.Where(c => c.BookIsDeleted == false && c.BookStatus == BookStatus.Reserved).CountAsync(cancellationToken),

            };
            //personel
            if (personels!=null&&personels.Count>0)
            {
                model.Personel = personels.Where(c => c.Deleted == false).Count();
            }

            //teacher
            if (teachers != null && teachers.Count > 0)
            {
                model.Teacher = teachers.Where(c => c.Deleted == false).Count();
            }

            //student
            if (students != null && students.Count > 0)
            {
                model.AllStudent = students.Where(c => c.Deleted == false).Count();
                //model.BorrowedStudent = students.Where(c => c.Deleted == false&&c.re).Count();
            }

            return View(model);
        }
    }
}