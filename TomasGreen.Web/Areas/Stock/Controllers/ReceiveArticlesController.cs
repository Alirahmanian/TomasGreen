using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TomasGreen.Model.Models;
using TomasGreen.Web.Areas.Stock.ViewModels;
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
        public IActionResult Index()
        {
            var receiveArticles = GetIndexList();
            


            return View(receiveArticles);
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
        public async Task<IActionResult> Create([Bind("Date,ArticleID,CompanyID,Description")] ReceiveArticle receiveArticle)
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
        public async Task<IActionResult> Edit(long id, [Bind("Date,ArticleID,CompanyID,Description")] ReceiveArticle receiveArticle)
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

        #region 
        private List<ReceiveArticleViewModel> GetIndexList()
        {
            var receiveArticles = _context.ReceiveArticles.Include(r => r.Article).Include(r => r.Company).Include(r => r.Warehouses).OrderBy(r => r.Date).ThenBy(r => r.Article.Name).ThenBy(r => r.Company.Name);
            var resultList = new List<ReceiveArticleViewModel>();
            foreach (var receiveArticle in receiveArticles)
            {
                var receiveArticleViewModel = new ReceiveArticleViewModel();
                receiveArticleViewModel.ID = (int)receiveArticle.ID;
                receiveArticleViewModel.Article = receiveArticle.Article;
                receiveArticleViewModel.Company = receiveArticle.Company ?? new Company();
                receiveArticleViewModel.Date = receiveArticle.Date;
                Dictionary<string, decimal> tempList = new Dictionary<string, decimal>();
                receiveArticleViewModel.Warehouses = new Dictionary<string, decimal>();
                var warehouses = _context.ReceiveArticleWarehouses.Where(r => r.ReceiveArticleID == receiveArticle.ID).Include(w => w.Warehouse);
                foreach (var warehouse in warehouses)
                {
                    receiveArticleViewModel.TotalWeight += (warehouse.QtyBoxes * receiveArticle.Article.BoxWeight) + warehouse.QtyExtraKg;

                    if (receiveArticleViewModel.WarehouseSummary != null)
                        receiveArticleViewModel.WarehouseSummary += "|";
                    var tempWarehouse = _context.Warehouses.Where(w => w.ID == warehouse.WarehouseID).FirstOrDefault();
                    if (tempWarehouse != null)
                    {
                        receiveArticleViewModel.Warehouses.Add(tempWarehouse.Name, (warehouse.QtyBoxes * receiveArticle.Article.BoxWeight) + warehouse.QtyExtraKg);
                        receiveArticleViewModel.WarehouseSummary += tempWarehouse.Name + ":" + ((warehouse.QtyBoxes * receiveArticle.Article.BoxWeight) + warehouse.QtyExtraKg).ToString();

                    }

                }
                resultList.Add(receiveArticleViewModel);
            }
            return resultList;
        }


        #endregion
    }
}
