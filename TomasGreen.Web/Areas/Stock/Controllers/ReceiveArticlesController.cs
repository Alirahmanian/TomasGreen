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
    public class ReceiveArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReceiveArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stock/ReceiveArticles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ReceiveArticles.Include(r => r.Article).Include(r => r.Company);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Stock/ReceiveArticles/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiveArticle = await _context.ReceiveArticles
                .Include(r => r.Article)
                .Include(r => r.Company)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (receiveArticle == null)
            {
                return NotFound();
            }

            return View(receiveArticle);
        }

        // GET: Stock/ReceiveArticles/Create
        public IActionResult Create()
        {
            ViewData["ArticleID"] = new SelectList(_context.Articles, "ID", "Name");
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name");
            return View();
        }

        // POST: Stock/ReceiveArticles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,ArticleID,CompanyID,Description,WarehouseID,QtyBoxes,QtyKg")] ReceiveArticle receiveArticle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receiveArticle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArticleID"] = new SelectList(_context.Articles, "ID", "Name", receiveArticle.ArticleID);
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", receiveArticle.CompanyID);
            return View(receiveArticle);
        }

        // GET: Stock/ReceiveArticles/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiveArticle = await _context.ReceiveArticles.SingleOrDefaultAsync(m => m.ID == id);
            if (receiveArticle == null)
            {
                return NotFound();
            }
            ViewData["ArticleID"] = new SelectList(_context.Articles, "ID", "Name", receiveArticle.ArticleID);
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", receiveArticle.CompanyID);
            return View(receiveArticle);
        }

        // POST: Stock/ReceiveArticles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,AddedDate,Date,ArticleID,CompanyID,Description,WarehouseID,QtyBoxes,QtyKg")] ReceiveArticle receiveArticle)
        {
            if (id != receiveArticle.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receiveArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceiveArticleExists(receiveArticle.ID))
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
            ViewData["ArticleID"] = new SelectList(_context.Articles, "ID", "Name", receiveArticle.ArticleID);
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", receiveArticle.CompanyID);
            return View(receiveArticle);
        }

        // GET: Stock/ReceiveArticles/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiveArticle = await _context.ReceiveArticles
                .Include(r => r.Article)
                .Include(r => r.Company)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (receiveArticle == null)
            {
                return NotFound();
            }

            return View(receiveArticle);
        }

        // POST: Stock/ReceiveArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var receiveArticle = await _context.ReceiveArticles.SingleOrDefaultAsync(m => m.ID == id);
            _context.ReceiveArticles.Remove(receiveArticle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceiveArticleExists(long id)
        {
            return _context.ReceiveArticles.Any(e => e.ID == id);
        }
    }
}
