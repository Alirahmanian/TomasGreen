using System;
using System.Collections;
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
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<ArticlesController> _localizer;
        public ArticlesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<ArticlesController> localizer)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
        }

        // GET: Stock/Articles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Articles.Include(a => a.ArticleCategory).Include(a => a.Country).Include(u => u.ArticleUnit).Include(p => p.ArticlePackageForm);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Stock/Articles/Details/5
        public async Task<IActionResult> Details(int? id)
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
            GetArticleSaveLists(new Article());
            return View();
        }

        // POST: Stock/Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article)
        {
            if (ModelState.IsValid)
            {
                if (VerifyUniqueArticle(article))
                {
                    ModelState.AddModelError(nameof(article.Name), _localizer["There is already an article with the same name and unit."]);
                    GetArticleSaveLists(article);
                    return View(article);
                }
               
                if (!VerifyUnitAndWeightPerPackage(article))
                {
                    ModelState.AddModelError(nameof(article.WeightPerPackage), _localizer["The choosen unit requires a value more than zero for this field."]);
                    GetArticleSaveLists(article);
                    return View(article);

                }
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                GetArticleSaveLists(article);
                return View(article);
            }
           
        }

        // GET: Stock/Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            GetArticleSaveLists(article);
            return View(article);
        }

        // POST: Stock/Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Article article)
        {
            if (id != article.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(VerifyUniqueArticle(article))
                    {
                        ModelState.AddModelError(nameof(article.Name), _localizer["There is already an article with the same name and unit."]);
                        GetArticleSaveLists(article);
                        return View(article);
                        
                    }
                    if (!VerifyUnitAndWeightPerPackage(article))
                    {
                        ModelState.AddModelError(nameof(article.WeightPerPackage), _localizer["The choosen unit requires a value more than zero for this field."]);
                        GetArticleSaveLists(article);
                        return View(article);

                    }
                    _context.Update(article);
                    await _context.SaveChangesAsync();

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
            GetArticleSaveLists(article);
            return View(article);
        }

        // GET: Stock/Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.SingleOrDefaultAsync(m => m.ID == id);
            if (IsRelated(article))
            {
                ModelState.AddModelError("", _localizer["Couldn't delete. The post is already related to other entities."]);
                return View(article);
            }
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ID == id);
        }
        #region Validations
        private bool VerifyUniqueName(Article model)
        {
          return _context.Articles.Any(a => a.Name == model.Name && a.ID != model.ID); ;
        }

        private bool VerifyUniqueArticle(Article model)
        {
            return _context.Articles.Any(a => a.Name == model.Name && a.ArticleUnitID == model.ArticleUnitID && a.ID != model.ID);
        }

        private bool VerifyUnitAndWeightPerPackage(Article model)
        {
            if(model.ArticleUnitID != 0)
            {
                if(_context.ArticleUnits.Any(u => u.ID == model.ArticleUnitID && u.MeasuresByKg == true))
                {
                    if (model.WeightPerPackage <= 0)
                        return false;
                }
            }
            return true;
        }
        private void GetArticleSaveLists(Article model)
        {
            ViewData["ArticleCategoryID"] = new SelectList(_context.ArticleCategories, "ID", "Name", model?.ArticleCategoryID);
            ViewData["CountryID"] = new SelectList(_context.Countries, "ID", "Name", model?.CountryID);
            ViewData["ArticleUNitID"] = new SelectList(_context.ArticleUnits, "ID", "Name", model.ArticleUnitID);
            ViewData["ArticlePackageFormID"] = new SelectList(_context.ArticlePakageForms, "ID", "Name", model.ArticlePackageFormID);
        }
        private bool IsRelated(Article model)
        {
            return Dependencies.CheckRelatedRecords(_context, "Articles", "ArticleID", model.ID);
        }

        #endregion
    }
}
