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
    public class PurchasedArticleWarehousesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchasedArticleWarehousesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stock/PurchasedArticleWarehouses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PurchasedArticleWarehouses.Include(r => r.PurchasedArticle).Include(r => r.Warehouse);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Stock/PurchasedArticleWarehouses/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var PurchasedArticleWarehouse = await _context.PurchasedArticleWarehouses
                .Include(r => r.PurchasedArticle)
                .Include(r => r.Warehouse)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (PurchasedArticleWarehouse == null)
            {
                return NotFound();
            }

            return View(PurchasedArticleWarehouse);
        }

        // GET: Stock/PurchasedArticleWarehouses/Create
        public IActionResult Create()
        {
            ViewData["PurchasedArticleID"] = new SelectList(_context.PurchasedArticles, "ID", "ID");
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "Name");
            return View();
        }

        // POST: Stock/PurchasedArticleWarehouses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchasedArticleID,WarehouseID,QtyBoxes,QtyExtraKg")] PurchasedArticleWarehouse purchasedArticleWarehouse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchasedArticleWarehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PurchasedArticleID"] = new SelectList(_context.PurchasedArticles, "ID", "ID", purchasedArticleWarehouse.PurchasedArticleID);
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "Name", purchasedArticleWarehouse.WarehouseID);
            return View(purchasedArticleWarehouse);
        }

        // GET: Stock/PurchasedArticleWarehouses/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var PurchasedArticleWarehouse = await _context.PurchasedArticleWarehouses.SingleOrDefaultAsync(m => m.ID == id);
            if (PurchasedArticleWarehouse == null)
            {
                return NotFound();
            }
            ViewData["PurchasedArticleID"] = new SelectList(_context.PurchasedArticles, "ID", "ID", PurchasedArticleWarehouse.PurchasedArticleID);
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "Name", PurchasedArticleWarehouse.WarehouseID);
            return View(PurchasedArticleWarehouse);
        }

        // POST: Stock/PurchasedArticleWarehouses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("PurchasedArticleID,WarehouseID,QtyBoxes,QtyExtraKg")] PurchasedArticleWarehouse PurchasedArticleWarehouse)
        {
            if (id != PurchasedArticleWarehouse.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(PurchasedArticleWarehouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchasedArticleWarehouseExists(PurchasedArticleWarehouse.ID))
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
            ViewData["PurchasedArticleID"] = new SelectList(_context.PurchasedArticles, "ID", "ID", PurchasedArticleWarehouse.PurchasedArticleID);
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "Name", PurchasedArticleWarehouse.WarehouseID);
            return View(PurchasedArticleWarehouse);
        }

        // GET: Stock/PurchasedArticleWarehouses/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var PurchasedArticleWarehouse = await _context.PurchasedArticleWarehouses
                .Include(r => r.PurchasedArticle)
                .Include(r => r.Warehouse)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (PurchasedArticleWarehouse == null)
            {
                return NotFound();
            }

            return View(PurchasedArticleWarehouse);
        }

        // POST: Stock/PurchasedArticleWarehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var PurchasedArticleWarehouse = await _context.PurchasedArticleWarehouses.SingleOrDefaultAsync(m => m.ID == id);
            _context.PurchasedArticleWarehouses.Remove(PurchasedArticleWarehouse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchasedArticleWarehouseExists(long id)
        {
            return _context.PurchasedArticleWarehouses.Any(e => e.ID == id);
        }
    }
}
