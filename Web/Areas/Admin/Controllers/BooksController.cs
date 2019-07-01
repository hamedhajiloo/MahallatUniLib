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
    [ShowErrorPageType]
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IImageService _imageService;
        private readonly IRepository<BookList> _repository;
        private readonly IRepository<Book> _bRepository;

        public BooksController(IBookService bookService, IImageService imageService, IRepository<BookList> repository, IRepository<Book> bRepository)
        {
            _bookService = bookService;
            _imageService = imageService;
            _repository = repository;
            _bRepository = bRepository;
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

        [HttpPost]
        public async Task<IActionResult> Delete(int gbookid, [FromHeader] CancellationToken cancellationToken)
        {
            var res = await _bookService.DeleteAsync(gbookid, cancellationToken);
            if (res == true)
            {
                TempData["War"] = "کتاب مورد نظر حذف شد";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "کتاب مورد نظر یافت نشد";
            return RedirectToAction(nameof(Index));
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
            var exists = await _repository.Table.Where(c => c.Id == id).Include(c => c.Books).FirstOrDefaultAsync(cancellationToken);

            if (exists == null)
            {
                TempData["Error"] = "این کتاب در سیستم وجود ندارد";
                return RedirectToAction(nameof(Index));
            }
            var bookCount = exists.Books.Count;
            if (bookCount > 1)
            {
                var deletBook = await _bRepository.Table.FirstOrDefaultAsync(c => c.BookListId == id, cancellationToken);
                await _bRepository.DeleteAsync(deletBook, cancellationToken);
            }
            else
            {
                try
                {
                    var deletBook = await _bRepository.Table.FirstOrDefaultAsync(c => c.BookListId == id, cancellationToken);
                    await _bRepository.DeleteAsync(deletBook, cancellationToken);
                    await _bookService.DeleteAsync(exists.Id, cancellationToken);
                }
                catch (System.Exception)
                {

                    throw;
                }
            }
            TempData["Success"] = "کتاب با موفقیت حذف شد";
            return RedirectToAction(nameof(Index));

        }

        [HttpGet("id:int")]
        public async Task<IActionResult> Details(int id, [FromHeader]CancellationToken cancellationToken)
        {
            var model = await _bookService.FindBookByIdAsync(id, cancellationToken);
            return View(model);
        }

    
        public async Task<IActionResult> Search(CancellationToken cancellationToken, Language lan, CourseType ctype, int field, BookStatus bstatus, Pagable pagable)
        {
            var model = await _bookService.GetAllBookAsync(cancellationToken, ctype, bstatus, lan, field, pagable.Search);
            //return Ok(model);
            return View(nameof(Index),model);
        }
    }
}