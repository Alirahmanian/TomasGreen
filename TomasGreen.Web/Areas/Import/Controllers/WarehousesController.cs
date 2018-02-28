using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Areas.Import.Controllers
{
    [Area("Import")]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class WarehousesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<WarehousesController> _localizer;
        public WarehousesController(ApplicationDbContext context, IStringLocalizer<WarehousesController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        // GET: Stock/Warehouses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Warehouses.ToListAsync());
        }

        // GET: Stock/Warehouses/Details/5
        public async Task<IActionResult> Details(long? id)
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

        // GET: Stock/Warehouses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stock/Warehouses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Address,Phone, IsReserve")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                if (!VerifyUniqueName(warehouse.Name, (int)warehouse.ID))
                {
                    ModelState.AddModelError(nameof(warehouse.Name), _localizer["Name is already taken."]);
                    return View(warehouse);
                }
                if (!VerifyUniqueIsReserveWarehouse(warehouse))
                {
                    ModelState.AddModelError(nameof(warehouse.IsReserve), _localizer["There is already a reserve warehouse."]);
                    return View(warehouse);
                }
                _context.Add(warehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // GET: Stock/Warehouses/Edit/5
        public async Task<IActionResult> Edit(long? id)
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
            return View(warehouse);
        }

        // POST: Stock/Warehouses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,Name,Description,Address,Phone,AddedDate, IsReserve")] Warehouse warehouse)
        {
            if (id != warehouse.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!VerifyUniqueName(warehouse.Name, (int)warehouse.ID))
                    {
                        ModelState.AddModelError(nameof(warehouse.Name), _localizer["Name is already taken."]);
                        return View(warehouse);
                    }
                    if (!VerifyUniqueIsReserveWarehouse(warehouse))
                    {
                        ModelState.AddModelError(nameof(warehouse.IsReserve), _localizer["There is already a reserve warehouse."]);
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
            return View(warehouse);
        }

        // GET: Stock/Warehouses/Delete/5
        public async Task<IActionResult> Delete(long? id)
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
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var warehouse = await _context.Warehouses.SingleOrDefaultAsync(m => m.ID == id);
            if(warehouse != null)
            {

            }
            //_context.Warehouses.Remove(warehouse);
           // await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WarehouseExists(long id)
        {
            return _context.Warehouses.Any(e => e.ID == id);
        }

        #region Validations
        public bool VerifyDependencies(Warehouse model)
        {
            if (model.ID == 0)
                return true;
            var warehouse = _context.Warehouses.Include(w => w.PurchasedArticles).Include(w => w.OrderDetails).Where(w => w.ID == model.ID).FirstOrDefault();
            if (warehouse.OrderDetails.Count > 0 || warehouse.PurchasedArticles.Count > 0)
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

        public bool VerifyUniqueIsReserveWarehouse(Warehouse model)
        {
            if (model.IsReserve)
            {
                var warehouse = _context.Warehouses.Where(a => a.IsReserve == true).AsNoTracking().FirstOrDefault();
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
