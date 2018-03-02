using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;
using TomasGreen.Web.Areas.Import.ViewModels;
using TomasGreen.Web.Balances;
using TomasGreen.Web.Validations;
using TomasGreen.Web.Extensions;
using TomasGreen.Web.BaseModels;

namespace TomasGreen.Web.Areas.Import.Controllers
{
    [Area("Import")]
    public class ArrivedArticlesController :BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ArrivedArticlesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        // GET: Sales/Orders
        public IActionResult Index()
        {
            List<OnthewayArticlesViewModel> arrivedArticlesViewModelList = new List<OnthewayArticlesViewModel>();
            

            var articlesOntheway = OnthewayArticlesBalance.GetOnthewayArticles(_context);
            foreach (var articleontheway in articlesOntheway)
            {
                var onthewayArticleViewModel = new OnthewayArticlesViewModel();
                onthewayArticleViewModel.PurchasedArticleWarehouse = articleontheway;
                onthewayArticleViewModel.PurchasedDate = articleontheway.PurchasedArticle.Date;
                onthewayArticleViewModel.ExpectedToArrive = articleontheway.PurchasedArticle.ExpectedToArrive;
                onthewayArticleViewModel.Company = articleontheway.PurchasedArticle.Company ?? new Company();
                onthewayArticleViewModel.Article = articleontheway.PurchasedArticle.Article ?? new Article();
                onthewayArticleViewModel.ContainerNumber = articleontheway.PurchasedArticle.ContainerNumber;
                arrivedArticlesViewModelList.Add(onthewayArticleViewModel);
            }
            return View(arrivedArticlesViewModelList);
        }
    }
}
