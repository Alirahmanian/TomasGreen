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
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Areas.Import.Controllers
{
    [Area("Import")]
    public class CompanySectionsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<CompanySectionsController> _localizer;
        public CompanySectionsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<CompanySectionsController> localizer)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
        }


        // GET: Import/CompanySections
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CompanySections.Include(c => c.Company);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Import/CompanySections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companySection = await _context.CompanySections
                .Include(c => c.Company)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (companySection == null)
            {
                return NotFound();
            }

            return View(companySection);
        }

        // GET: Import/CompanySections/Create
        public IActionResult Create()
        {
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name");
            return View();
        }

        // POST: Import/CompanySections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyID,Name")] CompanySection companySection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(companySection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", companySection.CompanyID);
            return View(companySection);
        }

        // GET: Import/CompanySections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companySection = await _context.CompanySections.SingleOrDefaultAsync(m => m.ID == id);
            if (companySection == null)
            {
                return NotFound();
            }
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", companySection.CompanyID);
            return View(companySection);
        }

        // POST: Import/CompanySections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyID,Name")] CompanySection companySection)
        {
            if (id != companySection.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companySection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanySectionExists(companySection.ID))
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
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", companySection.CompanyID);
            return View(companySection);
        }

        // GET: Import/CompanySections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companySection = await _context.CompanySections
                .Include(c => c.Company)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (companySection == null)
            {
                return NotFound();
            }

            return View(companySection);
        }

        // POST: Import/CompanySections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companySection = await _context.CompanySections.SingleOrDefaultAsync(m => m.ID == id);
            _context.CompanySections.Remove(companySection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanySectionExists(int id)
        {
            return _context.CompanySections.Any(e => e.ID == id);
        }
    }
}
