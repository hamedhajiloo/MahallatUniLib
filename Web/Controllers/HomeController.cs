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
    [Authorize]

    public class HomeController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IRepository<Field> _fieldRepository;
        private readonly IRepository<News> _nRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Setting> _settingRepo;
        private readonly IRepository<ReserveBook> _reserveBookRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly UserManager<User> _userManager;

        public HomeController(IBookService bookService,
                              IRepository<Field> fieldRepository,
                              IRepository<News> nRepository,
                              IRepository<User> userRepository,
                              IRepository<Setting> settingRepo,
                              IRepository<ReserveBook> reserveBookRepository,
                              IRepository<Book> bookRepository,
                              UserManager<User> userManager)
        {
            _bookService = bookService;
            _fieldRepository = fieldRepository ?? throw new ArgumentNullException(nameof(fieldRepository));
            this._nRepository = nRepository;
            this._userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this._settingRepo = settingRepo ?? throw new ArgumentNullException(nameof(settingRepo));
            this._reserveBookRepository = reserveBookRepository ?? throw new ArgumentNullException(nameof(reserveBookRepository));
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
            var setting = await _settingRepo.GetByIdAsync(cancellationToken, 1);
            var vm = new HomeIndexVM();

            vm.Fields = await _fieldRepository.TableNoTracking.ToListAsync(cancellationToken);
            vm.General = await _bookRepository.TableNoTracking.Where(c => c.FieldId == 1 && c.BookIsDeleted == false).Take(10).ToListAsync(cancellationToken);
            vm.ComputerBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldId == 2 && c.BookIsDeleted == false).Take(10).ToListAsync(cancellationToken);
            vm.UlomBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldId == 3 && c.BookIsDeleted == false).Take(10).ToListAsync(cancellationToken);
            vm.SanayeBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldId == 4 && c.BookIsDeleted == false).Take(10).ToListAsync(cancellationToken);
            vm.MechanickBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldId == 5 && c.BookIsDeleted == false).Take(10).ToListAsync(cancellationToken);
            vm.OmranBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldId == 6 && c.BookIsDeleted == false).Take(10).ToListAsync(cancellationToken);
            vm.News = await _nRepository.TableNoTracking.Where(c => c.Deleted == false).OrderByDescending(c => c.InsertDate).Take(10).ToListAsync();
            foreach (var item in vm.News)
                item.InserDateP = item.InsertDate.ToFriendlyPersianDateTextify(true);
            var userId = User.Identity.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.FullName = user.FullName;
            ViewBag.ReserveDay = setting.ReservDay;
            return View(vm);
        }

        [AllowAnonymous]
        public async Task Make()
        {
            var users = await _userRepository.TableNoTracking.ToListAsync();

            //
            if (users.Count==0)
            {
                var user = new User
                {
                    FullName = "Administrator",
                    UserName = "Administrator"
                };
                await _userManager.CreateAsync(user, "Admin@12b#");
                await _userManager.AddToRoleAsync(user, "Admin");

            }

        }

        public async Task<IActionResult> DashBoard(CancellationToken cancellationToken)
        {
            var model = new DashBoardModel();
            var userId = User.Identity.GetUserId();
            //Reserve Count
            model.ReserveCount = await _reserveBookRepository.TableNoTracking
                .Where(c => c.UserId == userId && c.BookStatus == BookStatus.Reserved).CountAsync();

            //Reserve Last Update DateTime
            if (model.ReserveCount != 0)
            {
                var lastReserve = await _reserveBookRepository.TableNoTracking
               .Where(c => c.UserId == userId && c.BookStatus == BookStatus.Reserved)
               .OrderByDescending(c => c.ReserveDate).FirstOrDefaultAsync(cancellationToken);
                model.ReserveLastupdate = lastReserve.ReserveDate.ToFriendlyPersianDateTextify();
            }
            else
            {
                model.ReserveLastupdate = "";
            }
            //Borrow Count
            model.BorrowCount = await _reserveBookRepository.TableNoTracking
               .Where(c => c.UserId == userId && c.BookStatus == BookStatus.Borrowed).CountAsync();

            //Borrow Last Update DateTime
            if (model.BorrowCount != 0)
            {
                var lastBorrow = await _reserveBookRepository.TableNoTracking
               .Where(c => c.UserId == userId && c.BookStatus == BookStatus.Borrowed)
               .OrderByDescending(c => c.ReserveDate).FirstOrDefaultAsync(cancellationToken);
                model.BorrowLastUpdate = lastBorrow.BorrowDate.ToFriendlyPersianDateTextify();
            }
            else
            {
                model.BorrowLastUpdate = "";
            }
            var thisuser = await _userRepository.TableNoTracking.Include(c => c.Penalties)
                .Where(c => c.Id == userId).SingleOrDefaultAsync(cancellationToken);

            var reservePenalty = thisuser.Penalties.Where(c => c.PenaltyType == PenaltyType.Reserve).ToList();
            var borrowPenalty = thisuser.Penalties.Where(c => c.PenaltyType == PenaltyType.Return).ToList();

            if (reservePenalty.Count != 0)
                model.ReservePunishAmount = reservePenalty.Sum(c => c.Amount);
            else
                model.ReservePunishAmount = 0;

            if (borrowPenalty.Count != 0)
                model.BorrowPunishAmount = borrowPenalty.Sum(c => c.Amount);
            else
                model.BorrowPunishAmount = 0;

            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.FullName = user.FullName;
            return View(model);
        }

    }
}