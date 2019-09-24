using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using DNTPersianUtils.Core;
using Common.Enums;
using Data.Repositories;
using Entities;
using Entities.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Dto;
using Web.Model;
using WebFramework.Api;
using WebFramework.Filters;
using Microsoft.AspNetCore.Identity;

namespace Web.Controllers
{
    [Authorize(Roles ="Student")]

    public class HomeController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IRepository<Field> _fieldRepository;
        private readonly IRepository<News> _nRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly UserManager<User> _userManager;

        public HomeController(IBookService bookService,
                              IRepository<Field> fieldRepository,
                              IRepository<News> nRepository,
                              IRepository<Book> bookRepository,
                              UserManager<User> userManager)
        {
            _bookService = bookService;
            _fieldRepository = fieldRepository ?? throw new ArgumentNullException(nameof(fieldRepository));
            this._nRepository = nRepository;
            _bookRepository = bookRepository;
            this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var pagable = new Pagable
            {
                Desc = true,
                Page = 1,
                PageSize = 10
            };

            var vm = new HomeIndexVM();

            vm.Fields = await _fieldRepository.TableNoTracking.ToListAsync(cancellationToken);
            vm.General = await _bookRepository.TableNoTracking.Where(c => c.FieldId==1 &&c.BookIsDeleted==false).Take(10).ToListAsync(cancellationToken);
            vm.ComputerBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldId == 2 && c.BookIsDeleted == false).Take(10).ToListAsync(cancellationToken);
            vm.UlomBooks = await _bookRepository.TableNoTracking.Where(c =>c.FieldId==3 && c.BookIsDeleted==false).Take(10).ToListAsync(cancellationToken);
            vm.SanayeBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldId == 4 && c.BookIsDeleted==false).Take(10).ToListAsync(cancellationToken);
            vm.MechanickBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldId == 5 && c.BookIsDeleted==false).Take(10).ToListAsync(cancellationToken);
            vm.OmranBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldId == 6 && c.BookIsDeleted==false).Take(10).ToListAsync(cancellationToken);
            vm.News = await _nRepository.TableNoTracking.Where(c => c.Deleted == false).OrderByDescending(c => c.InsertDate).Take(10).ToListAsync();
            foreach (var item in vm.News)
                item.InserDateP = item.InsertDate.ToFriendlyPersianDateTextify(true);
          
            return View(vm);
        }

      
        public async Task<IActionResult> DashBoard(CancellationToken cancellationToken)
        {
            var userId = User.Identity.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.FullName = user.FullName;
            return View();
        }
       
    }
}