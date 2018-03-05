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
            var applicationDbContext = _context.RoastingPlans.Include(r => r.Article).Include(r => r.Company).Include(r => r.Manager).Include(r => r.Warehouse);
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
                .Include(r => r.Article)
                .Include(r => r.Company)
                .Include(r => r.Manager)
                .Include(r => r.Warehouse)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (roastingPlan == null)
            {
                return NotFound();
            }

            return View(roastingPlan);
        }

        // GET: Roasting/RoastingPlans/Create
        public IActionResult Create()
        {
            GetRoastingPlanLists();
            return View();
        }

        // POST: Roasting/RoastingPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WarehouseID,ArticleID,CompanyID,ManagerID,Date,QtyPackages,QtExtra,ArticleUnitID,WeightPerPackage,TotalWeight,WeightChange,NewArticleID,NewQtyPackages,NewQtyExtra,NewArticleUnitID,NewWeightPerPackage,NewTotalWeight,Salt,NewPackagingMaterialPackageID,NewPackages,NewPackagingMaterialBagID,NewBags,PricePerUnit,TotalPrice")] RoastingPlan roastingPlan)
        {

            if (ModelState.IsValid)
            {
                _context.Add(roastingPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArticleID"] = new SelectList(_context.Articles, "ID", "Name", roastingPlan.ArticleID);
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", roastingPlan.CompanyID);
            ViewData["ManagerID"] = new SelectList(_context.Employees, "ID", "Email", roastingPlan.ManagerID);
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "Name", roastingPlan.WarehouseID);
            return View(roastingPlan);
        }

        // GET: Roasting/RoastingPlans/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roastingPlan = await _context.RoastingPlans.SingleOrDefaultAsync(m => m.ID == id);
            if (roastingPlan == null)
            {
                return NotFound();
            }
            GetRoastingPlanLists();
            return View(roastingPlan);
        }

        // POST: Roasting/RoastingPlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("WarehouseID,ArticleID,CompanyID,ManagerID,Date,QtyPackages,QtExtra,ArticleUnitID,WeightPerPackage,TotalWeight,WeightChange,NewArticleID,NewQtyPackages,NewQtyExtra,NewArticleUnitID,NewWeightPerPackage,NewTotalWeight,Salt,SaltLimit,NewPackagingMaterialPackageID,NewPackages,NewPackagingMaterialBagID,NewBags,PricePerUnit,TotalPrice")] RoastingPlan roastingPlan)
        {
            if (id != roastingPlan.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roastingPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoastingPlanExists(roastingPlan.ID))
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
            GetRoastingPlanLists();
            return View(roastingPlan);
        }

        // GET: Roasting/RoastingPlans/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roastingPlan = await _context.RoastingPlans
                .Include(r => r.Article)
                .Include(r => r.Company)
                .Include(r => r.Manager)
                .Include(r => r.Warehouse)
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

        private void GetRoastingPlanLists()
        {
            // ViewData["ArticleCategoryID"] = new SelectList(_context.ArticleCategories, "ID", "Name");
          //  ViewData["NewArticleCategoryID"] = new SelectList(_context.ArticleCategories, "ID", "Name");
           // ViewData["ArticleID"] = new SelectList(_context.Articles, "ID", "Name");
          //  ViewData["NewArticleID"] = new SelectList(_context.Articles, "ID", "Name");
            ViewData["ArticleUnitID"] = new SelectList(_context.ArticleUnits, "ID", "Name");
            ViewData["NewArticleUnitID"] = new SelectList(_context.ArticleUnits, "ID", "Name");
            ViewData["NewPackagingMaterialPackageID"] = new SelectList(_context.PackagingMaterials, "ID", "Name");
            ViewData["NewPackagingMaterialBagID"] = new SelectList(_context.PackagingMaterials, "ID", "Name");
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name");
            ViewData["ManagerID"] = new SelectList(_context.Employees, "ID", "FullName");
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "Name");

        }
        #region Ajax Calls
        public JsonResult GetCompaniessByWarehouseId(int warehouseID)
        {
            List<Company> companies = new List<Company>();
            if (_context.Warehouses.Any(w => w.ID == warehouseID && w.IsCustomers == true))
            {
               // var companies = _context.
            }
           

            return new JsonResult(companies.ToList());
        }
        public JsonResult GetArticlesByWarehouseId(int warehouseID)
        {
            //if(_context.Warehouses.Any(w => w.ID == warehouseID && w.IsCustomers == true))
            //{

            //}
            //else
            //{

            //}
            var articles = (from aw in _context.ArticleWarehouseBalances
                            where aw.WarehouseID == warehouseID
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
