﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TomasGreen.Model.Models;
using TomasGreen.Web.BaseModels;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Areas.Packing.Controllers
{
    [Area("Packing")]
    public class HomeController : BaseController 
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<HomeController> _localizer;
        public HomeController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<HomeController> localizer)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}