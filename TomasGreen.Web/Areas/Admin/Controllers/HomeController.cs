using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TomasGreen.Web.Models;
using Microsoft.Extensions.Localization;
using TomasGreen.Web.BaseModels;


namespace TomasGreen.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class HomeController : BaseController
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        public HomeController(IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
        }
        public IActionResult Index()
        {
            ViewData["Message"] = _localizer["HelloWorld"];
            return View();
        }
    }
}