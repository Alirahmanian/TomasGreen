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

namespace TomasGreen.Web.Areas.Import.Controllers
{
    [Area("Import")]
    public class CompaniesController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<CompaniesController> _localizer;
        public CompaniesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<CompaniesController> localizer)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
        }

        // GET: Sales/Companies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Companies.ToListAsync());
        }

        // GET: Sales/Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .SingleOrDefaultAsync(m => m.ID == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Sales/Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sales/Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company company)
        {
            if (ModelState.IsValid)
            {
                if (NameAlreadyTaken(company))
                {
                    ModelState.AddModelError(nameof(company.Name), _localizer["Name is already taken."]);
                    return View(company);
                }

                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Sales/Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.SingleOrDefaultAsync(m => m.ID == id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Sales/Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,AddedDate,Name,Balance,Ruble,Discount,Purchases,SouldToUs,Paid,Received,LastBalance,LastBalanceDate,Locked,CreditReceived,CreditLimit")] Company company)
        {
            if (id != company.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.ID))
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
            return View(company);
        }

        // GET: Sales/Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .SingleOrDefaultAsync(m => m.ID == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Sales/Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Companies.SingleOrDefaultAsync(m => m.ID == id);
            if (company.IsOwner)
            {
                ViewBag.Error = _localizer["Couldn't delete. The Company is the owner."];
                return View(company);
            }
            if (IsRelated(company))
            {
                ViewBag.Error = _localizer["Couldn't delete. The Post is already related to other entities."];
                return View(company);
            }
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.ID == id);
        }
        #region
        private bool NameAlreadyTaken(Company model)
        {
            return _context.Companies.Any(f => f.Name == model.Name && f.ID != model.ID);
        }

        private bool IsRelated(Company model)
        {
            return _context.ArticleWarehouseBalances.Any(a => a.CompanyID == model.ID);
        }
        #endregion
    }
}
