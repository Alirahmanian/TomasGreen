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
    public class CountriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CountriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stock/Countries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Countries.ToListAsync());
        }

        // GET: Stock/Countries/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .SingleOrDefaultAsync(m => m.ID == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Stock/Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stock/Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Country country)
        {
            if (ModelState.IsValid)
            {
                if (VerifyUniqueName(country.Name, 0))
                {
                    _context.Add(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(nameof(country.Name), "Name is already taken.");
                }
                 
            }
            return View(country);
        }

        // GET: Stock/Countries/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.SingleOrDefaultAsync(m => m.ID == id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Stock/Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,AddedDate,Name")] Country country)
        {
            if (id != country.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (VerifyUniqueName(country.Name, (int)country.ID))
                    {
                        _context.Update(country);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(country.Name), "Name is already taken.");
                        return View(country);

                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.ID))
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
            return View(country);
        }

        // GET: Stock/Countries/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .SingleOrDefaultAsync(m => m.ID == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Stock/Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var country = await _context.Countries.SingleOrDefaultAsync(m => m.ID == id);
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(long id)
        {
            return _context.Countries.Any(e => e.ID == id);
        }

        #region Validations
        public bool VerifyUniqueName(string name, int id)
        {

            var country = _context.Countries.Where(a => a.Name == name).AsNoTracking().FirstOrDefault();
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
