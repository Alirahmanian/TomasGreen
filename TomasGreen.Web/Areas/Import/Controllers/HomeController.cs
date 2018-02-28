using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TomasGreen.Web.Areas.Import.Controllers
{
    public class HomeController : Controller
    {
        [Area("Import")]
        public IActionResult Index()
        {
            return View();
        }
    }
}