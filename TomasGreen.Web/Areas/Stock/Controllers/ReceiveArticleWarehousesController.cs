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
    public class ReceiveArticleWarehousesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReceiveArticleWarehousesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stock/ReceiveArticleWarehouses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ReceiveArticleWarehouses.Include(r => r.ReceiveArticle).Include(r => r.Warehouse);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Stock/ReceiveArticleWarehouses/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiveArticleWarehouse = await _context.ReceiveArticleWarehouses
                .Include(r => r.ReceiveArticle)
                .Include(r => r.Warehouse)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (receiveArticleWarehouse == null)
            {
                return NotFound();
            }

            return View(receiveArticleWarehouse);
        }

        // GET: Stock/ReceiveArticleWarehouses/Create
        public IActionResult Create()
        {
            ViewData["ReceiveArticleID"] = new SelectList(_context.ReceiveArticles, "ID", "ID");
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "Name");
            return View();
        }

        // POST: Stock/ReceiveArticleWarehouses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReceiveArticleID,WarehouseID,QtyBoxes,QtyExtraKg")] ReceiveArticleWarehouse receiveArticleWarehouse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receiveArticleWarehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReceiveArticleID"] = new SelectList(_context.ReceiveArticles, "ID", "ID", receiveArticleWarehouse.ReceiveArticleID);
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "Name", receiveArticleWarehouse.WarehouseID);
            return View(receiveArticleWarehouse);
        }

        // GET: Stock/ReceiveArticleWarehouses/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiveArticleWarehouse = await _context.ReceiveArticleWarehouses.SingleOrDefaultAsync(m => m.ID == id);
            if (receiveArticleWarehouse == null)
            {
                return NotFound();
            }
            ViewData["ReceiveArticleID"] = new SelectList(_context.ReceiveArticles, "ID", "ID", receiveArticleWarehouse.ReceiveArticleID);
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "Name", receiveArticleWarehouse.WarehouseID);
            return View(receiveArticleWarehouse);
        }

        // POST: Stock/ReceiveArticleWarehouses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ReceiveArticleID,WarehouseID,QtyBoxes,QtyExtraKg")] ReceiveArticleWarehouse receiveArticleWarehouse)
        {
            if (id != receiveArticleWarehouse.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receiveArticleWarehouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceiveArticleWarehouseExists(receiveArticleWarehouse.ID))
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
            ViewData["ReceiveArticleID"] = new SelectList(_context.ReceiveArticles, "ID", "ID", receiveArticleWarehouse.ReceiveArticleID);
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "Name", receiveArticleWarehouse.WarehouseID);
            return View(receiveArticleWarehouse);
        }

        // GET: Stock/ReceiveArticleWarehouses/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiveArticleWarehouse = await _context.ReceiveArticleWarehouses
                .Include(r => r.ReceiveArticle)
                .Include(r => r.Warehouse)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (receiveArticleWarehouse == null)
            {
                return NotFound();
            }

            return View(receiveArticleWarehouse);
        }

        // POST: Stock/ReceiveArticleWarehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var receiveArticleWarehouse = await _context.ReceiveArticleWarehouses.SingleOrDefaultAsync(m => m.ID == id);
            _context.ReceiveArticleWarehouses.Remove(receiveArticleWarehouse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceiveArticleWarehouseExists(long id)
        {
            return _context.ReceiveArticleWarehouses.Any(e => e.ID == id);
        }
    }
}
