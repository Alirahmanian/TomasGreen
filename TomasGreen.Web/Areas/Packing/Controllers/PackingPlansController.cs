using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TomasGreen.Model.Models;
using TomasGreen.Web.Areas.Packing.ViewModels;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Areas.Packing.Controllers
{
    [Area("Packing")]
    public class PackingPlansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PackingPlansController(ApplicationDbContext context)
        {
            _context = context;
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

            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name");
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "ID", "FullName");
            // ViewData["ArticleID"] = new SelectList(_context.Articles, "ID", "Name");
            // ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "Name");
            return View(model);
        }

        // POST: Packing/PackingPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PackingPlanViewModel model)
        {
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
    }
}
