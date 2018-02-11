using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TomasGreen.Web.Areas.Accountment.Controllers
{
    [Area("Accountment")]
    public class HomeController :Controller
    {
        public IActionResult Index() => View();
       
    }
}
