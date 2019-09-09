using Common;
using Common.Enums;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Dto;
using Services.Services.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Filters;

namespace Web.Areas.Admin.Controllers
{
    [Route("[area]/[controller]/[action]")]
    [Area("Admin")]
    [Controller]
    //[ShowErrorPageType]
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IRepository<Isbn> _isbnRepository;
        private readonly IImageService _imageService;

        private readonly IRepository<Book> _repository;
        private readonly IRepository<Book> _bRepository;

        public BooksController(IBookService bookService,
                               IRepository<Isbn> isbnRepository,
                               IImageService imageService,
                               IRepository<Book> repository,
                               IRepository<Book> bRepository)
        {
            _bookService = bookService ?? throw new System.ArgumentNullException(nameof(bookService));
            this._isbnRepository = isbnRepository ?? throw new System.ArgumentNullException(nameof(isbnRepository));
            _imageService = imageService ?? throw new System.ArgumentNullException(nameof(imageService));
            _repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
            _bRepository = bRepository ?? throw new System.ArgumentNullException(nameof(bRepository));
        }
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var models = await _bookService.GetAllBookAsync(cancellationToken);
            return View(models);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ViewTitle = "فرم افزودن کتاب";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromHeader]CancellationToken cancellationToken, BookDto bookDto, string imagename)
        {
            if (ModelState.IsValid)
            {
                bookDto.ImageUrl = imagename;
                var exists = await _bookService.BookExists(bookDto, cancellationToken);
                if (exists == true)
                {
                    TempData["Error"] = "این کتاب قبلا ثبت شده است";
                    return View(bookDto);
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

            var result = await _isbnRepository.TableNoTracking.Where(c => c.BookId == isbn.BookId&&c.IsDeleted==false).ToListAsync(cancellationToken);

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


        [HttpGet("isbn:minlength(10)")]
        public async Task<IActionResult> DeleteOneBook([FromHeader]CancellationToken cancellationToken, string isbn)
        {
            var res = await _bookService.DeleteOneBookAsync(isbn, cancellationToken);

            if (res)
            {
                TempData["Success"] = "کتاب با موفقیت حذف شد";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "این کتاب در سیستم وجود ندارد";
            return RedirectToAction(nameof(Index));

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id, [FromHeader]CancellationToken cancellationToken)
        {
            var model = await _bookService.FindBookByIdAsync(id, cancellationToken);
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Search(CancellationToken cancellationToken, Language lan, CourseType ctype, int field, BookStatus bstatus, Pagable pagable)
        {
            var model = await _bookService.GetAllBookAsync(cancellationToken, ctype, bstatus, lan, field, pagable.Search);
            //ViewBag.Text = pagable.Search;
            //ViewBag.CType = ctype;
            //ViewBag.BStatus = bstatus;
            //ViewBag.Lan = lan;
            //ViewBag.Field = field;



            return this.Json(new { data = model });
            //return Ok(model);
            //return View(nameof(Index), model);
        }
    }
}