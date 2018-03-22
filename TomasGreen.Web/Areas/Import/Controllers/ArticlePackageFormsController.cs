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
    public class ArticlePackageFormsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<ArticlePackageFormsController> _localizer;
        public ArticlePackageFormsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<ArticlePackageFormsController> localizer)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
        }

        // GET: Import/ArticlePackageForms
        public async Task<IActionResult> Index()
        {
            return View(await _context.ArticlePakageForms.Include(a => a.Articles).ToListAsync());
        }

        // GET: Import/ArticlePackageForms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articlePackageForm = await _context.ArticlePakageForms
                .SingleOrDefaultAsync(m => m.ID == id);
            if (articlePackageForm == null)
            {
                return NotFound();
            }

            return View(articlePackageForm);
        }

        // GET: Import/ArticlePackageForms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Import/ArticlePackageForms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID, Name")] ArticlePackageForm articlePackageForm)
        {
            if (ModelState.IsValid)
            {
                if (NameAlreadyTaken(articlePackageForm))
                {
                    ModelState.AddModelError(nameof(articlePackageForm.Name), _localizer["Name is already taken."]);
                    return View(articlePackageForm);
                }
                _context.Add(articlePackageForm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(articlePackageForm);
        }

        // GET: Import/ArticlePackageForms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articlePackageForm = await _context.ArticlePakageForms.SingleOrDefaultAsync(m => m.ID == id);
            if (articlePackageForm == null)
            {
                return NotFound();
            }
            return View(articlePackageForm);
        }

        // POST: Import/ArticlePackageForms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID, Name")] ArticlePackageForm articlePackageForm)
        {
            if (id != articlePackageForm.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(NameAlreadyTaken(articlePackageForm))
                    {
                        ModelState.AddModelError(nameof(articlePackageForm.Name), _localizer["Name is already taken."]);
                        return View(articlePackageForm);
                    }
                    _context.Update(articlePackageForm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticlePackageFormExists(articlePackageForm.ID))
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
            return View(articlePackageForm);
        }

        // GET: Import/ArticlePackageForms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articlePackageForm = await _context.ArticlePakageForms
                .SingleOrDefaultAsync(m => m.ID == id);
            if (articlePackageForm == null)
            {
                return NotFound();
            }

            return View(articlePackageForm);
        }

        // POST: Import/ArticlePackageForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articlePackageForm = await _context.ArticlePakageForms.SingleOrDefaultAsync(m => m.ID == id);
            if (IsRelated(articlePackageForm))
            {
                ModelState.AddModelError("", _localizer["Couldn't delete. The Post is already related to other entities."]);
                return View(articlePackageForm);
            }
            _context.ArticlePakageForms.Remove(articlePackageForm);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticlePackageFormExists(int id)
        {
            return _context.ArticlePakageForms.Any(e => e.ID == id);
        }
        #region
        private bool NameAlreadyTaken(ArticlePackageForm model)
        {
            return _context.ArticlePakageForms.Any(f => f.Name == model.Name && f.ID != model.ID);
        }

        private bool IsRelated(ArticlePackageForm model)
        {
            return _context.Articles.Any(a => a.ArticlePackageFormID == model.ID);
        }
        #endregion
    }
}
