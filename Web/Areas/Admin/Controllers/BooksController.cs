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
using Web.Model;

namespace Web.Areas.Admin.Controllers
{
    [Route("[area]/[controller]/[action]")]
    [Area("Admin")]
    [Authorize(Roles = "Admin,Personel")]
    [Controller]
    //[ShowErrorPageType]
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IRepository<Isbn> _isbnRepository;
        private readonly IImageService _imageService;
        private readonly IRepository<Setting> _sRepository;
        private readonly IRepository<Penalty> _penaltyRepo;
        private readonly IRepository<ReserveBook> _rbRepository;
        private readonly IRepository<Book> _repository;
        private readonly IRepository<Field> fRepository;
        private readonly IRepository<Book> _bRepository;

        public BooksController(IBookService bookService,
                               IRepository<Isbn> isbnRepository,
                               IImageService imageService,
                               IRepository<Setting> sRepository,
                               IRepository<Penalty> penaltyRepo,
                               IRepository<ReserveBook> rbRepository,
                               IRepository<Book> repository,
                               IRepository<Field> fRepository,
                               IRepository<Book> bRepository)
        {
            _bookService = bookService ?? throw new System.ArgumentNullException(nameof(bookService));
            this._isbnRepository = isbnRepository ?? throw new System.ArgumentNullException(nameof(isbnRepository));
            _imageService = imageService ?? throw new System.ArgumentNullException(nameof(imageService));
            this._sRepository = sRepository ?? throw new ArgumentNullException(nameof(sRepository));
            this._penaltyRepo = penaltyRepo ?? throw new ArgumentNullException(nameof(penaltyRepo));
            this._rbRepository = rbRepository ?? throw new ArgumentNullException(nameof(rbRepository));
            _repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
            this.fRepository = fRepository;
            _bRepository = bRepository ?? throw new System.ArgumentNullException(nameof(bRepository));
        }
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var models = await _bookService.GetAllBookAsync(cancellationToken);
            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> GetReserveBook(CancellationToken cancellationToken)
        {
            var models = await _rbRepository.TableNoTracking.Include(c => c.Book).ThenInclude(s=>s.ReserveBook).Include(c => c.Book).ThenInclude(s => s.ISBNs).Where(c => c.BookStatus == BookStatus.Reserved)
                .Select(c => new BookSelectDto
                {
                    AuthorName = c.Book.AuthorName,
                    BookStatus=c.BookStatus,
                    Id=c.BookId,
                    Edition=c.Book.Edition,
                    UserId=c.UserId,
                    Language=c.Book.Language.ToDisplay(DisplayProperty.Name),
                    Name=c.Book.Name,
                    ImageUrl= c.Book.ImageUrl,
                    Publisher= c.Book.Publisher,
                    PublishYear=c.Book.PublishYear,
                    BorrowCount=c.Book.ReserveBook.Where(a=>a.BookStatus==BookStatus.Reserved).Count(),
                    Isbns = c.Book.ISBNs
                }).ToListAsync(cancellationToken);
            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> GetBorrowBook(CancellationToken cancellationToken)
        {
            var models = await _rbRepository.TableNoTracking.Include(c=>c.User).Include(c => c.Book).ThenInclude(s => s.ReserveBook).Include(c=>c.Book).ThenInclude(s=>s.ISBNs).Where(c => c.BookStatus == BookStatus.Borrowed)
                .Select(c => new BookSelectDto
                {
                    AuthorName = c.Book.AuthorName,
                    BookStatus = c.BookStatus,
                    Id = c.BookId,
                    Edition = c.Book.Edition,
                    UserId = c.UserId,
                    Language = c.Book.Language.ToDisplay(DisplayProperty.Name),
                    Name = c.Book.Name,
                    ImageUrl = c.Book.ImageUrl,
                    Publisher = c.Book.Publisher,
                    PublishYear = c.Book.PublishYear,
                    BorrowCount = c.Book.ReserveBook.Where(a => a.BookStatus == BookStatus.Borrowed).Count(),
                    Isbns=c.Book.ISBNs,
                    UserName = c.User.UserName,
                    FullName = c.User.FullName,
                    BorrowDate=c.BorrowDate.ToFriendlyPersianDateTextify()
                }).ToListAsync(cancellationToken);
            return View(models);
        }

        //

        public async Task<IActionResult> SearchBorrowed(CancellationToken cancellationToken, Pagable pagable)
        {
            var model = new List<BookSelectDto>();

            if (string.IsNullOrEmpty(pagable.Search))
            {
                model = await _rbRepository.TableNoTracking.Include(c=>c.User).Include(c => c.Book).ThenInclude(s => s.ReserveBook).Include(c => c.Book).ThenInclude(s => s.ISBNs).Where(c => c.BookStatus == BookStatus.Borrowed)
                .Select(c => new BookSelectDto
                {
                    AuthorName = c.Book.AuthorName,
                    BookStatus = c.BookStatus,
                    Id = c.BookId,
                    Edition = c.Book.Edition,
                    UserId = c.UserId,
                    Language = c.Book.Language.ToDisplay(DisplayProperty.Name),
                    Name = c.Book.Name,
                    ImageUrl = c.Book.ImageUrl,
                    Publisher = c.Book.Publisher,
                    PublishYear = c.Book.PublishYear,
                    BorrowCount = c.Book.ReserveBook.Where(a => a.BookStatus == BookStatus.Borrowed).Count(),
                    Isbns = c.Book.ISBNs,
                    UserName = c.User.UserName,
                    FullName = c.User.FullName,
                    BorrowDate = c.BorrowDate.ToFriendlyPersianDateTextify()
                }).ToListAsync(cancellationToken);

            }
            else
            {
                model = await _rbRepository.TableNoTracking.Include(c=>c.User).Include(c => c.Book).ThenInclude(s => s.ReserveBook).Include(c => c.Book).ThenInclude(s => s.ISBNs).Where(c => c.BookStatus == BookStatus.Borrowed &&(c.Book.Name.Contains(pagable.Search)|| c.Book.AuthorName.Contains(pagable.Search)|| c.User.UserName.Contains(pagable.Search)||c.User.FullName.Contains(pagable.Search)))
                .Select(c => new BookSelectDto
                {
                    AuthorName = c.Book.AuthorName,
                    BookStatus = c.BookStatus,
                    Id = c.BookId,
                    Edition = c.Book.Edition,
                    UserId = c.UserId,
                    Language = c.Book.Language.ToDisplay(DisplayProperty.Name),
                    Name = c.Book.Name,
                    ImageUrl = c.Book.ImageUrl,
                    Publisher = c.Book.Publisher,
                    PublishYear = c.Book.PublishYear,
                    BorrowCount = c.Book.ReserveBook.Where(a => a.BookStatus == BookStatus.Borrowed).Count(),
                    Isbns = c.Book.ISBNs,
                    UserName=c.User.UserName,
                    FullName=c.User.FullName
                }).ToListAsync(cancellationToken);

            }


            return this.Json(new { data = model });
        }

        [HttpGet]
        public ActionResult Create(CancellationToken cancellationToken)
        {
            //var filedList = await fRepository.TableNoTracking.ToListAsync(cancellationToken);
            //var model = new BookDto { Fields = filedList };
            ViewBag.ViewTitle = "فرم افزودن کتاب";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromHeader]CancellationToken cancellationToken, BookDto bookDto, string imagename)
        {
            if (ModelState.IsValid)
            {
                if (imagename == null)
                    imagename = "defaultbookimage.png";
                bookDto.ImageUrl = imagename;
                var exists = await _bookService.BookExists(bookDto, cancellationToken);
                if (exists == true)
                {
                    TempData["Error"] = "این کتاب قبلا ثبت شده است";
                    return View(bookDto);
                }
                for (int i = 0; i < bookDto.BooksISBN.Count; i++)
                {
                    for (int j = i + 1; j < bookDto.BooksISBN.Count; j++)
                    {
                        if (bookDto.BooksISBN[i] == bookDto.BooksISBN[j])
                        {
                            TempData["Error"] = "شابک های وارد شده تکراری است";
                            return View(bookDto);
                        }
                    }
                }

                var res = await _bookService.AddBookAsync(bookDto, cancellationToken);
                TempData["Success"] = "تبریک . کتاب با موفقیت ثبت شد";
                return RedirectToAction(nameof(Index));
            }
            return View(bookDto);
        }

        public async Task<IActionResult> UploadImage(IEnumerable<IFormFile> files)
        {
            var filename = await _imageService.UploadFile(files, @"upload\image\book\");

            return Json(new { status = "success", message = "تصویر با موفقیت آپلود شد.", imagename = filename });
        }


        [HttpGet("{gbookid:int}")]
        public async Task<IActionResult> Delete(int gbookid, [FromHeader] CancellationToken cancellationToken)
        {
            var book = await _bookService.FindBookByIdAsync(gbookid, cancellationToken);
            if (book == null)
            {
                TempData["Error"] = "کتاب مورد نظر یافت نشد";
                //return RedirectToAction(actionName: "Index", controllerName: "Home", new { area = "Admin" });
            }
            return View(book);
        }


        [HttpGet]
        public async Task<JsonResult> DeleteOne(int isbnId, [FromHeader] CancellationToken cancellationToken)
        {
            var isbn = await _isbnRepository.Table.Where(c => c.Id == isbnId).SingleOrDefaultAsync(cancellationToken);
            if (isbn == null)
            {
                TempData["Error"] = "کتاب مورد نظر یافت نشد";
                //return RedirectToAction(actionName: "Index", controllerName: "Home", new { area = "Admin" });
            }

            isbn.IsDeleted = true;
            await _isbnRepository.UpdateAsync(isbn, cancellationToken);

            var result = await _isbnRepository.TableNoTracking.Where(c => c.BookId == isbn.BookId && c.IsDeleted == false).ToListAsync(cancellationToken);

            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, [FromHeader]CancellationToken cancellationToken)
        {
            ViewBag.ViewTitle = "فرم ویرایش کتاب";
            var model = await _bookService.FindBookById4EditAsync(id, cancellationToken);
            if (model == null)
            {
                TempData["Error"] = "این کتاب در سیستم وجود ندارد";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromHeader]CancellationToken cancellationToken, BookDto bookDto, string imagename)
        {
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    TempData["Error"] = error.ErrorMessage;
                }
            }
            if (ModelState.IsValid)
            {
                if (imagename != null)
                    bookDto.ImageUrl = imagename;
                var exists = await _bookService.EditAsync(bookDto, cancellationToken);

                if (exists != true)
                {
                    TempData["Error"] = "این کتاب در سیستم وجود ندارد";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Success"] = "تبریک . ویرایش با موفقیت انجام شد";
                return RedirectToAction(nameof(Index));
            }
            return View(bookDto);
        }


        [HttpGet("id:int")]
        public async Task<IActionResult> DeleteOneBook([FromHeader]CancellationToken cancellationToken, int id)
        {
            var book = await _bRepository.Table.Where(c => c.Id == id).SingleOrDefaultAsync(cancellationToken);
            if (book == null)
            {
                TempData["Error"] = "این کتاب در سیستم وجود ندارد";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                book.BookIsDeleted = true;
                await _bRepository.UpdateAsync(book, cancellationToken);


                TempData["Success"] = "کتاب با موفقیت حذف شد";
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception)
            {
                TempData["Error"] = "متاسفانه مشکلی پیش آمده است";
                return RedirectToAction(nameof(Index));
                throw;
            }



        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id, [FromHeader]CancellationToken cancellationToken)
        {
            var model = await _bookService.FindBookByIdAsync(id, cancellationToken);
            return View(model);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> AddOneBook(int id, [FromHeader]CancellationToken cancellationToken)
        {
            var book = await _repository.TableNoTracking.Where(c => c.Id == id).SingleOrDefaultAsync(cancellationToken);
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> AddOneBook(AddOneBookDto dto, [FromHeader]CancellationToken cancellationToken)
        {
            var exists = await _isbnRepository.TableNoTracking.Where(c => c.Value.Trim() == dto.ISBN.Trim()).FirstOrDefaultAsync(cancellationToken);
            var book = await _repository.TableNoTracking.Where(c => c.Id == dto.BookId).SingleOrDefaultAsync(cancellationToken);

            if (exists != null)
            {
                TempData["Error"] = "شابک  وارد شده تکراری است";
                return View(book);
            }
            var _isbn = new Isbn
            {
                BookId = dto.BookId,
                Value = dto.ISBN
            };
            await _isbnRepository.AddAsync(_isbn, cancellationToken);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Search(CancellationToken cancellationToken, Common.Language lan, int field, BookStatus bstatus, Pagable pagable)
        {
            var model = await _bookService.GetAllBookAsync(cancellationToken, bstatus, lan, field, pagable.Search);

            return this.Json(new { data = model });
        }

        public async Task<IActionResult> Search4Borrow(CancellationToken cancellationToken, Pagable pagable)
        {
            var model = await _bookService.GetAllBookAsync(pagable, cancellationToken);

            return this.Json(new { data = model });
        }

        public async Task<IActionResult> GetBooks4Add2Borrow(string userid, [FromHeader]CancellationToken cancellationToken)
        {
            var dtNow = DateTime.Now;
            var setting = await _sRepository.GetByIdAsync(cancellationToken, 1);
            ViewBag.UserId = userid;
            //var result = await _bRepository.TableNoTracking.Include(a => a.ReserveBook).Where(c => c.ReserveBook.Count > 0).ToListAsync(cancellationToken);


            var model = await _bRepository.TableNoTracking.Include(c => c.ISBNs).Include(c => c.ReserveBook)
                .Where(c => (c.ISBNs.Where(a => a.IsDeleted == false).Count() > c.ReserveBook.Where(a => a.BookStatus == BookStatus.Borrowed).Count()) &&((c.ReserveBook
                .Where(a => a.UserId == userid &&
                (a.BookStatus == BookStatus.Reserved && (((DateTime)a.ReserveDate).AddDays(setting.BDay4Reserve) <= dtNow)))
                .Count() > 0)

                ||

                (c.ISBNs.Where(a => a.IsDeleted == false).Count() > c.ReserveBook.Count
                && c.ReserveBook.Where(a => a.UserId == userid).Count() == 0))
                ).ToListAsync(cancellationToken);
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Borrow(string userid, int bookid, [FromHeader]CancellationToken cancellationToken)
        {
            var rb = await _rbRepository.Table
                .Where(c => c.UserId == userid && c.BookId == bookid).SingleOrDefaultAsync(cancellationToken);

            if (rb == null)
            {
                rb = new ReserveBook
                {
                    BookId = bookid,
                    BookStatus = BookStatus.Borrowed,
                    BorrowDate = DateTime.Now,
                    UserId = userid
                };
                await _rbRepository.AddAsync(rb, cancellationToken);
            }

            else
            {
                rb.BookStatus = BookStatus.Borrowed;
                rb.BorrowDate = DateTime.Now;

                await _rbRepository.UpdateAsync(rb, cancellationToken);

            }
            TempData["Success"] = "عملیات با موفقیت انجام شد";
            return RedirectToAction(nameof(GetBooks4Add2Borrow));
        }

        //UnBorrow
        public async Task<IActionResult> UnBorrow(int bookid,string userid, [FromHeader]CancellationToken cancellationToken)
        {
            var rb = await _rbRepository.Table
                .Where(c => c.BookId == bookid &&c.UserId==userid&& c.BookStatus == BookStatus.Borrowed).SingleOrDefaultAsync(cancellationToken);

            await _rbRepository.DeleteAsync(rb, cancellationToken);
            TempData["Success"] = "عملیات با موفقیت انجام شد";
            return RedirectToAction(nameof(GetBorrowBook));
        }

        [HttpGet]
        public async Task<IActionResult> GetReservePenalty(CancellationToken cancellationToken)
        {
            var userId = User.Identity.GetUserId();
            var model = await _penaltyRepo.TableNoTracking.Include(c => c.Book).Include(c => c.User).Where(c => c.PenaltyType == PenaltyType.Reserve).Select(c => new PenaltySelectModel
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

            return View(model.OrderByDescending(c=>c.InsertDate).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> GetBorrowPenalty(CancellationToken cancellationToken)
        {
            var userId = User.Identity.GetUserId();
            var model = await _penaltyRepo.TableNoTracking.Include(c => c.Book).Include(c => c.User).Where(c => c.PenaltyType == PenaltyType.Return).Select(c => new PenaltySelectModel
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

            return View(model.OrderByDescending(c=>c.InsertDate).ToList());
        }


        [HttpPost]
        public async Task<IActionResult> PayBorrowPenalty(string userid,int bookid,CancellationToken cancellationToken)
        {
            var penalty = await _penaltyRepo.Table
                .Where(c => c.UserId == userid && c.BookId == bookid && c.PenaltyType == PenaltyType.Return).SingleOrDefaultAsync(cancellationToken);

            await _penaltyRepo.DeleteAsync(penalty, cancellationToken);

            TempData["Success"] = "عملیات با موفقیت انجام شد";
            return RedirectToAction(nameof(GetBorrowPenalty));
        }

        [HttpPost]
        public async Task<IActionResult> PayReservePenalty(string userid, int bookid, CancellationToken cancellationToken)
        {
            var penalty = await _penaltyRepo.Table
              .Where(c => c.UserId == userid && c.BookId == bookid && c.PenaltyType == PenaltyType.Reserve).SingleOrDefaultAsync(cancellationToken);

            await _penaltyRepo.DeleteAsync(penalty, cancellationToken);

            TempData["Success"] = "عملیات با موفقیت انجام شد";
            return RedirectToAction(nameof(GetReservePenalty));
        }
    }
    public class AddOneBookDto
    {
        public int BookId { get; set; }
        public string ISBN { get; set; }

    }
}