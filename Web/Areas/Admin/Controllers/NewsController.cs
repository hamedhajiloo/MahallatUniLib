using Common;
using Data.Repositories;
using DNTPersianUtils.Core;
using Entities.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Services.Services.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Areas.Admin.Controllers
{
    [Route("[area]/[controller]/[action]")]
    [Area("Admin")]
    [Authorize(Roles = "Admin,Personel")]
    [Controller]
    //[ShowErrorPageType]
    public class NewsController : Controller
    {
        private readonly IRepository<News> _repository;
        private readonly IImageService _imageService;

        public NewsController(IRepository<News> repository, IImageService imageService)
        {
            _repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
            _imageService = imageService ?? throw new System.ArgumentNullException(nameof(imageService));
        }
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            ViewBag.ViewTitle = "اخبار";
            List<News> models = await _repository.TableNoTracking.Where(c => c.Deleted == false).OrderByDescending(c => c.InsertDate).ToListAsync(cancellationToken);
            foreach (var item in models)
            {
                item.InserDateP = item.InsertDate.ToFriendlyPersianDateTextify();
            }
            return View(models);
        }

        [HttpGet]
        public ActionResult Create(CancellationToken cancellationToken)
        {
            ViewBag.ViewTitle = "فرم افزودن کتاب";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromHeader]CancellationToken cancellationToken, News news, string imagename)
        {
            if (ModelState.IsValid)
            {
                if (imagename == null)
                {
                    imagename = "defaultbookimage.png";
                }

                news.Picture = imagename;
                news.ThumbNail = imagename;
                news.InsertDate = DateTime.Now;
                await _repository.AddAsync(news, cancellationToken);
                TempData["Success"] = "خبر جدید با موفقیت ثبت شد";
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        public async Task<IActionResult> UploadImage(IEnumerable<IFormFile> files)
        {
            string filename = await _imageService.UploadFile(files, @"upload\image\news\");

            return Json(new { status = "success", message = "تصویر با موفقیت آپلود شد.", imagename = filename });
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id, [FromHeader] CancellationToken cancellationToken)
        {
            News news = await _repository.Table.Where(c => c.Id == id).SingleOrDefaultAsync(cancellationToken);
            if (news == null)
            {
                TempData["Error"] = "خبر مورد نظر یافت نشد";
            }
            else
            {
                news.Deleted = true;

                await _repository.UpdateAsync(news, cancellationToken);
                TempData["Success"] = "خبر مورد نظر یافت نشد";
            }
            return RedirectToAction(actionName: "Index", controllerName: "News", new { area = "Admin" });

        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id, [FromHeader]CancellationToken cancellationToken)
        {
            ViewBag.ViewTitle = "فرم ویرایش خبر";
            News model = await _repository.TableNoTracking.Where(c => c.Id == id).SingleOrDefaultAsync(cancellationToken);
            if (model == null)
            {
                TempData["Error"] = "این خبر در سیستم وجود ندارد";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromHeader]CancellationToken cancellationToken, News news, string imagename)
        {
            foreach (ModelStateEntry modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    TempData["Error"] = error.ErrorMessage;
                }
            }
            if (ModelState.IsValid)
            {
                if (imagename != null)
                {
                    news.Picture = imagename;
                    news.ThumbNail = imagename;
                }


                await _repository.UpdateAsync(news, cancellationToken);

                TempData["Success"] = "تبریک . ویرایش با موفقیت انجام شد";
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }




        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id, [FromHeader]CancellationToken cancellationToken)
        {
            News model = await _repository.TableNoTracking.Where(c => c.Id == id).SingleOrDefaultAsync(cancellationToken);
            model.InserDateP = model.InsertDate.ToFriendlyPersianDateTextify();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Search(CancellationToken cancellationToken, Pagable pagable)
        {
            Expression<Func<News, bool>> ConditionalExpression = p => p.Deleted == false;
            if (!string.IsNullOrEmpty(pagable.Search))
            {
                ConditionalExpression = p => (p.Title.Contains(pagable.Search) ||
                                            p.Message.Contains(pagable.Search)) && p.Deleted == false;
            }

            List<News> model = await _repository.TableNoTracking.Where(ConditionalExpression).OrderByDescending(c => c.InsertDate).ToListAsync(cancellationToken);
            foreach (var item in model)
            {
                item.InserDateP = item.InsertDate.ToFriendlyPersianDateTextify();
            }
            return Json(new { data = model });
        }
    }
}