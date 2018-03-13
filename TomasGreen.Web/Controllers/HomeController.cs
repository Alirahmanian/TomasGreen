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
            _localizer = localizer;
            _loggerFactory = loggerFactory;
        }
        public IActionResult Index()
        {
            try
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                var logger = _loggerFactory.CreateLogger("LoggerCategory");
                logger.LogInformation(".:. " + actionName);
                logger.LogInformation(".:. " + controllerName);

                ViewData["Message"] = _localizer["HelloWorld"];
            }
            catch(Exception err)
            {
                var logger = _loggerFactory.CreateLogger("LoggerCategory");
                logger.LogError(err.GetType().FullName);
               
            }
            finally
            {

            }

          
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
