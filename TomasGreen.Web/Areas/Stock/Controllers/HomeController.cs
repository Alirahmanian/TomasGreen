using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace TomasGreen.Web.Areas.Stock.Controllers
{
    [Area("Stock")]
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
