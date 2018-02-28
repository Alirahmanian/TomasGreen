using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace TomasGreen.Web.Areas.Purchase.Controllers
{
    [Area("Purchase")]
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
