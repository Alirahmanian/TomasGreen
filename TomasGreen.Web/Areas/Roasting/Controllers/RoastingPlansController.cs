using System;
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
using TomasGreen.Web.Validations;
using TomasGreen.Web.Balances;
using TomasGreen.Web.Extensions;

namespace TomasGreen.Web.Areas.Roasting.Controllers
{
    [Area("Roasting")]
    public class RoastingPlansController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<RoastingPlansController> _localizer;
        public RoastingPlansController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<RoastingPlansController> localizer)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
        }

        

        // GET: Roasting/RoastingPlans
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RoastingPlans.Include(r => r.Company).Include(r => r.Manager);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Roasting/RoastingPlans/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roastingPlan = await _context.RoastingPlans
               
                .Include(r => r.Company)
                .Include(r => r.Manager)
                
                .SingleOrDefaultAsync(m => m.ID == id);
            if (roastingPlan == null)
            {
                return NotFound();
            }

            return View(roastingPlan);
        }

        // GET: Roasting/RoastingPlans/Create
        public IActionResult Create(Int64? id)
        {
            if(id > 0)
            {
               var roastingPlan = _context.RoastingPlans.Where( r => r.ID == id)
              .Include(m => m.Manager)
              .Include(c => c.Company)
              .FirstOrDefault();
               GetRoastingPlanLists(roastingPlan);
               return View(roastingPlan);
            }

            var model = new RoastingPlan();
            GetRoastingPlanLists(model);
            return View();
        }

        // POST: Roasting/RoastingPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoastingPlan model)
        {
            var skippedErrors = ModelState.Keys.Where(key => key.StartsWith(nameof(model.PackagingMaterialPackageID)) || key.StartsWith(nameof(model.PackagingMaterialBagID)));
            foreach (var key in skippedErrors)
            {
                ModelState.Remove(key);
            }
            if (ModelState.IsValid)
            {
                // Server side validation
                var customModelValidator = RoastingPlanValidation.RoastingPlanIsValid(_context, "Create", model);
                if (customModelValidator.Value == false)
                {
                    ModelState.AddModelError("", "Could't save, please try again.");
                    ModelState.AddModelError(customModelValidator.Property, customModelValidator.Message);
                    GetRoastingPlanLists(model);
                    return View(model);
                }
                if(model.ID == 0)
                {
                    model.TotalWeight = GetTotalWeight(model.ArticleID, model.QtyPackages, model.QtyExtra);
                    model.NewTotalWeight = GetTotalWeight(model.NewArticleID, model.NewQtyPackages, model.NewQtyExtra);
                    model.TotalPrice = model.NewTotalWeight * model.PricePerUnit;
                    

                    var articleInOut = new ArticleInOut
                    {
                        ArticleID = model.ArticleID,
                        WarehouseID = model.FromWarehouseID,
                        CompanyID = (ArticleBalance.WarehouseIsCoutomers(_context, model.FromWarehouseID) == false)? ArticleBalance.GetWarehouseCompany(_context, model.FromWarehouseID) : model.CompanyID,
                        QtyPackagesOut = model.QtyPackages,
                        QtyExtraOut = model.QtyExtra
                    };
                    var result = ArticleBalance.Reduce(_context, articleInOut);
                    if (result.Value == false)
                    {
                        ModelState.AddModelError("", "Couldn't saved.");
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", JSonHelper.ToJSon(result));
                        }
                        GetRoastingPlanLists(model);
                        return View(model);
                    }
                    _context.Add(model);
                }
                else
                {


                    _context.Update(model);
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Could't save, please try again.");
                GetRoastingPlanLists(model);
            }

            return View(model);
        }

        

        // GET: Roasting/RoastingPlans/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.RoastingPlans
                .Include(m => m.Manager)
                .Include(c => c.Company)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (model == null)
            {
                return NotFound();
            }
            GetRoastingPlanLists(model);
            return View(model);
        }

        // POST: Roasting/RoastingPlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id,  RoastingPlan model)
        {
            if (id != model.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoastingPlanExists(model.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            GetRoastingPlanLists(model);
            return View(model);
        }

        // GET: Roasting/RoastingPlans/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roastingPlan = await _context.RoastingPlans
               
                .Include(r => r.Company)
                .Include(r => r.Manager)
              
                .SingleOrDefaultAsync(m => m.ID == id);
            if (roastingPlan == null)
            {
                return NotFound();
            }

            return View(roastingPlan);
        }

        // POST: Roasting/RoastingPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var roastingPlan = await _context.RoastingPlans.SingleOrDefaultAsync(m => m.ID == id);
            _context.RoastingPlans.Remove(roastingPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoastingPlanExists(long id)
        {
            return _context.RoastingPlans.Any(e => e.ID == id);
        }

        //======================================================================
        //======================================================================
        private void GetRoastingPlanLists(RoastingPlan model)
        {
            ViewData["NewArticleID"] = new SelectList(_context.Articles, "ID", "Name", model?.NewArticleID);
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", model?.CompanyID);
            ViewData["ManagerID"] = new SelectList(_context.Employees, "ID", "FullName", model?.ManagerID);
            ViewData["PackagingMaterialPackageID"] = new SelectList(_context.PackagingMaterials, "ID", "Name", model?.PackagingMaterialPackageID);
            ViewData["PackagingMaterialBagID"] = new SelectList(_context.PackagingMaterials, "ID", "Name", model?.PackagingMaterialBagID);

            if (model.CompanyID > 0)
            {
               ViewData["FromWarehouseID"] = new SelectList(GetFromWarehouseList(model.CompanyID), "ID", "Name", model?.FromWarehouseID);
               ViewData["ToWarehouseID"] = new SelectList(GetToWarehouseList(model.CompanyID), "ID", "Name", model?.ToWarehouseID);
                if(model.FromWarehouseID > 0)
                {
                    ViewData["ArticleID"] = new SelectList(GetArticleList(model.FromWarehouseID, model.CompanyID), "ID", "Name", model?.ArticleID);
                }
            }
        }

        private decimal GetTotalWeight(long articleID, int qtyPackages, decimal qtExtra)
        {
            decimal result = 0;
            var article = _context.Articles.Include(u => u.ArticleUnit).Where(a => a.ID == articleID).FirstOrDefault();
            if(article == null)
            {
                return result;
            }
            if(article.ArticleUnit.MeasuresByKg)
            {
                result = (qtyPackages * article.WeightPerPackage) + qtExtra;
            }
            else
            {
                result = qtyPackages;
            }
            
            return result;
        }
        public List<Warehouse> GetFromWarehouseList(Int64 companyID)
        {
            var warehouses = (from aw in _context.ArticleWarehouseBalances
                              where aw.CompanyID == companyID && (aw.QtyPackagesOnhand > 0 || aw.QtyExtraOnhand > 0)
                              join w in _context.Warehouses on aw.WarehouseID equals w.ID
                              orderby w.Name
                              select w
                             ).GroupBy(g => g.Name).Select(z => z.OrderBy(i => i.Name).First()).ToList();


            return warehouses;
        }
        public List<Warehouse> GetToWarehouseList(Int64 companyID)
        {
            List<Warehouse> warehouses = new List<Warehouse>();
            var company = _context.Companies.Where(c => c.ID == companyID).FirstOrDefault();
            if (company != null)
            {
                if (company.IsOwner)
                {
                    var ownerWarehouses = _context.Warehouses.Where(w => w.Archive == false).OrderBy(w => w.Name);
                    return ownerWarehouses.ToList();
                }
                else
                {
                    var customerWarehouses = _context.Warehouses.Where(w => w.IsCustomers == true && w.Archive == false).OrderBy(w => w.Name);
                    return customerWarehouses.ToList();
                }
            }
            return warehouses;
        }
        public List<ListItem> GetArticleList(Int64 warehouseID, Int64 companyID)
        {
            List<ListItem> listItems = new List<ListItem>();
            var articles = (from aw in _context.ArticleWarehouseBalances
                            where aw.CompanyID == companyID && aw.WarehouseID == warehouseID
                            join a in _context.Articles on aw.ArticleID equals a.ID
                            join w in _context.Warehouses on aw.WarehouseID equals w.ID
                            orderby a.Name
                            select new
                            {
                                Id = a.ID,
                                Name = a.Name + _localizer["[Package"] + ".:|:. " + aw.QtyPackagesOnhand.ToString() + ", " + _localizer["Extra"] + ": " + aw.QtyExtraOnhand.ToString() + "]"
                            }
                           );
            foreach(var article in articles)
            {
                listItems.Add(new ListItem { ID = article.Id, Name = article.Name });
            }
            return listItems;
        }
        //=======================================================================
        //=======================================================================
        #region Ajax Calls
        public JsonResult GetFromWarehousesByCompany(int companyID)
        {
            var warehouses = (from aw in _context.ArticleWarehouseBalances
                                   where aw.CompanyID == companyID && (aw.QtyPackagesOnhand > 0 || aw.QtyExtraOnhand > 0)
                                   join w in _context.Warehouses on aw.WarehouseID equals w.ID
                                   orderby w.Name
                                   
                                    select new
                                    {
                                        Id = w.ID,
                                        Name = w.Name,
                                    }
                                ).GroupBy(g => g.Name).Select(z => z.OrderBy(i => i.Name).First()).ToList();


            return new JsonResult(warehouses);
        }

        public JsonResult GetToWarehousesByCompany(int companyID)
        {
            List<Warehouse> warehouses = new List<Warehouse>();
            var company = _context.Companies.Where(c => c.ID == companyID).FirstOrDefault();
            if (company != null)
            {
                if (company.IsOwner)
                {
                    var ownerWarehouses = _context.Warehouses.Where(w => w.Archive == false).OrderBy(w => w.Name);
                    return new JsonResult(ownerWarehouses.ToList());
                }
                else
                {
                    var customerWarehouses = _context.Warehouses.Where(w => w.IsCustomers == true && w.Archive == false).OrderBy(w => w.Name);
                    return new JsonResult(customerWarehouses);
                }
            }
            return new JsonResult(warehouses);
        }

        public JsonResult GetArticlesByWarehouse(int warehouseID, int companyID)
        {
            var articles = (from aw in _context.ArticleWarehouseBalances
                            where aw.CompanyID == companyID && aw.WarehouseID == warehouseID
                            join a in _context.Articles on aw.ArticleID equals a.ID
                            join w in _context.Warehouses on aw.WarehouseID equals w.ID
                              orderby a.Name
                              select new
                              {
                                  Id = a.ID,
                                  Name = a.Name,
                                  Articlesonhand = _localizer["[Package"] + ": " + aw.QtyPackagesOnhand.ToString() + ", " + _localizer["Extra"] + ": " + aw.QtyExtraOnhand.ToString() + "]"
                              }
                               );
            return new JsonResult(articles.ToList());
        }
        #endregion
    }
}
