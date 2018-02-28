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
    public class OrderTransportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderTransportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stock/OrderTransports
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderTranports.ToListAsync());
        }

        // GET: Stock/OrderTransports/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderTransport = await _context.OrderTranports
                .SingleOrDefaultAsync(m => m.ID == id);
            if (orderTransport == null)
            {
                return NotFound();
            }

            return View(orderTransport);
        }

        // GET: Stock/OrderTransports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stock/OrderTransports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] OrderTransport orderTransport)
        {
            if (ModelState.IsValid)
            {
                if (VerifyUniqueName(orderTransport.Name, 0))
                {
                    _context.Add(orderTransport);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(nameof(orderTransport.Name), "Name is already taken.");
                }
                    
            }
            return View(orderTransport);
        }

        // GET: Stock/OrderTransports/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderTransport = await _context.OrderTranports.SingleOrDefaultAsync(m => m.ID == id);
            if (orderTransport == null)
            {
                return NotFound();
            }
            return View(orderTransport);
        }

        // POST: Stock/OrderTransports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID, Name, AddedDate")] OrderTransport orderTransport)
        {
            if (id != orderTransport.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (VerifyUniqueName(orderTransport.Name, (int)orderTransport.ID))
                    {
                        _context.Update(orderTransport);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(orderTransport.Name), "Name is already taken.");
                        return View(orderTransport);
                    }
                       
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderTransportExists(orderTransport.ID))
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
            return View(orderTransport);
        }

        // GET: Stock/OrderTransports/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderTransport = await _context.OrderTranports
                .SingleOrDefaultAsync(m => m.ID == id);
            if (orderTransport == null)
            {
                return NotFound();
            }

            return View(orderTransport);
        }

        // POST: Stock/OrderTransports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var orderTransport = await _context.OrderTranports.SingleOrDefaultAsync(m => m.ID == id);
            _context.OrderTranports.Remove(orderTransport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderTransportExists(long id)
        {
            return _context.OrderTranports.Any(e => e.ID == id);
        }

        #region Validations
        public bool VerifyUniqueName(string name, int id)
        {

            var country = _context.OrderTranports.Where(a => a.Name == name).AsNoTracking().FirstOrDefault();
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
