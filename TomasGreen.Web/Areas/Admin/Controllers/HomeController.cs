﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TomasGreen.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController :Controller
    {
        public IActionResult Index() => View();
       
    }
}