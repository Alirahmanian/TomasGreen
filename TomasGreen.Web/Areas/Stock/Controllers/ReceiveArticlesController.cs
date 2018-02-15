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
using TomasGreen.Web.Validations;


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

        public IActionResult SaveReceiveArticleWarehouse(Int64? id)
        {
            var model = new SaveReceiveArticleWarehouseViewModel();
            if(id > 0)
            {
                model.ReceiveArticle = _context.ReceiveArticles.Where(r => r.ID == id).FirstOrDefault();
                
            }
            model.ReceiveArticle = new ReceiveArticle();
            model.ReceiveArticle.Date = DateTime.Today;
            
            model.ReceiveArticleWarehouses = GetReceiveArticleWarehouse(id ?? 0);
            ViewData["ArticleID"] = new SelectList(_context.Articles, "ID", "Name");
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveReceiveArticleWarehouse(SaveReceiveArticleWarehouseViewModel model)
        {
            //
            var skipped = ModelState.Keys.Where(key => key.StartsWith("ReceiveArticle.ID") || key.StartsWith("ReceiveArticle.CompanyID"));
            foreach (var key in skipped)
            {
                ModelState.Remove(key);
            }
            if(!ModelState.IsValid)
            {
                var columValidatedMessage = ReceiveArticleValidation.ReceivArticleIsValid(_context, model.ReceiveArticle);
                if (!columValidatedMessage.Value)
                {
                    ModelState.AddModelError(columValidatedMessage.Property, columValidatedMessage.Message);
                    return View(model);
                }
            }

            if (!ModelState.IsValid)
                {
                    model.ReceiveArticleWarehouses = GetReceiveArticleWarehouse((long)model.ReceiveArticle?.ID);
                    ViewData["ArticleID"] = new SelectList(_context.Articles, "ID", "Name", model.ReceiveArticle.ArticleID);
                    ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", model.ReceiveArticle.CompanyID);
                    return View(model);
                }
                if(model.ReceiveArticle.ID == 0)
                {
                    var guid = Guid.NewGuid();
                    model.ReceiveArticle.Guid = guid;
                    _context.Add(model.ReceiveArticle);
                    await _context.SaveChangesAsync();
                    var savedReceiveArticle = _context.ReceiveArticles.Where(r => r.Date == model.ReceiveArticle.Date && r.ArticleID == r.ArticleID && r.Guid == guid).FirstOrDefault();
                    if(savedReceiveArticle != null)
                    {
                      foreach(var receiveArticleWarehouse in model.ReceiveArticleWarehouses)
                      {
                        if(receiveArticleWarehouse.QtyBoxes > 0 || receiveArticleWarehouse.QtyExtraKg > 0)
                        {
                            receiveArticleWarehouse.ReceiveArticleID = savedReceiveArticle.ID;
                            _context.Add(receiveArticleWarehouse);
                        }
                      }
                      await _context.SaveChangesAsync();
                    }

                  return RedirectToAction(nameof(Index));
                }
                else
                {
                    var savedReceiveArticle = _context.ReceiveArticles.Where(r => r.ID == model.ReceiveArticle.ID).FirstOrDefault();
                    if (savedReceiveArticle != null)
                    {
                        if (savedReceiveArticle.ArticleID != model.ReceiveArticle.ArticleID)
                        {
                            //rollBack ArticleWarehouseBalance
                        }
                        if(savedReceiveArticle.CompanyID != model.ReceiveArticle.CompanyID)
                        {
                          // rollBack Company balance
                        }
                        _context.Update(model.ReceiveArticle);
                        foreach (var receiveArticleWarehouse in model.ReceiveArticleWarehouses)
                        {
                            if (receiveArticleWarehouse.ReceiveArticleID != 0)
                            {

                            }
                            else
                            {

                            }
                        }
                        await _context.SaveChangesAsync();
                    }
                   
                }
               
               // return RedirectToAction(nameof(Index));
            
            
            ViewData["ArticleID"] = new SelectList(_context.Articles, "ID", "Name", model.ReceiveArticle.ArticleID);
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", model.ReceiveArticle.CompanyID);
            return View(model);
        }

        // GET: Stock/ReceiveArticles/Create
        public IActionResult Create()
        {
            ViewData["ArticleID"] = new SelectList(_context.Articles, "ID", "Name");
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name");
            ViewBag.ReceivArticleWarehouse = GetReceiveArticleWarehouse(0);
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
            var receiveArticles = _context.ReceiveArticles.Include(r => r.Article).Include(r => r.Company).Include(r => r.Warehouses).OrderBy(r => r.Date).ThenBy(r => r.Article.Name).ThenBy(r => r.Company.Name).AsNoTracking().ToList();
            var resultList = new List<ReceiveArticleViewModel>();
            foreach (var receiveArticle in receiveArticles)
            {
                var receiveArticleViewModel = new ReceiveArticleViewModel
                {
                    ID = (int)receiveArticle.ID,
                    Article = receiveArticle.Article,
                    Company = receiveArticle.Company ?? new Company(),
                    Date = receiveArticle.Date
                };
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

        private List<ReceiveArticleWarehouse> GetReceiveArticleWarehouse(Int64 id = 0)
        {
            var result = new List<ReceiveArticleWarehouse>();
            
            foreach(var warehouse in _context.Warehouses.OrderBy(w => w.Name))
                {
                    var tempReceiveArticleWarehouse = new ReceiveArticleWarehouse();
                    if (id != 0)
                    {
                        var savedReceiveArticleWarehouse = _context.ReceiveArticleWarehouses.Where(r => r.ReceiveArticleID == id && r.WarehouseID == warehouse.ID).FirstOrDefault();
                        if(savedReceiveArticleWarehouse != null)
                        {
                           // tempReceiveArticleWarehouse.ID = savedReceiveArticleWarehouse.ID;
                            tempReceiveArticleWarehouse.ReceiveArticleID = savedReceiveArticleWarehouse.ReceiveArticleID;
                            tempReceiveArticleWarehouse.WarehouseID = warehouse.ID;
                            tempReceiveArticleWarehouse.Warehouse = warehouse;
                            tempReceiveArticleWarehouse.QtyBoxes = savedReceiveArticleWarehouse.QtyBoxes;
                            tempReceiveArticleWarehouse.QtyExtraKg = savedReceiveArticleWarehouse.QtyExtraKg;
                        }
                        else
                        {
                            //tempReceiveArticleWarehouse.ID = 0;
                            tempReceiveArticleWarehouse.ReceiveArticleID = 0;
                            tempReceiveArticleWarehouse.WarehouseID = warehouse.ID;
                            tempReceiveArticleWarehouse.Warehouse = warehouse;
                            tempReceiveArticleWarehouse.QtyBoxes = 0;
                            tempReceiveArticleWarehouse.QtyExtraKg = 0;
                        }
                         

                        
                    }
                    else
                    {
                        // tempReceiveArticleWarehouse.ID = 0;
                         tempReceiveArticleWarehouse.ReceiveArticleID = 0;
                         tempReceiveArticleWarehouse.WarehouseID = warehouse.ID;
                         tempReceiveArticleWarehouse.Warehouse = warehouse;
                         tempReceiveArticleWarehouse.QtyBoxes = 0;
                         tempReceiveArticleWarehouse.QtyExtraKg = 0;
                    }
                 
                
                    
                    result.Add(tempReceiveArticleWarehouse);
                }
           
            return result;
        }

        #endregion
    }
}
