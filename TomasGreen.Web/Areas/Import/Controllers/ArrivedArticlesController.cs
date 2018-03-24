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
            

            var articlesOntheway = GetOnthewayArticles(_context);
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
            SavedPurchasedArticleWarehouse.Warehouse = _context.Warehouses.Where(w => w.ID == SavedPurchasedArticleWarehouse.WarehouseID).FirstOrDefault();
            var purchasedArticle = _context.PurchasedArticles.Where(p => p.ID == SavedPurchasedArticleWarehouse.PurchasedArticleID).FirstOrDefault();
            if(purchasedArticle != null)
            {
                purchasedArticle.HasIssue = SavedPurchasedArticleWarehouse.HasIssue();
                purchasedArticle.Arrived = true;
                _context.Update(purchasedArticle);
            }
            var articleInOutForReduce = new ArticleInOut
            {
                ArticleID = SavedPurchasedArticleWarehouse.PurchasedArticle.ArticleID,
                WarehouseID = SavedPurchasedArticleWarehouse.WarehouseID,
                CompanyID = ArticleBalance.GetWarehouseCompany(_context, SavedPurchasedArticleWarehouse.Warehouse),
                QtyPackagesOut = SavedPurchasedArticleWarehouse.QtyPackages,
                QtyExtraOut = SavedPurchasedArticleWarehouse.QtyExtra
            };
            var reducedResult = ArticleBalance.Reduce(_context, articleInOutForReduce);
            if (result.Value == false)
            {
                ModelState.AddModelError("", "Couldn't save.");
                if (_hostingEnvironment.IsDevelopment())
                {
                    ModelState.AddModelError("", JSonHelper.ToJSon(result));
                }
                return View(model);

            }
            var arrivedWarehouse = _context.Warehouses.Where(w => w.ID == SavedPurchasedArticleWarehouse.ArrivedAtWarehouseID).FirstOrDefault();
            var articleInOut = new ArticleInOut
            {
                ArticleID = SavedPurchasedArticleWarehouse.PurchasedArticle.ArticleID,
                WarehouseID = (int)SavedPurchasedArticleWarehouse.ArrivedAtWarehouseID,
                CompanyID = ArticleBalance.GetWarehouseCompany(_context, arrivedWarehouse),
                QtyPackagesIn = SavedPurchasedArticleWarehouse.QtyPackagesArrived,
                QtyExtraIn = SavedPurchasedArticleWarehouse.QtyExtraArrived
            };
            var addedResult = ArticleBalance.Add(_context, articleInOut);
            if (result.Value == false)
            {
                ModelState.AddModelError("", "Couldn't save.");
                if (_hostingEnvironment.IsDevelopment())
                {
                    ModelState.AddModelError("", JSonHelper.ToJSon(result));
                }
                return View(model);

            }
            //Shift order's Warehouse 
            var shiftWarehouseResult = ShiftOrdersWarehouse(SavedPurchasedArticleWarehouse);
            _context.Update(SavedPurchasedArticleWarehouse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchasedArticleWarehouseExists(int id)
        {
            return _context.PurchasedArticleWarehouses.Any(p => p.ID == id);
        }



        #region
        private PropertyValidation ShiftOrdersWarehouse(PurchasedArticleWarehouse purchasedArticleWarehouse)
        {
            var result = new PropertyValidation { Value = true, Action = "ArrivedArticles|ShiftOrdersWarehouse", Model = "PurchasedArticleWarehouse", Property = "", Message = "" };
            try
            {
                var purchasedArticle = _context.PurchasedArticles.Where(p => p.ID == purchasedArticleWarehouse.PurchasedArticleID).FirstOrDefault();
                var orderDetails = _context.OrderDetails.Include(d => d.Order).Where(o => o.Order.LoadedDate == null)
                    .Where(o => o.Order.OrderDate >= purchasedArticle.Date && o.Order.OrderDate <= purchasedArticleWarehouse.AddedDate)
                    .Where(d => d.ArticleID == purchasedArticle.ArticleID && d.WarehouseID == purchasedArticleWarehouse.WarehouseID);
                var totalQtyPackages = 0;
                decimal totalQtyExtra = 0;
                foreach (var item in orderDetails)
                {
                    totalQtyPackages += item.QtyPackages;
                    totalQtyExtra += item.QtyExtra;
                }
                if(totalQtyPackages > purchasedArticleWarehouse.QtyPackagesArrived || totalQtyExtra > purchasedArticleWarehouse.QtyExtraArrived)
                {
                    result.Value = false; result.Property = "QtyPackagesArrived"; result.Message = "Arrived article is less than Orderd.";
                    return result;
                }
                foreach (var item in orderDetails)
                {
                    item.WarehouseID = (int)purchasedArticleWarehouse.ArrivedAtWarehouseID;
                    _context.Update(item);
                }

            }
            catch (Exception exception)
            {
                result.Value = false; result.Property = "exception"; result.Message = exception.Message.ToString();
            }
            return result;
        }

        private List<PurchasedArticleWarehouse> GetOnthewayArticles(ApplicationDbContext _context)
        {
            var list = _context.PurchasedArticleWarehouses.Where(a => a.ArrivedDate == null && a.Warehouse.IsOnTheWay == true).Include(p => p.PurchasedArticle).ThenInclude(pa => pa.Article).Include(w => w.Warehouse).OrderBy(a => a.PurchasedArticle.Date).ThenBy(a => a.PurchasedArticle.Article).ToList();
            return list;

        }
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
