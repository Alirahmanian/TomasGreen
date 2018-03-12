﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TomasGreen.Web.BaseModels;

namespace TomasGreen.Web.Areas.Export.Controllers
{
    [Area("Export")]
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}