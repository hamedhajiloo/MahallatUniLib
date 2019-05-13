using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Setting/[action]")]
    public class SettingController : Controller
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var model = await _settingService.GetAsync(cancellationToken);
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(CancellationToken cancellationToken)
        {
            var model = await _settingService.GetAsync(cancellationToken);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Setting setting, [FromHeader]CancellationToken cancellationToken)
        {

            var res = await _settingService.EditAsync(cancellationToken, setting);
            if (res==true)
            {
                TempData["Success"] = "تبریک . ویرایش تنظیمات با موفقیت انجام شد";
            }
            else
            {
                TempData["Error"] = "ویرایش ناموفق بود";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}