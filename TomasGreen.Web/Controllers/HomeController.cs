using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TomasGreen.Web.Models;
using Microsoft.Extensions.Localization;
using TomasGreen.Web.BaseModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using TomasGreen.Web.Data;
using System.Net.Http;
using Newtonsoft.Json;
using TomasGreen.Web.Services;
using TomasGreen.Model.Models;

namespace TomasGreen.Web.Controllers
{
    // [Authorize]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<HomeController> _localizer;
        private ILoggerFactory _loggerFactory;
        public HomeController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<HomeController> localizer, ILoggerFactory loggerFactory)
        {
            _context = context;
            _localizer = localizer;
            _loggerFactory = loggerFactory;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {

            return View();
        }
       
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
