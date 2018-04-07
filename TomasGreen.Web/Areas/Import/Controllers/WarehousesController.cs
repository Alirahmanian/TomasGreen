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
using TomasGreen.Web.Areas.Import.ViewModels;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Areas.Import.Controllers
{
    [Area("Import")]
   // [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class WarehousesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<WarehousesController> _localizer;
        public WarehousesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<WarehousesController> localizer)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
        }

        // GET: Stock/Warehouses
        public async Task<IActionResult> Index()
        {
            //.Any(b => b.QtyExtraOnhand > 0 || b.QtyPackagesOnhand > 0)
            return View(await _context.Warehouses.Include(a => a.ArticleWarehouseBalances).Include(s => s.Section).ToListAsync());
        }

        // GET: Stock/Warehouses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses
                .SingleOrDefaultAsync(m => m.ID == id);
            if (warehouse == null)
            {
                return NotFound();
            }
            var warehouseDetailsViewModel = new WarehouseDetailsViewModel();
            warehouseDetailsViewModel.Warehouse = warehouse;
            warehouseDetailsViewModel.Articles = _context.ArticleWarehouseBalances.Include(a => a.Article).Include(c => c.Company)
                .Where(b => b.WarehouseID == warehouse.ID && (b.QtyExtraOnhand > 0 || b.QtyPackagesOnhand > 0)).ToList();
            return View(warehouseDetailsViewModel);
        }

        // GET: Stock/Warehouses/Create
        public IActionResult Create()
        {
            ViewData["CompanySectionID"] = new SelectList(_context.CompanySections, "ID", "Name");

            return View();
        }

        // POST: Stock/Warehouses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Address,Phone,IsOnTheWay,IsCutomers,CompanySectionID")] Warehouse warehouse)
        {
            var skippedErrors = ModelState.Keys.Where(key => key.StartsWith("Warehouse.CompanySectionID"));
            foreach (var key in skippedErrors)
            {
                ModelState.Remove(key);
            }
            if (ModelState.IsValid)
            {
                if (warehouse.CompanySectionID == 0)
                {
                    warehouse.CompanySectionID = null;
                }
                if (!VerifyUniqueName(warehouse.Name, (int)warehouse.ID))
                {
                    ViewData["CompanySectionID"] = new SelectList(_context.CompanySections, "ID", "Name");
                    ModelState.AddModelError(nameof(warehouse.Name), _localizer["Name is already taken."]);
                    return View(warehouse);
                }
                if (!VerifyUniqueIsOnTheWayWarehouse(warehouse))
                {
                    ViewData["CompanySectionID"] = new SelectList(_context.CompanySections, "ID", "Name");
                    ModelState.AddModelError(nameof(warehouse.IsOnTheWay), _localizer["There is already an on the way warehouse."]);
                    return View(warehouse);
                }
                if(!VerifyUniqueIsCustomersWarehouse(warehouse))
                {
                    ViewData["CompanySectionID"] = new SelectList(_context.CompanySections, "ID", "Name");
                    ModelState.AddModelError(nameof(warehouse.IsCustomers), _localizer["There is already a customer warehouse."]);
                    return View(warehouse);
                }
                if (!VerifyIsCustomersWithNoSectionWarehouse(warehouse))
                {
                    ViewData["CompanySectionID"] = new SelectList(_context.CompanySections, "ID", "Name");
                    ModelState.AddModelError(nameof(warehouse.CompanySectionID), _localizer["Customer warehouse can't have section."]);
                    return View(warehouse);
                }
                _context.Add(warehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // GET: Stock/Warehouses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses.SingleOrDefaultAsync(m => m.ID == id);
            if (warehouse == null)
            {
                return NotFound();
            }
            ViewData["CompanySectionID"] = new SelectList(_context.CompanySections, "ID", "Name");
            return View(warehouse);
        }

        // POST: Stock/Warehouses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,Address,Phone,AddedDate,IsOnTheWay,IsCustomers,CompanySectionID")] Warehouse warehouse)
        {
            if (id != warehouse.ID)
            {
                return NotFound();
            }
            var skippedErrors = ModelState.Keys.Where(key => key.StartsWith("Warehouse.CompanySectionID"));
            foreach (var key in skippedErrors)
            {
                ModelState.Remove(key);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if(warehouse.CompanySectionID == 0)
                    {
                        warehouse.CompanySectionID = null;
                    }
                    if (!VerifyUniqueName(warehouse.Name, (int)warehouse.ID))
                    {
                        ViewData["CompanySectionID"] = new SelectList(_context.CompanySections, "ID", "Name");
                        ModelState.AddModelError(nameof(warehouse.Name), _localizer["Name is already taken."]);
                        return View(warehouse);
                    }
                    if (!VerifyUniqueIsOnTheWayWarehouse(warehouse))
                    {
                        ViewData["CompanySectionID"] = new SelectList(_context.CompanySections, "ID", "Name");
                        ModelState.AddModelError(nameof(warehouse.IsOnTheWay), _localizer["There is already an on the way warehouse."]);
                        return View(warehouse);
                    }
                    if (!VerifyUniqueIsCustomersWarehouse(warehouse))
                    {
                        ViewData["CompanySectionID"] = new SelectList(_context.CompanySections, "ID", "Name");
                        ModelState.AddModelError(nameof(warehouse.IsCustomers), _localizer["There is already a customer warehouse."]);
                        return View(warehouse);
                    }
                    
                    if(!VerifyIsCustomersWithNoSectionWarehouse(warehouse))
                    {
                        ViewData["CompanySectionID"] = new SelectList(_context.CompanySections, "ID", "Name");
                        ModelState.AddModelError(nameof(warehouse.CompanySectionID), _localizer["Customer warehouse can't have section."]);
                        return View(warehouse);
                    }
                    _context.Update(warehouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarehouseExists(warehouse.ID))
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
            else
            {
                ViewData["CompanySectionID"] = new SelectList(_context.CompanySections, "ID", "Name");
                return View(warehouse);

            }
            return View(warehouse);
        }

        // GET: Stock/Warehouses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses
                .SingleOrDefaultAsync(m => m.ID == id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // POST: Stock/Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouse = await _context.Warehouses.SingleOrDefaultAsync(m => m.ID == id);
            if(warehouse != null)
            {

            }
            //_context.Warehouses.Remove(warehouse);
           // await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WarehouseExists(int id)
        {
            return _context.Warehouses.Any(e => e.ID == id);
        }

        #region Validations
        public bool VerifyDependencies(Warehouse model)
        {
            if (model.ID == 0)
                return true;
            var warehouse = _context.Warehouses.Include(w => w.PurchasedArticleDetails).Include(w => w.OrderDetails).Where(w => w.ID == model.ID).FirstOrDefault();
            if (warehouse.OrderDetails.Count > 0 || warehouse.PurchasedArticleDetails.Count > 0)
                return false;
          
            
            return true;
        }
        public bool VerifyUniqueName(string name, int id)
        {

            var warehouse = _context.Warehouses.Where(a => a.Name == name).AsNoTracking().FirstOrDefault();
            if (warehouse != null)
            {
                if (id != 0)
                {
                    if (warehouse.ID != id)
                        return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public bool VerifyUniqueIsOnTheWayWarehouse(Warehouse model)
        {
            if (model.IsOnTheWay)
            {
                var warehouse = _context.Warehouses.Where(a => a.IsOnTheWay == true).AsNoTracking().FirstOrDefault();
                if (warehouse != null)
                {
                    if (model.ID > 0)
                    {
                        if (warehouse.ID == model.ID)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool VerifyUniqueIsCustomersWarehouse(Warehouse model)
        {
            if (model.IsCustomers)
            {
                var warehouse = _context.Warehouses.Where(a => a.IsCustomers == true).AsNoTracking().FirstOrDefault();
                if (warehouse != null)
                {
                    if (model.ID > 0)
                    {
                        if (warehouse.ID == model.ID)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool VerifyIsCustomersWithNoSectionWarehouse(Warehouse model)
        {
            if(model.IsCustomers && model.CompanySectionID != null && model.CompanySectionID != 0)
            {
                return false;
            }
            
            return true;
        }
        public bool VerifyUniqueCompanySectionWarehouse(Warehouse model)
        {
            if(model.CompanySectionID != null)
            {
                var warehouse = _context.Warehouses.Where(a => a.CompanySectionID == model.CompanySectionID).AsNoTracking().FirstOrDefault();
                if (warehouse != null)
                {
                    if (model.ID > 0)
                    {
                        if (warehouse.ID == model.ID)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

            #endregion
     }
}
