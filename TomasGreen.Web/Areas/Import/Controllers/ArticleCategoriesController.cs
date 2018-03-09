﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Areas.Import.Controllers
{
    [Area("Import")]
    public class ArticleCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArticleCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stock/ArticleCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.ArticleCategories.ToListAsync());
        }

        // GET: Stock/ArticleCategories/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleCategory = await _context.ArticleCategories
                .SingleOrDefaultAsync(m => m.ID == id);
            if (articleCategory == null)
            {
                return NotFound();
            }

            return View(articleCategory);
        }

        // GET: Stock/ArticleCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stock/ArticleCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] ArticleCategory articleCategory)
        {
            if (ModelState.IsValid)
            {
                if (VerifyUniqueName(articleCategory.Name, 0))
                {
                    _context.Add(articleCategory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(nameof(articleCategory.Name), "Name is already taken.");
                }
                    
            }
            return View(articleCategory);
        }

        // GET: Stock/ArticleCategories/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleCategory = await _context.ArticleCategories.SingleOrDefaultAsync(m => m.ID == id);
            if (articleCategory == null)
            {
                return NotFound();
            }
            return View(articleCategory);
        }

        // POST: Stock/ArticleCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,AddedDate,Name")] ArticleCategory articleCategory)
        {
            if (id != articleCategory.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (VerifyUniqueName(articleCategory.Name, (int)articleCategory.ID))
                    {
                        _context.Update(articleCategory);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(articleCategory.Name), "Name is already taken.");
                        return View(articleCategory);

                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleCategoryExists(articleCategory.ID))
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
            return View(articleCategory);
        }

        // GET: Stock/ArticleCategories/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleCategory = await _context.ArticleCategories
                .SingleOrDefaultAsync(m => m.ID == id);
            if (articleCategory == null)
            {
                return NotFound();
            }

            return View(articleCategory);
        }

        // POST: Stock/ArticleCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var articleCategory = await _context.ArticleCategories.SingleOrDefaultAsync(m => m.ID == id);
            _context.ArticleCategories.Remove(articleCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleCategoryExists(long id)
        {
            return _context.ArticleCategories.Any(e => e.ID == id);
        }

        #region Validations
        public bool VerifyUniqueName(string name, int id)
        {

            var articleCategory = _context.ArticleCategories.Where(a => a.Name == name).AsNoTracking().FirstOrDefault();
            if (articleCategory != null)
            {
                if (id != 0)
                {
                    if (articleCategory.ID != id)
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