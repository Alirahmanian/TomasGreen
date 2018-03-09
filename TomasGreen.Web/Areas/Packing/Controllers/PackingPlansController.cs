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
using TomasGreen.Web.Areas.Packing.ViewModels;

namespace TomasGreen.Web.Areas.Packing.Controllers
{
    [Area("Packing")]
    public class PackingPlansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<PackingPlansController> _localizer;

        public PackingPlansController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<PackingPlansController> localizer)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
        }

        // GET: Packing/PackingPlans
        public async Task<IActionResult> Index()
        {
            return View(await _context.PackingPlans.ToListAsync());
        }

        // GET: Packing/PackingPlans/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packingPlan = await _context.PackingPlans
                .SingleOrDefaultAsync(m => m.ID == id);
            if (packingPlan == null)
            {
                return NotFound();
            }

            return View(packingPlan);
        }

        // GET: Packing/PackingPlans/Create
        public IActionResult Create(Int64 id)
        {
            var model = new PackingPlanViewModel();
            model.PackingPlanMix = new PackingPlanMix();
            model.PackingPlanMixArticle = new PackingPlanMixArticle();
            if (id > 0)
            {
                model.PackingPlan = _context.PackingPlans.Where(p => p.ID == id).FirstOrDefault();
               // model.PackingPlan.Mixes = _context.PackingPlanMixs.Where(m => m.PackingPlanID == model.PackingPlan.ID).ToList();
                
                if (model.PackingPlan != null)
                {
                    model.PackingPlanMix.PackingPlanID = model.PackingPlan.ID;

                }
            }
            else
            {
                model.PackingPlan = new PackingPlan();
                model.PackingPlan.Date = DateTime.Today;

            }
            GetPackingPlanLists(model);
           
            return View(model);
        }

        // POST: Packing/PackingPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PackingPlanViewModel model, string SavePackingPlan, string SavePackingPlanMix, string AddMixArticle)
        {
            var skippedErrors = ModelState.Keys;
            foreach (var key in skippedErrors)
            {
                ModelState.Remove(key);
            }
            if (!string.IsNullOrWhiteSpace(SavePackingPlanMix))
            {
                if (model.PackingPlan.ID == 0)
                {
                    ModelState.AddModelError("", "Please save the Packing header first.");
                    return View(model);

                }

            }
            if (!string.IsNullOrWhiteSpace(AddMixArticle))
            {
                if (model.PackingPlanMix.ID == 0)
                {
                    ModelState.AddModelError("", "Please save the Mix and packing before adding articles to it.");
                    return View(model);

                }

            }

            if (ModelState.IsValid)
            {
               // _context.Add(packingPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Packing/PackingPlans/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packingPlan = await _context.PackingPlans.SingleOrDefaultAsync(m => m.ID == id);
            if (packingPlan == null)
            {
                return NotFound();
            }
            return View(packingPlan);
        }

        // POST: Packing/PackingPlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Date,ManagerID,CompanyID")] PackingPlan packingPlan)
        {
            if (id != packingPlan.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(packingPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PackingPlanExists(packingPlan.ID))
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
            return View(packingPlan);
        }

        // GET: Packing/PackingPlans/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packingPlan = await _context.PackingPlans
                .SingleOrDefaultAsync(m => m.ID == id);
            if (packingPlan == null)
            {
                return NotFound();
            }

            return View(packingPlan);
        }

        // POST: Packing/PackingPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var packingPlan = await _context.PackingPlans.SingleOrDefaultAsync(m => m.ID == id);
            _context.PackingPlans.Remove(packingPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PackingPlanExists(long id)
        {
            return _context.PackingPlans.Any(e => e.ID == id);
        }

        //======================================================================
        //======================================================================
        private void GetPackingPlanLists(PackingPlanViewModel model)
        {
            ViewData["NewArticleID"] = new SelectList(_context.Articles, "ID", "Name", model?.PackingPlanMix?.NewArticleID);
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", model?.PackingPlan?.CompanyID);
            ViewData["ManagerID"] = new SelectList(_context.Employees, "ID", "FullName", model?.PackingPlan?.ManagerID);
            ViewData["PackagingMaterialPackageID"] = new SelectList(_context.PackagingMaterials, "ID", "Name", model?.PackingPlanMix?.PackagingMaterialPackageID);
            ViewData["PackagingMaterialBagID"] = new SelectList(_context.PackagingMaterials, "ID", "Name", model?.PackingPlanMix?.PackagingMaterialBagID);

            if (model?.PackingPlan?.CompanyID > 0)
            {
                ViewData["WarehouseID"] = new SelectList(GetFromWarehouseList(model.PackingPlan.CompanyID), "ID", "Name", model.PackingPlanMixArticle.WarehouseID);
                ViewData["ToWarehouseID"] = new SelectList(GetToWarehouseList(model.PackingPlan.CompanyID), "ID", "Name", model.PackingPlanMix.ToWarehouseID);
                if (model.PackingPlanMixArticle.WarehouseID > 0)
                {
                    ViewData["ArticleID"] = new SelectList(GetArticleList(model.PackingPlanMixArticle.WarehouseID, model.PackingPlan.CompanyID), "ID", "Name", model.PackingPlanMixArticle.ArticleID);
                }
            }
        }

        private decimal GetTotalWeight(long articleID, int qtyPackages, decimal qtExtra)
        {
            decimal result = 0;
            var article = _context.Articles.Include(u => u.ArticleUnit).Where(a => a.ID == articleID).FirstOrDefault();
            if (article == null)
            {
                return result;
            }
            if (article.ArticleUnit.MeasuresByKg)
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
            foreach (var article in articles)
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
