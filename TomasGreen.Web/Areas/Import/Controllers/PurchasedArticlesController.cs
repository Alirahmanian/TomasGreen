using System;
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

namespace TomasGreen.Web.Areas.Import.Controllers
{
    [Area("Import")]
    public class PurchasedArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PurchasedArticlesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Stock/PurchasedArticles
        public IActionResult Index()
        {
            var purchasedArticles = GetIndexList();
            return View(purchasedArticles);
        }

        // GET: Stock/PurchasedArticles/Details/5
        public async Task<IActionResult> Details(long? id)
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

        public IActionResult Create(Int64? id)
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
                    model.PurchasedArticleWarehouses = GetPurchasedArticleWarehouse((long)model.PurchasedArticle?.ID);
                    AddPurchasedArticleLists(model);
                    return View(model);
                }
            }

            if (!ModelState.IsValid)
            {
                model.PurchasedArticleWarehouses = GetPurchasedArticleWarehouse((long)model.PurchasedArticle?.ID);
                AddPurchasedArticleLists(model);
                return View(model);
            }
            if(model.PurchasedArticle.ID == 0)
            {
                var guid = Guid.NewGuid();
                model.PurchasedArticle.Guid = guid;
                _context.Add(model.PurchasedArticle);
                await _context.SaveChangesAsync();
                var savedPurchasedArticle = _context.PurchasedArticles.Where(r => r.Date == model.PurchasedArticle.Date && r.ArticleID == r.ArticleID && r.Guid == guid).FirstOrDefault();
                if(savedPurchasedArticle != null)
                {
                    ModelState.AddModelError("", "Couldn't saved.");
                    model.PurchasedArticleWarehouses = GetPurchasedArticleWarehouse(0);
                    AddPurchasedArticleLists(model);
                    return View(model);
                }
                var onThewayWarehouse = _context.Warehouses.Where(w => w.IsOnTheWay).FirstOrDefault();
                if (onThewayWarehouse == null)
                {
                    ModelState.AddModelError("", "Couldn't saved.");
                    model.PurchasedArticleWarehouses = GetPurchasedArticleWarehouse(0);
                    AddPurchasedArticleLists(model);
                    return View(model);
                }
                var articleBalance = new ArticleBalance(_context);
                var tempPurchasesArticleWarehouse = new PurchasedArticleWarehouse();
                tempPurchasesArticleWarehouse.WarehouseID = onThewayWarehouse.ID;
                tempPurchasesArticleWarehouse.Warehouse = onThewayWarehouse;
                tempPurchasesArticleWarehouse.PurchasedArticleID = savedPurchasedArticle.ID;
                tempPurchasesArticleWarehouse.PurchasedArticle = savedPurchasedArticle;
                    foreach (var purchasedArticleWarehouse in model.PurchasedArticleWarehouses)
                    {
                        if (purchasedArticleWarehouse.QtyPackages > 0 || purchasedArticleWarehouse.QtyExtra > 0)
                        {
                            purchasedArticleWarehouse.PurchasedArticleID = savedPurchasedArticle.ID;
                            _context.Add(purchasedArticleWarehouse);
                            tempPurchasesArticleWarehouse.QtyPackages += purchasedArticleWarehouse.QtyPackages;
                            tempPurchasesArticleWarehouse.QtyExtra += purchasedArticleWarehouse.QtyExtra;
                        }
                    }
                var result = articleBalance.AddPurchasedArticleToBalance(tempPurchasesArticleWarehouse);
                if (result.Value == false)
                {
                    ModelState.AddModelError("", "Couldn't saved.");
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
                    var errors = new List<PropertyValidatedMessage>();
                    var articleBalance = new ArticleBalance(_context);
                    var onThewayWarehouse = _context.Warehouses.Where(w => w.IsOnTheWay).FirstOrDefault();
                    var tempPurchasesArticleWarehouse = new PurchasedArticleWarehouse();
                    tempPurchasesArticleWarehouse.WarehouseID = onThewayWarehouse.ID;
                    tempPurchasesArticleWarehouse.Warehouse = onThewayWarehouse;
                    tempPurchasesArticleWarehouse.PurchasedArticleID = savedPurchasedArticle.ID;
                    tempPurchasesArticleWarehouse.PurchasedArticle = savedPurchasedArticle;
                    foreach (var purchasedArticleWarehouse in savedPurchasedArticle.Warehouses)
                    {
                        if (purchasedArticleWarehouse.QtyPackages > 0 || purchasedArticleWarehouse.QtyExtra > 0)
                        {
                            tempPurchasesArticleWarehouse.QtyPackages += purchasedArticleWarehouse.QtyPackages;
                            tempPurchasesArticleWarehouse.QtyExtra += purchasedArticleWarehouse.QtyExtra;
                            _context.Remove(purchasedArticleWarehouse);
                        }
                    }
                  
                    var result = articleBalance.RemovePurchasedArticleFromBalance(tempPurchasesArticleWarehouse);
                    if (result.Value == false)
                    {
                        ModelState.AddModelError("", "Couldn't saved.");
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
                    tempPurchasesArticleWarehouse.QtyPackages = 0;
                    tempPurchasesArticleWarehouse.QtyExtra = 0;
                    foreach (var purchasedArticleWarehouse in model.PurchasedArticleWarehouses)
                    {
                        if (purchasedArticleWarehouse.QtyPackages > 0 || purchasedArticleWarehouse.QtyExtra > 0)
                        {
                            tempPurchasesArticleWarehouse.QtyPackages += purchasedArticleWarehouse.QtyPackages;
                            tempPurchasesArticleWarehouse.QtyExtra += purchasedArticleWarehouse.QtyExtra;

                            purchasedArticleWarehouse.PurchasedArticleID = savedPurchasedArticle.ID;
                            purchasedArticleWarehouse.AddedDate = savedPurchasedArticle.AddedDate;
                            purchasedArticleWarehouse.ModifiedDate = DateTime.Now;
                            _context.Add(purchasedArticleWarehouse);
                           
                        }
                    }
                    result = articleBalance.AddPurchasedArticleToBalance(tempPurchasesArticleWarehouse);
                    if (result.Value == false)
                    {
                        ModelState.AddModelError("", "Couldn't saved.");
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
                    ModelState.AddModelError("", "Couldn't saved.");
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
        public async Task<IActionResult> Delete(long? id)
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
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var purchasedArticle = await _context.PurchasedArticles.Include(w => w.Warehouses).SingleOrDefaultAsync(m => m.ID == id);
            //rollback old values from article balance
            var articleBalance = new ArticleBalance(_context);
            foreach (var purchasedArticleWarehouse in purchasedArticle.Warehouses)
            {
                if (purchasedArticleWarehouse.QtyPackages > 0 || purchasedArticleWarehouse.QtyExtra > 0)
                {
                    var onThewayWarehouse = _context.Warehouses.Where(w => w.IsOnTheWay).FirstOrDefault();
                    var result = articleBalance.RemovePurchasedArticleFromBalance(purchasedArticleWarehouse, onThewayWarehouse);
                   if (result.Value == false)
                    {
                        ModelState.AddModelError("", "Couldn't saved.");
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", JSonHelper.ToJSon(result));
                        }
                        return View(purchasedArticle);

                    }
                    _context.Remove(purchasedArticleWarehouse);
                }
            }

            _context.PurchasedArticles.Remove(purchasedArticle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchasedArticleExists(long id)
        {
            return _context.PurchasedArticles.Any(e => e.ID == id);
        }

        private void AddPurchasedArticleLists(SavePurchasedArticleWarehouseViewModel model)
        {
            ViewData["ArticleID"] = new SelectList(_context.Articles.Include(u => u.ArticleUnit).Include(f => f.ArticlePackageForm), "ID", "Name", model.PurchasedArticle?.ArticleID);
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", model.PurchasedArticle?.CompanyID);
           
        }

        #region 
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

        private List<PurchasedArticleWarehouse> GetPurchasedArticleWarehouse(Int64 id = 0)
        {
            var result = new List<PurchasedArticleWarehouse>();
            
            foreach(var warehouse in _context.Warehouses.Where(w => w.IsOnTheWay == false).OrderBy(w => w.Name))
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
