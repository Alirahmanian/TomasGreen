using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;
using TomasGreen.Web.Areas.Sales.ViewModels;

namespace TomasGreen.Web.Areas.Sales.Controllers
{
    [Area("Sales")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sales/Orders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Orders.Include(o => o.Company);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sales/Orders/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Company)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Sales/Orders/Create
        public IActionResult Create(Int64? id)
        {
            var model = new SaveOrderViewModel();
            model.OrderDetail = new OrderDetail();
            if (id > 0)
            {
                model.Order = _context.Orders.Where(o => o.ID == id).Include(d => d.OrderDetails).FirstOrDefault();
                if(model.Order != null)
                {
                    model.OrderDetail.OrderID = model.Order.ID;
                }
            }
            else
            {
                model.Order = new Order();
                model.Order.OrderDate = DateTime.Today;
            }
           
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name");
            ViewData["OrderTransportID"] = new SelectList(_context.OrderTranports, "ID", "Name");
            ViewData["ArticleCategoryID"] = new SelectList(_context.ArticleCategories, "ID", "Name");
           // ViewData["ArticleID"] = new SelectList(_context.Articles, "ID", "Name");
           // ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "Name");
            return View(model);
        }

        // POST: Sales/Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveOrderViewModel model, string CreateOrder, string AddOrderDetail)
        {
            if (!string.IsNullOrWhiteSpace(CreateOrder))
            {
                ModelState.AddModelError("", "Create clicked");
            }
            else
            {
                ModelState.AddModelError("", "Add clicked");
            }
                if (!ModelState.IsValid)
            {
                ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", model.Order.CompanyID);
                ViewData["OrderTransportID"] = new SelectList(_context.OrderTranports, "ID", "Name");
                ViewData["ArticleID"] = new SelectList(_context.Articles, "ID", "Name");
                ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "Name");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                _context.Add(model.Order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {

            }
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", model.Order.CompanyID);
            ViewData["OrderTransportID"] = new SelectList(_context.OrderTranports, "ID", "Name");
            ViewData["ArticleCategoryID"] = new SelectList(_context.ArticleCategories, "ID", "Name");
            ViewData["ArticleID"] = new SelectList(_context.Articles, "ID", "Name");
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "Name");
            return View(model);
        }

        // GET: Sales/Orders/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", order.CompanyID);
            return View(order);
        }

        // POST: Sales/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,AddedDate,CompanyID,EmpoyeeID,AmountArticle,AmountReserve,TotalPrice,OrderDate,PaymentDate,PaidDate,LoadingDate,LoadedDate,AmountPaid,Coments,OrderdBy,PaymentWarning,ForcedPaid,OrderPaid,Cash")] Order order)
        {
            if (id != order.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.ID))
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
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", order.CompanyID);
            return View(order);
        }

        // GET: Sales/Orders/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Company)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Sales/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.ID == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(long id)
        {
            return _context.Orders.Any(e => e.ID == id);
        }

        #region Ajax Calls
        public JsonResult GetArticleCategories()
        {
          
            var articleCategories = _context.ArticleCategories.Where(c => c.Archive != true).OrderBy(c => c.Name);
           
         
            return new JsonResult(articleCategories);
        }

        public JsonResult GetArticlesByCategoryId (Int64 categoryId)
        {

            var articles = _context.Articles.Where(a => a.ArticleCategoryID == categoryId).OrderBy(c => c.Name).ToList();
            return new JsonResult(articles);
        }
        #endregion
    }
}
