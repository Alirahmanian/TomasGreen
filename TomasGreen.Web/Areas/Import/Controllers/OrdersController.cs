using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;
using TomasGreen.Web.Areas.Import.ViewModels;
using TomasGreen.Web.Balances;
using TomasGreen.Web.Validations;
using TomasGreen.Web.Extensions;
using TomasGreen.Web.BaseModels;

namespace TomasGreen.Web.Areas.Import.Controllers
{
    [Area("Import")]
    public class OrdersController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public OrdersController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
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
                model.Order = _context.Orders.Where(o => o.ID == id).FirstOrDefault();
                model.Order.OrderDetails = _context.OrderDetails.Include(a => a.Article).Include(w => w.Warehouse).Where(d => d.OrderID == model.Order.ID).ToList();
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
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "ID", "FullName");
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
        public async Task<IActionResult> Create(SaveOrderViewModel model, string SaveOrder, string AddOrderDetail)
        {
            var articleBalance = new ArticleBalance(_context);
            if (!string.IsNullOrWhiteSpace(AddOrderDetail))
            {
                if (model.Order.ID == 0)
                {
                    ModelState.AddModelError("", "Please save the order header before adding articles to it.");
                    return View(model);

                }
                
            }
            if (!ModelState.IsValid)
            {
                AddOrderLists(model);
                return View(model);
            }
            var customModelValidator = OrderValidation.OrderIsValid(_context, model.Order);
            if (customModelValidator.Value == false)
            {
                ModelState.AddModelError(customModelValidator.Property, customModelValidator.Message);
                AddOrderLists(model);
                return View(model);
            }

            if (ModelState.IsValid)
            {
                if(model.Order.ID == 0)
                {
                    //new order
                    var guid = Guid.NewGuid();
                    model.Order.Guid = guid;
                    _context.Add(model.Order);
                    await _context.SaveChangesAsync();
                    var savedOrder = _context.Orders.Where(o => o.OrderDate == model.Order.OrderDate && o.CompanyID == model.Order.CompanyID && o.Guid == guid).FirstOrDefault();
                    if (savedOrder != null)
                    {
                        model.Order = savedOrder;
                        AddOrderLists(model);
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Could't save Order.");
                        AddOrderLists(model);
                        return View(model);
                    }
                   
                }
                else
                {
                    //update
                    _context.Update(model.Order);
                    if (!string.IsNullOrWhiteSpace(AddOrderDetail))
                    {
                        if(model.Order.ID != 0)
                        {
                            //var articlePackageWeight = _context.Articles.Where(a => a.ID == model.OrderDetail.ArticleID).FirstOrDefault()?.PackageWeight ?? 0;
                            //var totalWeight = (model.OrderDetail.QtyBoxes * articlePackageWeight) + (model.OrderDetail.QtyReserveBoxes * articlePackageWeight) + model.OrderDetail.QtyKg;
                            
                            var totalWeightOrPackage = OrderValidation.OrderDetailTotalWeightOrPackage(_context, model.OrderDetail);
                            model.OrderDetail.Extended_Price = model.OrderDetail.Price * totalWeightOrPackage;
                            if(model.OrderDetail.OrderID == 0)
                            {
                                model.OrderDetail.OrderID = model.Order.ID;
                            }
                            var customValidator = OrderValidation.OrderDetailIsValid(_context, model.OrderDetail);
                            if(!customValidator.Value)
                            {
                                ModelState.AddModelError("", "Couldn't saved.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", JSonHelper.ToJSon(customValidator));
                                }
                                AddOrderLists(model);
                                return View(model);
                            }
                            
                            var result = articleBalance.AddOrderDetailToBalance(model.OrderDetail);
                            if(result.Value == false)
                            {
                                ModelState.AddModelError("", "Couldn't saved.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", JSonHelper.ToJSon(result));
                                }
                                AddOrderLists(model);
                                return View(model);
                            }
                            _context.OrderDetails.Add(model.OrderDetail);
                            await _context.SaveChangesAsync();

                            AddOrderLists(model);
                            return View(model);
                        }
                        else
                        {
                            //
                            ModelState.AddModelError("", "Please save order before trying to add details.");
                            AddOrderLists(model);
                            return View(model);
                        }
                    }
                     
                   
                }
               
            }
            else
            {
                ModelState.AddModelError("", "Couldn't save.");
                AddOrderLists(model);
                return View(model);

            }
            AddOrderLists(model);
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

        private void AddOrderLists(SaveOrderViewModel model)
        {
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", model.Order.CompanyID);
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "ID", "FullName");
            ViewData["OrderTransportID"] = new SelectList(_context.OrderTranports, "ID", "Name");
            if(model.Order.ID > 0)
            {
                if (model.ArticleCategory != null)
                {
                    ViewData["ArticleID"] = new SelectList(_context.Articles.Where(a => a.ArticleCategoryID == model.ArticleCategory.ID), "ID", "Name");
                }
                if (model.OrderDetail != null && model.OrderDetail.ArticleID != 0)
                {
                    ViewData["WarehouseID"] = new SelectList((from aw in _context.ArticleWarehouseBalances
                                                              where aw.ArticleID == model.OrderDetail.ArticleID
                                                              join a in _context.Articles on aw.ArticleID equals a.ID
                                                              join w in _context.Warehouses on aw.WarehouseID equals w.ID
                                                              orderby w.Name
                                                              select new
                                                              {
                                                                  Id = w.ID,
                                                                  Name = w.Name,
                                                                  Articlesonhand = " [Box: " + aw.QtyPackagesOnhand.ToString() + ", Extra: " + aw.QtyExtraOnhand.ToString() + "]"
                                                              }
                                    ), "ID", "Name");
                }
            }
            
           
        }
       

        #region Ajax Calls
        public JsonResult GetArticleCategories()
        {
          
            var articleCategories = _context.ArticleCategories.Where(c => c.Archive != true).OrderBy(c => c.Name);
           
         
            return new JsonResult(articleCategories);
        }

        public JsonResult GetArticlesByCategoryId (int categoryId)
        {
            var articles = from a in _context.Articles where a.ArticleCategoryID == categoryId orderby a.Name select new { ID = a.ID, Name = a.Name };

           // var articles = _context.Articles.Where(a => a.ArticleCategoryID == categoryId).OrderBy(c => c.Name).ToList();
            return new JsonResult(articles.ToList());
        }
        public JsonResult GetWarehousesByArticleID(Int64 articleId)
        {
            var warehouses= (from aw in _context.ArticleWarehouseBalances where aw.ArticleID == articleId
                                 join a in _context.Articles on aw.ArticleID equals a.ID
                                 join w in _context.Warehouses on aw.WarehouseID equals w.ID
                                 orderby w.Name
                                 select new
                                 {
                                     Id = w.ID, Name = w.Name, 
                                     Articlesonhand = " [Package: " + aw.QtyPackagesOnhand.ToString() + ", Extra: " + aw.QtyExtraOnhand.ToString() + "]"
                                 }
                                );
            return new JsonResult(warehouses.ToList());
        }

        public ActionResult GetCompanyInfoForOrder(Int64 customerId)
        {
            var customer = _context.Companies.Where(c => c.ID == customerId).FirstOrDefault();
            return PartialView("_CustomerDetailsForOrderPartialView", customer);
        }

        public ActionResult GetArticleWarehouseBalance(Int64 articleId, Int64 warehouseId)
        {
            var balance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == articleId && b.WarehouseID == warehouseId).Include(a => a.Article).Include(w => w.Warehouse).FirstOrDefault();
            return PartialView("_ArticleWarehouseBalancePartialView", balance);
        }
        #endregion
    }
}
