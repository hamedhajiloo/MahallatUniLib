using Common;
using Common.Enums;
using Data.Repositories;
using DNTPersianUtils.Core;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Controllers
{
    [Authorize]
    [Route("Book/[action]")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IRepository<Field> _fieldRepository;
        private readonly IRepository<ReserveBook> _rbRepository;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Setting> _settingRepo;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Penalty> _penaltyRepo;
        private readonly IRepository<Book> _bookRepository;

        public BookController(IBookService bookService,
                              IRepository<Field> fieldRepository,
                              IRepository<ReserveBook> rbRepository,
                              IRepository<User> userRepo,
                              IRepository<Setting> settingRepo,
                              UserManager<User> userManager,
                              IRepository<Penalty> penaltyRepo,
                              IRepository<Book> bookRepository)
        {
            _bookService = bookService;
            _fieldRepository = fieldRepository;
            _rbRepository = rbRepository ?? throw new System.ArgumentNullException(nameof(rbRepository));
            this._userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            this._settingRepo = settingRepo ?? throw new ArgumentNullException(nameof(settingRepo));
            this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this._penaltyRepo = penaltyRepo ?? throw new ArgumentNullException(nameof(penaltyRepo));
            _bookRepository = bookRepository;
        }


        public async Task<ActionResult> Index(int id, CancellationToken cancellationToken,string search="")
        {
            //3eee51e6c2f6436983b8ded8138309ff

            //var list = new List<Book>();

            //for (int i = 0; i < 100; i++)
            //    list.Add(new Book { ImageUrl = "3eee51e6c2f6436983b8ded8138309ff.jpg", FieldId = 1, Name = "Test", Language = Common.Language.Persion,Publisher="Test Publisher",AuthorName="Test Author" });

            //await _bookRepository.AddRangeAsync(list, cancellationToken);
            TempData["Search"] = search;
            ViewBag.Id = id;

            var userId = User.Identity.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.FullName = user.FullName;
            Pagable pagable = new Pagable
            {
                Desc = true,
                Page = 1,
                PageSize = 9
            };

            List<Book> books = new List<Book>();
            if(id!=0)
                books = await _bookRepository.TableNoTracking.Where(c => c.FieldId == id && c.BookIsDeleted == false&&(c.Name.Contains(search)||c.AuthorName.Contains(search))).Take(9).ToListAsync(cancellationToken);
            else
                books= await _bookRepository.TableNoTracking.Where(c => c.BookIsDeleted == false && (c.Name.Contains(search) || c.AuthorName.Contains(search))).Take(9).ToListAsync(cancellationToken);

            return View(books);
        }


        [HttpGet]
        public async Task<ActionResult> PageIndex(int id, CancellationToken cancellationToken, int page, string search = "")
        {
            var userId = User.Identity.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.FullName = user.FullName;

            List<Book> books = new List<Book>();
            if (id != 0)
                books = await _bookRepository.TableNoTracking.Where(c => c.FieldId == id && c.BookIsDeleted == false && (c.Name.Contains(search) || c.AuthorName.Contains(search)))
                    .Skip((page - 1) * 9).Take(9).ToListAsync(cancellationToken);
            else
                books = await _bookRepository.TableNoTracking.Where(c => c.BookIsDeleted == false && (c.Name.Contains(search) || c.AuthorName.Contains(search)))
                    .Skip((page - 1) * 9).Take(9).ToListAsync(cancellationToken);

            return PartialView("_ItemsList", books);
        }


        //GetFree
        public async Task<ActionResult> GetFree(int id, CancellationToken cancellationToken)
        {
            var userId = User.Identity.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.FullName = user.FullName;
            ViewBag.Id = id;
            Pagable pagable = new Pagable
            {
                Desc = true,
                Page = 1,
                PageSize = 10
            };

            List<Book> books = new List<Book>();
            if (id != 0)
                books = await _bookRepository.TableNoTracking.Include(c=>c.ISBNs).Include(c=>c.ReserveBook).Where(c => c.FieldId == id && c.BookIsDeleted == false&&c.ISBNs.Count()>c.ReserveBook.Count()).Take(10).ToListAsync(cancellationToken);
            else
                books = await _bookRepository.TableNoTracking.Include(c => c.ISBNs).Include(c => c.ReserveBook).Where(c => c.BookIsDeleted == false && c.ISBNs.Count() > c.ReserveBook.Count()).Take(10).ToListAsync(cancellationToken);

            return View(viewName:nameof(Index),books);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult> Details(int id, [FromHeader]CancellationToken cancellationToken)
        {
            var userId = User.Identity.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.FullName = user.FullName;
            Services.Dto.BookSelectDto model = await _bookService.FindBookByIdAsync(id, cancellationToken);
            return View(model);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Reserve(int id, [FromHeader]CancellationToken cancellationToken)
        {
            var setting = await _settingRepo.GetByIdAsync(cancellationToken, 1);
            var userId = User.Identity.GetUserId();
            var user = await _userRepo.TableNoTracking.Include(c=>c.ReserveBooks).Where(c => c.Id == userId).SingleOrDefaultAsync(cancellationToken);
            ViewBag.FullName = user.FullName;
            Book book = await _bookRepository.TableNoTracking
                .Where(c => c.Id == id && c.BookIsDeleted == false).SingleOrDefaultAsync(cancellationToken);

            if (book == null)
                return new JsonResult("کتاب مورد نظر یافت نشد");

            ReserveBook rb = await _rbRepository.TableNoTracking.Where(c => c.UserId == userId && c.BookId == id).SingleOrDefaultAsync(cancellationToken);
            string message = "قبلا این کتاب را رزرو کرده اید";
            if (rb != null)
            {
                return new JsonResult(message);
            }

            if (user.ReserveBooks.Where(c=>c.BookStatus==BookStatus.Reserved).Count()>=setting.ReservCount)
            {
                return new JsonResult($"شما نمی توانید بیشتر از {setting.ReservCount} کتاب رزرو کنید");
            }

            ReserveBook newrb = new ReserveBook
            {
                BookId = id,
                BookStatus = Common.Enums.BookStatus.Reserved,
                ReserveDate = DateTime.Now,
                UserId = userId
            };

            await _rbRepository.AddAsync(newrb, cancellationToken);
            message = "با موفقیت رزرو شد";
            return new JsonResult(message);
        }

        //UnReserve
       
        [HttpGet("{id:int}")]
        public async Task<ActionResult> UnReserve(int id, [FromHeader]CancellationToken cancellationToken)
        {
            var userId = User.Identity.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.FullName = user.FullName;
            Book book = await _bookRepository.TableNoTracking
                .Where(c => c.Id == id && c.BookIsDeleted == false)
                .SingleOrDefaultAsync(cancellationToken);

            if (book==null)
                return new JsonResult("کتاب مورد نظر یافت نشد");

            ReserveBook rb = await _rbRepository.Table.Where(c => c.UserId == userId && c.BookId == id).SingleOrDefaultAsync(cancellationToken);


            string message = "این کتاب را تا به حال رزرو نکرده اید";
            if (rb == null)
                return new JsonResult(message);


            await _rbRepository.DeleteAsync(rb, cancellationToken);
            message = "با موفقیت حذف شد";
            return new JsonResult(message);
        }
        [HttpGet]
        public async Task<IActionResult> GetReserve(CancellationToken cancellationToken)
        {
            var userId = User.Identity.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.FullName = user.FullName;
            var books = await _rbRepository.TableNoTracking.Include(c=>c.Book)
                .Where(c => c.UserId == userId && c.BookStatus == BookStatus.Reserved).ToListAsync(cancellationToken);
            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> GetBorrow(CancellationToken cancellationToken)
        {
            var userId = User.Identity.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.FullName = user.FullName;
            var books = await _rbRepository.TableNoTracking.Include(c=>c.Book)
                .Where(c => c.UserId == userId && c.BookStatus == BookStatus.Borrowed).ToListAsync(cancellationToken);
            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> GetReservePenalty(CancellationToken cancellationToken)
        {
            var userId = User.Identity.GetUserId();
            var model = await _penaltyRepo.TableNoTracking.Include(c => c.Book).Include(c => c.User).Where(c=>c.PenaltyType==PenaltyType.Reserve&&c.UserId==userId).Select(c => new PenaltySelectModel
            {
                BookAuthor = c.Book.AuthorName,
                Amount=c.Amount,
                BookId=c.BookId,
                BookName=c.Book.Name,
                FullName=c.User.FullName,
                UserId=c.UserId,
                UserName=c.User.UserName,
                InsertDate=c.InsertDate
            }).ToListAsync(cancellationToken);


            foreach (var item in model)
                item.InsertDateP = item.InsertDate.ToFriendlyPersianDateTextify();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetBorrowPenalty(CancellationToken cancellationToken)
        {
            var userId = User.Identity.GetUserId();
            var model = await _penaltyRepo.TableNoTracking.Include(c => c.Book).Include(c => c.User).Where(c=>c.PenaltyType==PenaltyType.Return&&c.UserId==userId).Select(c => new PenaltySelectModel
            {
                BookAuthor = c.Book.AuthorName,
                Amount = c.Amount,
                BookId = c.BookId,
                BookName = c.Book.Name,
                FullName = c.User.FullName,
                UserId = c.UserId,
                UserName = c.User.UserName,
                InsertDate=c.InsertDate
            }).ToListAsync(cancellationToken);

           
            foreach (var item in model)
                item.InsertDateP = item.InsertDate.ToFriendlyPersianDateTextify();

            return View(model);
        }
    }
}