using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Areas.Stock.Controllers
{
    [Area("Stock")]
    public class WarehousesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WarehousesController(ApplicationDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Create([Bind("Name,Description,Address,Phone")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                if (VerifyUniqueName(warehouse.Name, 0))
                {
                    _context.Add(warehouse);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(nameof(warehouse.Name), "Name is already taken.");
                }
                   
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
        public async Task<IActionResult> Edit(long id, [Bind("ID,Name,Description,Address,Phone,AddedDate")] Warehouse warehouse)
        {
            if (id != warehouse.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (VerifyUniqueName(warehouse.Name, (int)warehouse.ID))
                    {
                        _context.Update(warehouse);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(warehouse.Name), "Name is already taken.");
                        return View(warehouse);
                    }

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
            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WarehouseExists(long id)
        {
            return _context.Warehouses.Any(e => e.ID == id);
        }

        #region Validations
        public bool VerifyUniqueName(string name, int id)
        {

            var country = _context.Warehouses.Where(a => a.Name == name).AsNoTracking().FirstOrDefault();
            if (country != null)
            {
                if (id != 0)
                {
                    if (country.ID != id)
                        return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
