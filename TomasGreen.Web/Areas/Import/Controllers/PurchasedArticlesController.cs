﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TomasGreen.Model.Models;
using TomasGreen.Web.Areas.Import.ViewModels;
using TomasGreen.Web.Balances;
using TomasGreen.Web.Data;
using TomasGreen.Web.Validations;
using TomasGreen.Web.Extensions;
using Microsoft.Extensions.Localization;


namespace TomasGreen.Web.Areas.Import.Controllers
{
    [Area("Import")]
    public class PurchasedArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<PurchasedArticlesController> _localizer;
        public PurchasedArticlesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<PurchasedArticlesController> localizer)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
        }

        // GET: Stock/PurchasedArticles
        public IActionResult Index()
        {
            var purchasedArticles = GetIndexList();
            return View(purchasedArticles);
        }

        // GET: Stock/PurchasedArticles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchasedArticle = await _context.PurchasedArticles
                .Include(r => r.Article)
                .Include(r => r.Company)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (purchasedArticle == null)
            {
                return NotFound();
            }

            return View(purchasedArticle);
        }

        public IActionResult Create(int? id)
        {
            var model = new SavePurchasedArticleWarehouseViewModel();
            if(id > 0)
            {
                model.PurchasedArticle = _context.PurchasedArticles.Where(r => r.ID == id).FirstOrDefault();
                
            }
            else
            {
                model.PurchasedArticle = new PurchasedArticle();
                model.PurchasedArticle.Date = DateTime.Today;
            }
            
            
            model.PurchasedArticleWarehouses = GetPurchasedArticleWarehouse(id ?? 0);
            AddPurchasedArticleLists(new SavePurchasedArticleWarehouseViewModel());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SavePurchasedArticleWarehouseViewModel model)
        {
            //
            var skippedErrors = ModelState.Keys.Where(key => key.StartsWith("PurchasedArticle.ID") || key.StartsWith("PurchasedArticle.CompanyID"));
            foreach (var key in skippedErrors)
            {
                ModelState.Remove(key);
            }
            if(!ModelState.IsValid)
            {
                var customModelValidator = PurchasedArticleValidation.PurchasedArticleIsValid(_context, model.PurchasedArticle);
                if (customModelValidator.Value == false)
                {
                    ModelState.AddModelError(customModelValidator.Property, customModelValidator.Message);
                    model.PurchasedArticleWarehouses = GetPurchasedArticleWarehouse((int)model.PurchasedArticle?.ID);
                    AddPurchasedArticleLists(model);
                    return View(model);
                }
            }

            if (!ModelState.IsValid)
            {
                model.PurchasedArticleWarehouses = GetPurchasedArticleWarehouse((int)model.PurchasedArticle?.ID);
                AddPurchasedArticleLists(model);
                return View(model);
            }
            if(model.PurchasedArticle.ID == 0)
            {
                var guid = Guid.NewGuid();
                model.PurchasedArticle.Guid = guid;
                model.PurchasedArticle.Discount = 0;
                model.PurchasedArticle.Received = false;
                model.PurchasedArticle.TotalPrice = 0;
                model.PurchasedArticle.TotalPerUnit = 0;
                _context.Add(model.PurchasedArticle);
                await _context.SaveChangesAsync();
                var savedPurchasedArticle = _context.PurchasedArticles.Where(r => r.Date == model.PurchasedArticle.Date && r.ArticleID == model.PurchasedArticle.ArticleID && r.Guid == guid).FirstOrDefault();
                if(savedPurchasedArticle == null)
                {
                    ModelState.AddModelError("", "Couldn't save.");
                    model.PurchasedArticleWarehouses = GetPurchasedArticleWarehouse(0);
                    AddPurchasedArticleLists(model);
                    return View(model);
                }
                var onThewayWarehouse = _context.Warehouses.Where(w => w.IsOnTheWay).FirstOrDefault();
                if (onThewayWarehouse == null)
                {
                    ModelState.AddModelError("", "Couldn't save.");
                    model.PurchasedArticleWarehouses = GetPurchasedArticleWarehouse(0);
                    AddPurchasedArticleLists(model);
                    return View(model);
                }
                var tempPurchasedArticleWarehouse = new PurchasedArticleWarehouse();
                tempPurchasedArticleWarehouse.WarehouseID = onThewayWarehouse.ID;
                tempPurchasedArticleWarehouse.Warehouse = onThewayWarehouse;
                tempPurchasedArticleWarehouse.PurchasedArticleID = savedPurchasedArticle.ID;
                tempPurchasedArticleWarehouse.PurchasedArticle = savedPurchasedArticle;
                    foreach (var purchasedArticleWarehouse in model.PurchasedArticleWarehouses)
                    {
                        if (purchasedArticleWarehouse.QtyPackages > 0 || purchasedArticleWarehouse.QtyExtra > 0)
                        {
                            purchasedArticleWarehouse.PurchasedArticleID = savedPurchasedArticle.ID;
                            _context.Add(purchasedArticleWarehouse);
                            tempPurchasedArticleWarehouse.QtyPackages += purchasedArticleWarehouse.QtyPackages;
                            tempPurchasedArticleWarehouse.QtyExtra += purchasedArticleWarehouse.QtyExtra;
                        }
                    }
                var articleInOut = new ArticleInOut
                {
                    ArticleID = tempPurchasedArticleWarehouse.PurchasedArticle.ArticleID,
                    WarehouseID = tempPurchasedArticleWarehouse.WarehouseID,
                    CompanyID = ArticleBalance.GetWarehouseCompany(_context, tempPurchasedArticleWarehouse.Warehouse),
                    QtyPackages = tempPurchasedArticleWarehouse.QtyPackages,
                    QtyExtra = tempPurchasedArticleWarehouse.QtyExtra
                };
                var result = ArticleBalance.Add(_context, articleInOut);
               // var result = articleBalance.AddPurchasedArticleToBalance(tempPurchasedArticleWarehouse);
                if (result.Value == false)
                {
                    ModelState.AddModelError("", "Couldn't save.");
                    if (_hostingEnvironment.IsDevelopment())
                    {
                        ModelState.AddModelError("", JSonHelper.ToJSon(result));
                    }
                    _context.Remove(savedPurchasedArticle);
                    await _context.SaveChangesAsync();
                    model.PurchasedArticleWarehouses = GetPurchasedArticleWarehouse(0);
                    AddPurchasedArticleLists(model);
                    return View(model);

                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var savedPurchasedArticle = _context.PurchasedArticles.Where(r => r.ID == model.PurchasedArticle.ID).Include(w => w.Warehouses).FirstOrDefault();
                if (savedPurchasedArticle != null)
                {
                    //rollback old values from article balance
                 
                    var onThewayWarehouse = _context.Warehouses.Where(w => w.IsOnTheWay).FirstOrDefault();
                    var oldPurchasedArticleWarehouse = new PurchasedArticleWarehouse();
                    oldPurchasedArticleWarehouse.WarehouseID = onThewayWarehouse.ID;
                    oldPurchasedArticleWarehouse.Warehouse = onThewayWarehouse;
                    oldPurchasedArticleWarehouse.PurchasedArticleID = savedPurchasedArticle.ID;
                    oldPurchasedArticleWarehouse.PurchasedArticle = savedPurchasedArticle;
                    foreach (var purchasedArticleWarehouse in savedPurchasedArticle.Warehouses)
                    {
                        if (purchasedArticleWarehouse.QtyPackages > 0 || purchasedArticleWarehouse.QtyExtra > 0)
                        {
                            oldPurchasedArticleWarehouse.QtyPackages += purchasedArticleWarehouse.QtyPackages;
                            oldPurchasedArticleWarehouse.QtyExtra += purchasedArticleWarehouse.QtyExtra;
                            _context.Remove(purchasedArticleWarehouse);
                        }
                    }
                    var changedPurchasedArticleWarehouse = new PurchasedArticleWarehouse();
                    changedPurchasedArticleWarehouse.WarehouseID = onThewayWarehouse.ID;
                    changedPurchasedArticleWarehouse.Warehouse = onThewayWarehouse;
                    changedPurchasedArticleWarehouse.PurchasedArticleID = savedPurchasedArticle.ID;
                    changedPurchasedArticleWarehouse.PurchasedArticle = savedPurchasedArticle;
                    foreach (var purchasedArticleWarehouse in savedPurchasedArticle.Warehouses)
                    {
                        if (purchasedArticleWarehouse.QtyPackages > 0 || purchasedArticleWarehouse.QtyExtra > 0)
                        {
                            changedPurchasedArticleWarehouse.QtyPackages += purchasedArticleWarehouse.QtyPackages;
                            changedPurchasedArticleWarehouse.QtyExtra += purchasedArticleWarehouse.QtyExtra;
                            _context.Remove(purchasedArticleWarehouse);
                        }
                    }
                    var articleInOut = new ArticleInOut
                    {
                        ArticleID = changedPurchasedArticleWarehouse.PurchasedArticle.ArticleID,
                        WarehouseID = changedPurchasedArticleWarehouse.WarehouseID,
                        CompanyID = ArticleBalance.GetWarehouseCompany(_context, changedPurchasedArticleWarehouse.Warehouse),
                        QtyPackages = (changedPurchasedArticleWarehouse.QtyPackages > oldPurchasedArticleWarehouse.QtyPackages)? (changedPurchasedArticleWarehouse.QtyPackages - oldPurchasedArticleWarehouse.QtyPackages)
                        : ((oldPurchasedArticleWarehouse.QtyPackages - changedPurchasedArticleWarehouse.QtyPackages) * -1),
                        QtyExtra = (changedPurchasedArticleWarehouse.QtyExtra > oldPurchasedArticleWarehouse.QtyExtra) ? (changedPurchasedArticleWarehouse.QtyExtra - oldPurchasedArticleWarehouse.QtyExtra)
                        : ((oldPurchasedArticleWarehouse.QtyExtra - changedPurchasedArticleWarehouse.QtyExtra) * -1)
                    };
                    var result = ArticleBalance.Add(_context, articleInOut);
                    if (result.Value == false)
                    {
                        ModelState.AddModelError("", "Couldn't save.");
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", JSonHelper.ToJSon(result));
                        }
                        model.PurchasedArticleWarehouses = GetPurchasedArticleWarehouse(savedPurchasedArticle?.ArticleID ?? 0);
                        AddPurchasedArticleLists(model);
                        return View(model);

                    }

                    if (savedPurchasedArticle.CompanyID != model.PurchasedArticle.CompanyID)
                    {
                        // rollBack Company balance
                    }
                    tempPurchasedArticleWarehouse.QtyPackages = 0;
                    tempPurchasedArticleWarehouse.QtyExtra = 0;
                    foreach (var purchasedArticleWarehouse in model.PurchasedArticleWarehouses)
                    {
                        if (purchasedArticleWarehouse.QtyPackages > 0 || purchasedArticleWarehouse.QtyExtra > 0)
                        {
                            tempPurchasedArticleWarehouse.QtyPackages += purchasedArticleWarehouse.QtyPackages;
                            tempPurchasedArticleWarehouse.QtyExtra += purchasedArticleWarehouse.QtyExtra;

                            purchasedArticleWarehouse.PurchasedArticleID = savedPurchasedArticle.ID;
                            purchasedArticleWarehouse.AddedDate = savedPurchasedArticle.AddedDate;
                            purchasedArticleWarehouse.ModifiedDate = DateTime.Now;
                            _context.Add(purchasedArticleWarehouse);
                           
                        }
                    }

                    articleInOut.ArticleID = tempPurchasedArticleWarehouse.PurchasedArticle.ArticleID;
                    articleInOut.WarehouseID = tempPurchasedArticleWarehouse.WarehouseID;
                    articleInOut.CompanyID = ArticleBalance.GetWarehouseCompany(_context, tempPurchasedArticleWarehouse.Warehouse);
                    articleInOut.QtyPackages = tempPurchasedArticleWarehouse.QtyPackages;
                    articleInOut.QtyExtra = tempPurchasedArticleWarehouse.QtyExtra;
                    
                    result = ArticleBalance.Add(_context, articleInOut);
                   // result = articleBalance.AddPurchasedArticleToBalance(tempPurchasedArticleWarehouse);
                    if (result.Value == false)
                    {
                        ModelState.AddModelError("", "Couldn't save.");
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", JSonHelper.ToJSon(result));
                        }
                        model.PurchasedArticleWarehouses = GetPurchasedArticleWarehouse(savedPurchasedArticle?.ArticleID ?? 0);
                        AddPurchasedArticleLists(model);
                        return View(model);

                    }
                    savedPurchasedArticle.ArticleID = model.PurchasedArticle.ArticleID;
                    savedPurchasedArticle.Date = model.PurchasedArticle.Date;
                    savedPurchasedArticle.CompanyID = model.PurchasedArticle.CompanyID;
                    savedPurchasedArticle.ContainerNumber = model.PurchasedArticle.ContainerNumber;
                    savedPurchasedArticle.Description = model.PurchasedArticle.Description;
                    _context.Update(savedPurchasedArticle);

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Couldn't save.");
                    model.PurchasedArticleWarehouses = GetPurchasedArticleWarehouse(0);
                    AddPurchasedArticleLists(model);
                    return View(model);
                }

                   
            }

            // return RedirectToAction(nameof(Index));

            
            ViewData["ArticleID"] = new SelectList(_context.Articles, "ID", "Name", model.PurchasedArticle.ArticleID);
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", model.PurchasedArticle.CompanyID);
            return View(model);
        }

     
        // GET: Stock/PurchasedArticles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchasedArticle = await _context.PurchasedArticles
                .Include(r => r.Article)
                .Include(r => r.Company)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (purchasedArticle == null)
            {
                return NotFound();
            }

            return View(purchasedArticle);
        }

        // POST: Stock/PurchasedArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var savedPurchasedArticle = await _context.PurchasedArticles.Include(w => w.Warehouses).SingleOrDefaultAsync(m => m.ID == id);
            //rollback old values from article balance
            if(savedPurchasedArticle.Warehouses.Count() > 0)
            {
              
                var onThewayWarehouse = _context.Warehouses.Where(w => w.IsOnTheWay).FirstOrDefault();
                var tempPurchasedArticleWarehouse = new PurchasedArticleWarehouse();
                tempPurchasedArticleWarehouse.WarehouseID = onThewayWarehouse.ID;
                tempPurchasedArticleWarehouse.Warehouse = onThewayWarehouse;
                tempPurchasedArticleWarehouse.PurchasedArticleID = savedPurchasedArticle.ID;
                tempPurchasedArticleWarehouse.PurchasedArticle = savedPurchasedArticle;
                foreach (var purchasedArticleWarehouse in savedPurchasedArticle.Warehouses)
                {
                    if (purchasedArticleWarehouse.QtyPackages > 0 || purchasedArticleWarehouse.QtyExtra > 0)
                    {
                        tempPurchasedArticleWarehouse.QtyPackages += purchasedArticleWarehouse.QtyPackages;
                        tempPurchasedArticleWarehouse.QtyExtra += purchasedArticleWarehouse.QtyExtra;
                        _context.Remove(purchasedArticleWarehouse);
                    }
                }
                var articleInOut = new ArticleInOut
                {
                    ArticleID = tempPurchasedArticleWarehouse.PurchasedArticle.ArticleID,
                    WarehouseID = tempPurchasedArticleWarehouse.WarehouseID,
                    CompanyID = ArticleBalance.GetWarehouseCompany(_context, tempPurchasedArticleWarehouse.Warehouse),
                    QtyPackages = (tempPurchasedArticleWarehouse.QtyPackages * -1),
                    QtyExtra = (tempPurchasedArticleWarehouse.QtyExtra * -1)
                };
                var result = ArticleBalance.Reduce(_context, articleInOut);
               // var result = articleBalance.RemovePurchasedArticleFromBalance(tempPurchasedArticleWarehouse);
                if (result.Value == false)
                {
                    ModelState.AddModelError("", "Couldn't save.");
                    if (_hostingEnvironment.IsDevelopment())
                    {
                        ModelState.AddModelError("", JSonHelper.ToJSon(result));
                    }
                    return View(savedPurchasedArticle);

                }
            }
            _context.PurchasedArticles.Remove(savedPurchasedArticle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchasedArticleExists(int id)
        {
            return _context.PurchasedArticles.Any(e => e.ID == id);
        }

        private void AddPurchasedArticleLists(SavePurchasedArticleWarehouseViewModel model)
        {
            ViewData["ArticleID"] = new SelectList(_context.Articles.Include(u => u.ArticleUnit).Include(f => f.ArticlePackageForm), "ID", "Name", model.PurchasedArticle?.ArticleID);
            ViewData["CompanyID"] = new SelectList(_context.Companies.Where(c => c.IsOwner == false), "ID", "Name", model.PurchasedArticle?.CompanyID);
           
        }

        #region 
        private decimal GetTotalPerUnit(PurchasedArticleWarehouse model)
        {
            decimal result = 0;
            var article = _context.Articles.Where(a => a.ID == model.PurchasedArticle.ArticleID).FirstOrDefault();
            var unit = _context.ArticleUnits.Where(u => u.ID == article.ArticleUnitID).FirstOrDefault();
            if (unit != null)
            {
                if (unit.MeasuresByKg)
                {
                    result = (model.QtyPackages * article.WeightPerPackage) + model.QtyExtra;
                }
                else
                {
                    result = model.QtyPackages;
                }
            }

            return result;
        }
        private List<PurchasedArticleViewModel> GetIndexList()
        {
            var purchasedArticles = _context.PurchasedArticles.Include(r => r.Article).ThenInclude(u => u.ArticleUnit).Include(r => r.Company).Include(r => r.Warehouses).OrderBy(r => r.Date).ThenBy(r => r.Article.Name).ThenBy(r => r.Company.Name).AsNoTracking().ToList();
            var resultList = new List<PurchasedArticleViewModel>();
            foreach (var item in purchasedArticles)
            {
                var purchasedArticleViewModel = new PurchasedArticleViewModel
                {
                    ID = (int)item.ID,
                    Article = item.Article,
                    Company = item.Company ?? new Company(),
                    Date = item.Date,
                    TotalPerUnit = item.TotalPerUnit,
                    Received = item.Received,
                    ExpectedToArrive = item.ExpectedToArrive

                };
                
                Dictionary<string, decimal> tempList = new Dictionary<string, decimal>();
                purchasedArticleViewModel.Warehouses = new Dictionary<string, decimal>();
                var warehouses = _context.PurchasedArticleWarehouses.Where(r => r.PurchasedArticleID == item.ID).Include(w => w.Warehouse);
                foreach (var warehouse in warehouses)
                {
                    purchasedArticleViewModel.TotalPerUnit += (warehouse.QtyPackages * item.Article.WeightPerPackage) + warehouse.QtyExtra;
                    if (purchasedArticleViewModel.WarehouseSummary != null)
                        purchasedArticleViewModel.WarehouseSummary += "|";
                    var tempWarehouse = _context.Warehouses.Where(w => w.ID == warehouse.WarehouseID).FirstOrDefault();
                    if (tempWarehouse != null)
                    {
                        purchasedArticleViewModel.Warehouses.Add(tempWarehouse.Name, (warehouse.QtyPackages * item.Article.WeightPerPackage) + warehouse.QtyExtra);
                        purchasedArticleViewModel.WarehouseSummary += tempWarehouse.Name + ":" + ((warehouse.QtyPackages * item.Article.WeightPerPackage) + warehouse.QtyExtra).ToString();

                    }

                }
                resultList.Add(purchasedArticleViewModel);
            }
            return resultList;
        }

        private List<PurchasedArticleWarehouse> GetPurchasedArticleWarehouse(int id = 0)
        {
            var result = new List<PurchasedArticleWarehouse>();
            
            foreach(var warehouse in _context.Warehouses.Where(w => w.IsOnTheWay == true).OrderBy(w => w.Name))
                {
                    var tempPurchasedArticleWarehouse = new PurchasedArticleWarehouse();
                    if (id != 0)
                    {
                        var savedPurchasedArticleWarehouse = _context.PurchasedArticleWarehouses.Where(r => r.PurchasedArticleID == id && r.WarehouseID == warehouse.ID).FirstOrDefault();
                        if(savedPurchasedArticleWarehouse != null)
                        {
                           // tempPurchasedArticleWarehouse.ID = savedPurchasedArticleWarehouse.ID;
                            tempPurchasedArticleWarehouse.PurchasedArticleID = savedPurchasedArticleWarehouse.PurchasedArticleID;
                            tempPurchasedArticleWarehouse.WarehouseID = warehouse.ID;
                            tempPurchasedArticleWarehouse.Warehouse = warehouse;
                            tempPurchasedArticleWarehouse.QtyPackages = savedPurchasedArticleWarehouse.QtyPackages;
                            tempPurchasedArticleWarehouse.QtyExtra = savedPurchasedArticleWarehouse.QtyExtra;
                        }
                        else
                        {
                            //tempPurchasedArticleWarehouse.ID = 0;
                            tempPurchasedArticleWarehouse.PurchasedArticleID = 0;
                            tempPurchasedArticleWarehouse.WarehouseID = warehouse.ID;
                            tempPurchasedArticleWarehouse.Warehouse = warehouse;
                            tempPurchasedArticleWarehouse.QtyPackages = 0;
                            tempPurchasedArticleWarehouse.QtyExtra = 0;
                        }
                         

                        
                    }
                    else
                    {
                        // tempPurchasedArticleWarehouse.ID = 0;
                         tempPurchasedArticleWarehouse.PurchasedArticleID = 0;
                         tempPurchasedArticleWarehouse.WarehouseID = warehouse.ID;
                         tempPurchasedArticleWarehouse.Warehouse = warehouse;
                         tempPurchasedArticleWarehouse.QtyPackages = 0;
                         tempPurchasedArticleWarehouse.QtyExtra = 0;
                    }
                 
                
                    
                    result.Add(tempPurchasedArticleWarehouse);
                }
           
            return result;
        }

        #endregion
    }
}
