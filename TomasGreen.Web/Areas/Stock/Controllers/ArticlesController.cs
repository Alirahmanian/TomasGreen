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
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stock/Articles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Articles.Include(a => a.ArticleCategory).Include(a => a.Country);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Stock/Articles/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.ArticleCategory)
                .Include(a => a.Country)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Stock/Articles/Create
        public IActionResult Create()
        {
            ViewData["ArticleCategoryID"] = new SelectList(_context.ArticleCategories, "ID", "Name");
            ViewData["CountryID"] = new SelectList(_context.Countries, "ID", "Name");
            return View();
        }

        // POST: Stock/Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,MinimumPrice,BoxWeight,ArticleCategoryID,CountryID")] Article article)
        {
            if (ModelState.IsValid)
            {
                if (VerifyUniqueName(article.Name, 0))
                {
                    _context.Add(article);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(nameof(article.Name), "Name is already taken.");
                }
            }
           
            ViewData["ArticleCategoryID"] = new SelectList(_context.ArticleCategories, "ID", "Name");
            ViewData["CountryID"] = new SelectList(_context.Countries, "ID", "Name", article.CountryID);
            return View(article);
        }

        // GET: Stock/Articles/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.SingleOrDefaultAsync(m => m.ID == id);
            if (article == null)
            {
                return NotFound();
            }
            ViewData["ArticleCategoryID"] = new SelectList(_context.ArticleCategories, "ID", "Name", article.ArticleCategoryID);
            ViewData["CountryID"] = new SelectList(_context.Countries, "ID", "Name", article.CountryID);
            return View(article);
        }

        // POST: Stock/Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,AddedDate,Name,Description,MinimumPrice,BoxWeight,ArticleCategoryID,CountryID")] Article article)
        {
            if (id != article.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(VerifyUniqueName(article.Name, (int)article.ID))
                    {
                        _context.Update(article);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(article.Name), "Name is already taken.");
                        ViewData["ArticleCategoryID"] = new SelectList(_context.ArticleCategories, "ID", "ID", article.ArticleCategoryID);
                        ViewData["CountryID"] = new SelectList(_context.Countries, "ID", "Name", article.CountryID);
                        return View(article);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ID))
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
            ViewData["ArticleCategoryID"] = new SelectList(_context.ArticleCategories, "ID", "ID", article.ArticleCategoryID);
            ViewData["CountryID"] = new SelectList(_context.Countries, "ID", "Name", article.CountryID);
            return View(article);
        }

        // GET: Stock/Articles/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.ArticleCategory)
                .Include(a => a.Country)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Stock/Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var article = await _context.Articles.SingleOrDefaultAsync(m => m.ID == id);
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(long id)
        {
            return _context.Articles.Any(e => e.ID == id);
        }
        #region Validations
        public bool VerifyUniqueName(string name, int id)
        {

            var article = _context.Articles.Where(a => a.Name == name).AsNoTracking().FirstOrDefault();
            if (article != null)
            {
                if (id != 0)
                {
                    if (article.ID != id)
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
