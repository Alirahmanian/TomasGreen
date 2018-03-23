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
using Microsoft.Extensions.Localization;

namespace TomasGreen.Web.Areas.Import.Controllers
{
    [Area("Import")]
    public class ArrivedArticlesController :BaseController
    {

        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<ArrivedArticlesController> _localizer;
        public ArrivedArticlesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<ArrivedArticlesController> localizer)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
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
                onthewayArticleViewModel.Warehouse = articleontheway.Warehouse;
                onthewayArticleViewModel.Company = articleontheway.PurchasedArticle.Company ?? new Company();
                onthewayArticleViewModel.Article = articleontheway.PurchasedArticle.Article ?? new Article();
                onthewayArticleViewModel.ContainerNumber = articleontheway.PurchasedArticle.ContainerNumber;
                arrivedArticlesViewModelList.Add(onthewayArticleViewModel);
            }
            return View(arrivedArticlesViewModelList);
        }

        // GET: Stock/Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SavedPurchasedArticleWarehouses = await _context.PurchasedArticleWarehouses.Include(p => p.PurchasedArticle).ThenInclude(a => a.Article).SingleOrDefaultAsync(m => m.ID == id);
            if (SavedPurchasedArticleWarehouses == null)
            {
                return NotFound();
            }
            if(SavedPurchasedArticleWarehouses.ArrivedDate != null)
            {
                return RedirectToAction(nameof(Index));
            }
            var model = new ArrivedArticleViewModel
            {
                PurchasedArticleWarehouseID = SavedPurchasedArticleWarehouses.ID,
                ContainerNumber = SavedPurchasedArticleWarehouses.PurchasedArticle.ContainerNumber,
                Article = SavedPurchasedArticleWarehouses.PurchasedArticle.Article,
                ArrivedDate = DateTime.Today,
            };
            ViewBag.ArrivedAtWarehouseID = new SelectList(_context.Warehouses.Where(w => w.IsOnTheWay == false).OrderBy(w => w.Name), "ID", "Name", model?.Warehouse?.ID);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? PurchasedArticleWarehouseID, ArrivedArticleViewModel model)
        {
            if (PurchasedArticleWarehouseID != model.PurchasedArticleWarehouseID)
            {
                return NotFound();
            }

            var SavedPurchasedArticleWarehouse = _context.PurchasedArticleWarehouses.Where(p => p.ID == model.PurchasedArticleWarehouseID).FirstOrDefault();
            if(SavedPurchasedArticleWarehouse == null)
            {
                return NotFound();
            }
            var result = Validate(model);
            if (!result.Value)
            {
                ModelState.AddModelError("", _localizer["Couldn't save."]);
                if (_hostingEnvironment.IsDevelopment())
                {
                    ModelState.AddModelError("", JSonHelper.ToJSon(result));
                }
                return View(model);
            }
            SavedPurchasedArticleWarehouse.ArrivedAtWarehouseID = model.Warehouse.ID;
            SavedPurchasedArticleWarehouse.ArrivedDate = model.ArrivedDate;
            SavedPurchasedArticleWarehouse.QtyPackagesArrived = model.QtyPackagesArrived;
            SavedPurchasedArticleWarehouse.QtyExtraArrived = model.QtyExtraArrived;
            SavedPurchasedArticleWarehouse.Notes = model.Notes;
            _context.Update(SavedPurchasedArticleWarehouse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchasedArticleWarehouseExists(int id)
        {
            return _context.PurchasedArticleWarehouses.Any(p => p.ID == id);
        }
        #region
        private PropertyValidation Validate(ArrivedArticleViewModel model)
        {
            var result = new PropertyValidation {Value = true, Action="ArrivedArticles", Model="PurchasedArticleWarehouse", Property = "", Message = "" };
            if (model.QtyPackagesArrived  < 0 )
            {
                result.Value = false; result.Property = nameof(model.QtyPackagesArrived); result.Message = "Please put a valid number.";
                return result;
            }
            if (model.QtyExtraArrived < 0)
            {
                result.Value = false; result.Property = nameof(model.QtyExtraArrived); result.Message = "Please put a valid number.";
                return result;
            }
            if (model.QtyPackagesArrived == 0 && model.QtyExtraArrived == 0)
            {
                result.Value = false; result.Property = nameof(model.QtyPackagesArrived); result.Message = "Please put a value bigger than zero.";
                return result;
            }
            if (model.Warehouse.ID == 0)
            {
                result.Value = false; result.Property = nameof(model.QtyPackagesArrived); result.Message = "Please select a warehouse.";
                return result;
            }
            return result;
        }
        #endregion
    }
}
